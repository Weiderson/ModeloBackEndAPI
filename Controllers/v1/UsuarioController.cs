using Asp.Versioning;
using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Interfaces;
using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DesafioBackendAPI.Controllers.v1;

[ApiController]
[AllowAnonymous]
[Route("api/v{version:apiVersion}/Usuario")]
[ApiVersion("1.0")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _repository;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(
        IUsuarioRepository repository,
        IMapper mapper,
        ITokenService tokenService,
        ILogger<UsuarioController> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving users.");
        var users = (await _repository.GetAsync(cancellationToken).ConfigureAwait(false)).ToList();
        if (users.Count == 0)
            return NotFound("Lista de usuários é nula ou vazia.");

        return Ok(_mapper.Map<IEnumerable<UsuarioDTO>>(users));
    }

    [HttpGet("{email}")]
    public async Task<IActionResult> Get(string email, CancellationToken cancellationToken)
    {
        var user = await _repository.GetAsync(email, cancellationToken).ConfigureAwait(false);
        if (user == null)
            return NotFound("Usuário não encontrado.");

        return Ok(_mapper.Map<UsuarioDTO>(user));
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        [FromQuery] UsuarioDTO usuarioDTO,
        CancellationToken cancellationToken)
    {
        if (usuarioDTO == null)
            return BadRequest("Dados inválidos.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Email))
            return BadRequest("Email inválido.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Senha))
            return BadRequest("Senha inválida.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Nome))
            return BadRequest("Nome inválido.");

        var existingUser = await _repository
            .UsuarioExisteAsync(usuarioDTO.Email, cancellationToken)
            .ConfigureAwait(false);
        if (existingUser != null)
            return BadRequest("Usuário já existe.");

        var usuario = _mapper.Map<Usuario>(usuarioDTO);
        usuario.Id = null;
        usuario.DtInclusao = DateTime.Now;
        SetPasswordHash(usuario, usuarioDTO.Senha);

        await _repository.AddAsync(usuario, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("User {UserId} created.", usuario.Id);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromQuery] UsuarioDTO usuarioDTO,
        CancellationToken cancellationToken)
    {
        if (usuarioDTO == null)
            return BadRequest("Dados inválidos.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Email))
            return BadRequest("Email inválido.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Senha))
            return BadRequest("Senha inválida.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Nome))
            return BadRequest("Nome inválido.");

        var usuario = await _repository
            .GetTrackedAsync(usuarioDTO.Email, cancellationToken)
            .ConfigureAwait(false);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        usuario.Nome = usuarioDTO.Nome;
        usuario.DtAlteracao = DateTime.Now;
        SetPasswordHash(usuario, usuarioDTO.Senha);

        await _repository.UpdateAsync(usuario, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("User {UserId} updated.", usuario.Id);
        return Ok();
    }

    [HttpDelete("{email}")]
    public async Task<IActionResult> Delete(string email, CancellationToken cancellationToken)
    {
        var usuario = await _repository.GetTrackedAsync(email, cancellationToken).ConfigureAwait(false);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        await _repository.DeleteAsync(usuario, cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("User {UserId} deleted.", usuario.Id);
        return Ok();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(
        [FromQuery] UsuarioDTO usuarioDTO,
        CancellationToken cancellationToken)
    {
        if (usuarioDTO == null)
            return BadRequest("Dados inválidos.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Email))
            return BadRequest("Email inválido.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Senha))
            return BadRequest("Senha inválida.");

        var usuario = await _repository
            .UsuarioExisteExcluidoAsync(usuarioDTO.Email, cancellationToken)
            .ConfigureAwait(false);
        if (usuario?.SenhaSalt == null || usuario.SenhaHash == null)
            return BadRequest("Usuário inexistente ou excluído.");

        using var hmac = new HMACSHA256(usuario.SenhaSalt);
        var result = hmac.ComputeHash(Encoding.UTF8.GetBytes(usuarioDTO.Senha));
        if (!CryptographicOperations.FixedTimeEquals(usuario.SenhaHash, result))
            return BadRequest("Senha inválida.");

        var token = _tokenService.CriarToken(usuario);
        _logger.LogInformation("User login succeeded for user {UserId}.", usuario.Id);
        return Ok(new { Token = token });
    }

    private static void SetPasswordHash(Usuario usuario, string password)
    {
        using var hmac = new HMACSHA256();
        usuario.SenhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        usuario.SenhaSalt = hmac.Key;
    }
}
