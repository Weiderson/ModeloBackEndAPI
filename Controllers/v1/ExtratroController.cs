using Asp.Versioning;
using AutoMapper;
using ModeloBackEndAPI.Application.DTOs;
using ModeloBackEndAPI.Application.Enums;
using ModeloBackEndAPI.Domain.Model;
using ModeloBackEndAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ModeloBackEndAPI.Controllers.v1
{
    /// <summary>
    /// Controlador responsável pelas operações de <c>Extratro</c>.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/Extrato")]
    [ApiVersion("1.0")]
    public class ExtratroController : ControllerBase
    {
        private readonly Contexto _db;
        private readonly IExtratroRepository _repository;
        private readonly ILogger<ContaController> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository _baseRepository;

        /// <summary>
        /// Inicializa uma nova instância de <c>ExtratroController</c>.
        /// </summary>
        public ExtratroController(Contexto db, ILogger<ContaController> logger, IExtratroRepository repository, IMapper mapper, IBaseRepository baseRepository)
        {
            _db = db;
            _repository = repository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper;
            _baseRepository = baseRepository;
        }

        #region SALDO E EXTRATO

        /// <summary>
        /// Obtém o saldo atual da conta.
        /// </summary>
        [HttpGet("Saldo/{ContaId}")]
        public async Task<IActionResult> GetSaldo(int ContaId)
        {
            try
            {
                _logger.LogInformation("Buscando saldo...");

                if (ContaId <= 0)
                    return BadRequest("Id inválido.");

                var situacao = await Task.Run(() => _baseRepository.GetSituacaoAsync(ContaId)).ConfigureAwait(false);
                if (!situacao)
                    return BadRequest("Conta inativa e/ou excluída!");

                var saldo = await Task.Run(() => _baseRepository.GetSaldoAsync(ContaId)).ConfigureAwait(false);

                _logger.LogInformation("Saldo retornado.");

                return Ok(saldo); //Retorna solicitação com DTOs
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }


        /// <summary>
        /// Obtém o extrato da conta para o período solicitado.
        /// </summary>
        [HttpGet("Extrato")]
        public async Task<IActionResult> GetExtrato([FromQuery] ExtratoDTO ExtratoDTO)
        {
            try
            {
                _logger.LogInformation("Buscando extrato...");

                if (ExtratoDTO.ContaId <= 0)
                    return BadRequest("Id inválido.");

                var situacao = await Task.Run(() => _baseRepository.GetSituacaoAsync(ExtratoDTO.ContaId)).ConfigureAwait(false);
                if (!situacao)
                    return BadRequest("Conta inativa e/ou excluída!");

                if (!System.Enum.IsDefined(ExtratoDTO.ExtratoPeriodo))
                    return BadRequest("Período informado: " + ExtratoDTO.ExtratoPeriodo + " inválido. Os valores válidos são: " + string.Join(", ", (ExtratoPeriodo[])System.Enum.GetValues(typeof(ExtratoPeriodo))) + ".");

                int periodo = ExtratoDTO.ExtratoPeriodo switch
                {
                    ExtratoPeriodo.Dia => 1,
                    ExtratoPeriodo.Semana => 7,
                    ExtratoPeriodo.Mes => 30,
                    ExtratoPeriodo.Ano => 365,
                    _ => throw new ArgumentOutOfRangeException(nameof(ExtratoDTO.ExtratoPeriodo))
                };

                var _extrato = await Task.Run(() => _repository.GetExtratoAsync(ExtratoDTO.ContaId, periodo)).ConfigureAwait(false);
                if (!_extrato.Any())
                    return NotFound("Não existe transações para o período informado.");

                List<string> _listaExtrado = new List<string>();

                var linha = "------------------------------------------------------------------------------------------";

                var saldo = await Task.Run(() => _baseRepository.GetSaldoAsync(ExtratoDTO.ContaId)).ConfigureAwait(false);
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
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        #endregion Saldo e Extrato

    }
}