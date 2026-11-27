using Asp.Versioning;
using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Interfaces;
using DesafioBackendAPI.Application.Services;
using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace DesafioBackendAPI.Controllers.v1
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/Conta")]
    [ApiVersion("1.0")]
    public class ContaController : ControllerBase
    {
        private readonly IContaRepository _repository;
        private readonly ILogger<ContaController> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository _baseRepository;
        private readonly IRetornaReceita _retornaReceita;

        public ContaController(
            ILogger<ContaController> logger,
            IContaRepository repository,
            IMapper mapper,
            IBaseRepository baseRepository,
            IRetornaReceita retornaReceita)
        {
            _repository = repository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper;
            _baseRepository = baseRepository;
            _retornaReceita = retornaReceita;
        }

        #region CRUD

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Add(ContaDTO contadto, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Adicionando registro...");

                if (contadto == null)
                    return BadRequest("Dados não podem ser nulos.");

                if (contadto.Arquivo == null || contadto.Arquivo.Length == 0)
                    return BadRequest("Arquivo da conta não informado.");

                if (!FormataString.SomenteNumeros(contadto.Cnpj))
                    return BadRequest("CNPJ deve conter apenas números.");

                if (contadto.Cnpj.Length != 14)
                    return BadRequest("CNPJ deve conter 14 números.");

                var _conta = await _repository
                    .GetByAgencyAndNumberAsync(contadto.Agencia, contadto.Numero, cancellationToken)
                    .ConfigureAwait(false);
                if (_conta != null)
                    return BadRequest("Conta existente.");

                if (await _retornaReceita.RetornaNome(contadto.Cnpj, cancellationToken).ConfigureAwait(false) == null)
                    return BadRequest("CNPJ inválido.");

                var conta_agencia_numero = "_" + contadto.Numero + contadto.Agencia;
                var complemento_nome = RandomNumberGenerator.GetInt32(100000000, int.MaxValue);
                var local_arquivo = Path.Combine("Armazenamento", conta_agencia_numero + complemento_nome + System.IO.Path.GetExtension(contadto.Arquivo.FileName));

                var conta = _mapper.Map<Conta>(contadto);

                //Campos não mapeados
                conta.LocalArquivo = local_arquivo;
                conta.DtInclusao = DateTime.Now;
                conta.Id = null;

                await RetornaArquivo.SalvarArquivoDisco(local_arquivo, contadto, cancellationToken).ConfigureAwait(false);
                try
                {
                    await _repository.AddAsync(conta, cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                    RetornaArquivo.ExcluirArquivoDisco(local_arquivo);
                    throw;
                }

                _logger.LogInformation("Registro id: " + conta.Id + " adicionado!");

                return Ok();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Buscando registros...");

                var contas = await _repository.GetAsync(cancellationToken).ConfigureAwait(false);

                if (!contas.Any())
                    return NotFound("Lista de contas é nula ou vazia.");

                foreach (var conta in contas)
                {
                    var nome = await _retornaReceita.RetornaNome(conta.Cnpj, cancellationToken).ConfigureAwait(false);

                    if (nome == "TooManyRequests")
                        conta.NomeCompleto = "Número de requisições acima do limite permitido em ReceitaWS (HTTP 429).";
                    else if (nome != null)
                        conta.NomeCompleto = nome;
                    else
                        conta.NomeCompleto = "Ocorreu um erro na solicitação.";
                }

                var contasDTO = _mapper.Map<IEnumerable<ContaDTO>>(contas);

                _logger.LogInformation("Registros retornados.");

                return Ok(contasDTO); //Retorna solicitação com DTOs
            }

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Buscando registro...");

                var _conta = await _repository.GetAsync(Id, cancellationToken).ConfigureAwait(false);

                if (_conta == null)
                    return NotFound("Conta inexistente.");

                var nome = await _retornaReceita.RetornaNome(_conta.Cnpj, cancellationToken).ConfigureAwait(false);

                if (nome == "TooManyRequests")
                    _conta.NomeCompleto = "Número de requisições acima do limite permitido em ReceitaWS (HTTP 429).";
                else if (nome != null)
                    _conta.NomeCompleto = nome;
                else
                    _conta.NomeCompleto = "Ocorreu um erro na solicitação.";

                var _contaDTO = _mapper.Map<ContaDTO>(_conta);

                _logger.LogInformation("Registro id: " + Id + "retornado.");

                return Ok(_contaDTO); //Retorna solicitação com DTOs
            }

        }

        [HttpGet("Download/{nome}")]
        public async Task<IActionResult> Download(string nome, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Buscando arquivo por nome: " + nome);

                var arquivo_nome = nome;
                var local_arquivo = Path.Combine(Directory.GetCurrentDirectory(), "Armazenamento");
                var local_arquivo_completo = Path.Combine(local_arquivo, arquivo_nome);

                if (!System.IO.File.Exists(local_arquivo_completo))
                {
                    return BadRequest("Arquivo informado inexistente");
                }

                var fileBytes = await RetornaArquivo.RecuperarArquivoDisco(local_arquivo_completo, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Download do arquivo foi efetuado");

                return File(fileBytes, "application/octet-stream", arquivo_nome);
            }

        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, ContaDTO contadto, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Adicionando registro...");

                if (contadto == null)
                    return BadRequest("conta não pode ser nulo.");

                if (!FormataString.SomenteNumeros(contadto.Cnpj))
                    return BadRequest("CNPJ deve conter apenas números.");

                if (contadto.Cnpj.Length != 14)
                    return BadRequest("CNPJ deve conter 14 números.");

                var contaExistente = await _repository.GetTrackedAsync(Id, cancellationToken).ConfigureAwait(false);
                if (contaExistente == null)
                    return NotFound("Conta inexistente.");

                var situacao = await _baseRepository.GetSituacaoAsync(Id, cancellationToken).ConfigureAwait(false);
                if (!situacao)
                    return BadRequest("Conta inativa e/ou excluída!");

                if (await _retornaReceita.RetornaNome(contadto.Cnpj, cancellationToken).ConfigureAwait(false) == null)
                    return BadRequest("CNPJ inválido.");

                var arquivoNovo = contadto.Arquivo != null && contadto.Arquivo.Length > 0;
                var localArquivoAnterior = contaExistente.LocalArquivo;
                string? local_arquivo = null;
                if (arquivoNovo)
                {
                    var conta_agencia_numero = "_" + contadto.Numero + contadto.Agencia;
                    var complemento_nome = RandomNumberGenerator.GetInt32(100000000, int.MaxValue);
                    local_arquivo = Path.Combine("Armazenamento", conta_agencia_numero + complemento_nome + Path.GetExtension(contadto.Arquivo.FileName));
                }

                var _conta = _mapper.Map(contadto, contaExistente);

                //Campos não mapeados
                _conta.Id = Id;
                _conta.DtInclusao = contaExistente.DtInclusao;
                _conta.DtExclusao = contaExistente.DtExclusao;
                _conta.DtAlteracao = DateTime.Now;
                _conta.Saldo = await _baseRepository.GetSaldoAsync(Id, cancellationToken).ConfigureAwait(false);
                _conta.LocalArquivo = localArquivoAnterior;

                if (arquivoNovo)
                {
                    await RetornaArquivo.SalvarArquivoDisco(local_arquivo!, contadto, cancellationToken).ConfigureAwait(false);
                    _conta.LocalArquivo = local_arquivo;
                }

                try
                {
                    await _repository.UpdateAsync(_conta, cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                    if (arquivoNovo)
                        RetornaArquivo.ExcluirArquivoDisco(local_arquivo);
                    throw;
                }

                if (arquivoNovo && !string.Equals(localArquivoAnterior, local_arquivo, StringComparison.Ordinal))
                    RetornaArquivo.ExcluirArquivoDisco(localArquivoAnterior);

                _logger.LogInformation("Registro id: " + _conta.Id + " alterado!");

                return Ok();
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Deletando registro id: " + id);

                var conta = await _repository.GetTrackedAsync(id, cancellationToken).ConfigureAwait(false);

                if (conta == null)
                    return NotFound("Registro não encontrado.");

                if (conta.Id.HasValue)
                {
                    var saldo = await _baseRepository.GetSaldoAsync(conta.Id.Value, cancellationToken).ConfigureAwait(false);

                    if (saldo != 0)
                        return BadRequest("Não é possível excluir uma conta com saldo diferente de zero.");
                }

                if (conta.DtExclusao != null)
                    return BadRequest("Não é possível excluir uma conta que já foi excluída.");

                await _repository.DeleteAsync(id, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Registro id: " + id + " excluído!");

                return Ok();
            }

        }

        #endregion CRUD

    }
}