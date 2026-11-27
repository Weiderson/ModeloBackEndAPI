using Asp.Versioning;
using AutoMapper;
using ModeloBackEndAPI.Application.DTOs;
using ModeloBackEndAPI.Application.Services.Token;
using ModeloBackEndAPI.Domain.Model;
using ModeloBackEndAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ModeloBackEndAPI.Controllers.v2
{
    /// <summary>
    /// Controlador responsável pelas operações de <c>Usuario</c>.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Usuario")]
    [ApiVersion("2.0")]
    public class UsuarioController : ControllerBase
    {
        private readonly Contexto _db;
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UsuarioController> _logger;

        /// <summary>
        /// Inicializa uma nova instância de <c>UsuarioController</c>.
        /// </summary>
        public UsuarioController(Contexto db, IUsuarioRepository repository, IMapper mapper, IConfiguration configuration, ILogger<UsuarioController> logger)
        {
            _db = db;
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _tokenService = new TokenService(configuration);
            _logger = logger;
        }

        /// <summary>
        /// Obtém os registros ou o recurso solicitado.
        /// </summary>
        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                _logger.LogInformation("Buscando registro...");

                var usuario = await _repository.GetAsync(email).ConfigureAwait(false);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);

                _logger.LogInformation("Registro retornado...");
                return Ok(usuarioDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Exclui o registro indicado.
        /// </summary>
        [Authorize]
        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                _logger.LogInformation("Excluindo registro");
                var usuario = await _repository.GetAsync(email).ConfigureAwait(false);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                await _repository.DeleteAsync(usuario).ConfigureAwait(false);
                _logger.LogInformation("Registro excluído.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Valida as credenciais e gera um token JWT.
        /// </summary>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromQuery] UsuarioDTO usuarioDTO)
        {
            try
            {
                _logger.LogInformation("Efetuando login");
                if (usuarioDTO == null)
                    return BadRequest("Dados inválidos.");

                if (usuarioDTO.Email == null)
                    return BadRequest("Email inválido.");

                if (usuarioDTO.Senha == null)
                    return BadRequest("Senha inválida.");


                var usuario = await _repository.UsuarioExisteExcluidoAsync(usuarioDTO.Email).ConfigureAwait(false);
                if (usuario == null)
                    return BadRequest("Usuário inexistente ou excluído.");

                if (usuario.SenhaSalt is null || usuario.SenhaHash is null)
                    return BadRequest("Credenciais inválidas.");

                var hash = new HMACSHA256(usuario.SenhaSalt);
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(usuarioDTO.Senha));

                if (!usuario.SenhaHash.SequenceEqual(result))
                    return BadRequest("Senha inválida.");

                var token = _tokenService.CriarToken(usuario);

                hash.Dispose();
                _logger.LogInformation("Login efetuado");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}
