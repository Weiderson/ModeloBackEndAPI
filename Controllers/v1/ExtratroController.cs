using Asp.Versioning;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Enums;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackendAPI.Controllers.v1
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/Extrato")]
    [ApiVersion("1.0")]
    public class ExtratroController : ControllerBase
    {
        private readonly IExtratoRepository _repository;
        private readonly ILogger<ExtratroController> _logger;
        private readonly IBaseRepository _baseRepository;

        public ExtratroController(ILogger<ExtratroController> logger, IExtratoRepository repository, IBaseRepository baseRepository)
        {
            _repository = repository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _baseRepository = baseRepository;
        }

        #region SALDO E EXTRATO

        [HttpGet("Saldo/{ContaId}")]
        public async Task<IActionResult> GetSaldo(int ContaId, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Buscando saldo...");

                if (ContaId <= 0)
                    return BadRequest("Id inválido.");

                var situacao = await _baseRepository.GetSituacaoAsync(ContaId, cancellationToken).ConfigureAwait(false);
                if (!situacao)
                    return BadRequest("Conta inativa e/ou excluída!");

                var saldo = await _baseRepository.GetSaldoAsync(ContaId, cancellationToken).ConfigureAwait(false);

                _logger.LogInformation("Saldo retornado.");

                return Ok(saldo); //Retorna solicitação com DTOs
            }

        }


        [HttpGet("Extrato")]
        public async Task<IActionResult> GetExtrato([FromQuery] ExtratoDTO ExtratoDTO, CancellationToken cancellationToken)
        {
            {
                _logger.LogInformation("Buscando extrato...");

                if (ExtratoDTO.ContaId <= 0)
                    return BadRequest("Id inválido.");

                var situacao = await _baseRepository.GetSituacaoAsync(ExtratoDTO.ContaId, cancellationToken).ConfigureAwait(false);
                if (!situacao)
                    return BadRequest("Conta inativa e/ou excluída!");

                if (!System.Enum.TryParse(typeof(ExtratoPeriodo), ExtratoDTO.ExtratoPeriodo.ToString(), true, out var parsedPeriodo))
                    return BadRequest("Período informado: " + parsedPeriodo + " inválido. Os valores válidos são: " + string.Join(", ", (ExtratoPeriodo[])System.Enum.GetValues(typeof(ExtratoPeriodo))) + ".");

                int periodo = ExtratoDTO.ExtratoPeriodo switch
                {
                    ExtratoPeriodo.Dia => 1,
                    ExtratoPeriodo.Semana => 7,
                    ExtratoPeriodo.Mes => 30,
                    ExtratoPeriodo.Ano => 365
                };

                var _extrato = await _repository.GetExtratoAsync(ExtratoDTO.ContaId, periodo, cancellationToken).ConfigureAwait(false);
                if (!_extrato.Any())
                    return NotFound("Não existe transações para o período informado.");

                List<string> _listaExtrado = new List<string>();

                var linha = "------------------------------------------------------------------------------------------";

                var saldo = await _baseRepository.GetSaldoAsync(ExtratoDTO.ContaId, cancellationToken).ConfigureAwait(false);
                string saldocompleto = DateTime.Now + " - SALDO: " + saldo.ToString();

                _listaExtrado.Add(linha);
                _listaExtrado.Add(saldocompleto);
                _listaExtrado.Add(linha);

                foreach (var item in _extrato)
                {
                    string itemextrato = string.Empty;

                    if (item.Tipo == TransacaoTipo.Transferencia.ToString())
                    {
                        if (item.ContaId == ExtratoDTO.ContaId)
                        {
                            itemextrato = item.DtInclusao.ToString("dd/MM/yyyy") + " " + item.Tipo.ToString() + " " + "-" + item.Valor.ToString("C");
                        }
                        else if (item.ContaId_Destino == ExtratoDTO.ContaId)
                        {
                            itemextrato = item.DtInclusao.ToString("dd/MM/yyyy") + " " + item.Tipo.ToString() + " " + "+" + item.Valor.ToString("C");
                        }
                    }
                    else if (item.Tipo == TransacaoTipo.Pagamento.ToString() || item.Tipo == TransacaoTipo.Saque.ToString())
                    {
                        itemextrato = item.DtInclusao.ToString("dd/MM/yyyy") + " " + item.Tipo.ToString() + " " + "-" + item.Valor.ToString("C");
                    }
                    else if (item.Tipo == TransacaoTipo.Deposito.ToString())
                    {
                        itemextrato = item.DtInclusao.ToString("dd/MM/yyyy") + " " + item.Tipo.ToString() + " " + "+" + item.Valor.ToString("C");
                    }

                    _listaExtrado.Add(itemextrato);
                }

                _logger.LogInformation("Extrato retornado.");

                return Ok(_listaExtrado);
            }

        }

        #endregion Saldo e Extrato

    }
}