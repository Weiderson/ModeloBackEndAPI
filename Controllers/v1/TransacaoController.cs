using Asp.Versioning;
using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Enums;
using DesafioBackendAPI.Domain.Interfaces;
using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackendAPI.Controllers.v1;

[ApiController]
[AllowAnonymous]
[Route("api/v{version:apiVersion}/Transacao")]
[ApiVersion("1.0")]
public class TransacaoController : ControllerBase
{
    private readonly ITransacaoRepository _repository;
    private readonly ILogger<TransacaoController> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository _baseRepository;

    public TransacaoController(
        ITransacaoRepository repository,
        ILogger<TransacaoController> logger,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IBaseRepository baseRepository)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _baseRepository = baseRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        [FromQuery] TransacaoDTO transacaoDto,
        CancellationToken cancellationToken)
    {
        if (transacaoDto == null)
            return BadRequest("Dados não podem ser nulos.");

        if (!Enum.IsDefined(typeof(TransacaoTipo), transacaoDto.Tipo))
            return BadRequest("Tipo de transação inválido.");

        if (transacaoDto.Valor <= 0)
            return BadRequest("Valor deve ser maior que zero.");

        if (transacaoDto.Tipo == TransacaoTipo.Transferencia)
        {
            if (transacaoDto.ContaId_Destino == null)
                return BadRequest("Conta de destino inválida.");

            if (transacaoDto.ContaId == transacaoDto.ContaId_Destino)
                return BadRequest("Conta de origem e destino são iguais.");
        }

        await using var transaction = await _unitOfWork
            .BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Creating transaction for account {AccountId}.", transacaoDto.ContaId);

        var sourceAccountActive = await _baseRepository
            .GetSituacaoAsync(transacaoDto.ContaId, cancellationToken)
            .ConfigureAwait(false);
        if (!sourceAccountActive)
            return BadRequest("Conta inativa e/ou excluída!");

        if (transacaoDto.Tipo is TransacaoTipo.Pagamento or TransacaoTipo.Saque
            or TransacaoTipo.Transferencia)
        {
            var sourceBalance = await _baseRepository
                .GetSaldoAsync(transacaoDto.ContaId, cancellationToken)
                .ConfigureAwait(false);
            if (sourceBalance < transacaoDto.Valor)
                return BadRequest("Saldo insuficiente para realizar a transação.");

            await _repository.UpdateSaldoAsync(
                    transacaoDto.ContaId,
                    sourceBalance - transacaoDto.Valor,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        else if (transacaoDto.Tipo == TransacaoTipo.Deposito)
        {
            var sourceBalance = await _baseRepository
                .GetSaldoAsync(transacaoDto.ContaId, cancellationToken)
                .ConfigureAwait(false);

            await _repository.UpdateSaldoAsync(
                    transacaoDto.ContaId,
                    sourceBalance + transacaoDto.Valor,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        if (transacaoDto.Tipo == TransacaoTipo.Transferencia)
        {
            var destinationId = transacaoDto.ContaId_Destino!.Value;
            var destinationAccountActive = await _baseRepository
                .GetSituacaoAsync(destinationId, cancellationToken)
                .ConfigureAwait(false);
            if (!destinationAccountActive)
                return BadRequest("Conta de destino inativa e/ou excluída!");

            await _repository.UpdateContaDestinoAsync(
                    destinationId,
                    transacaoDto.Valor,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        var transacao = _mapper.Map<Transacao>(transacaoDto);
        transacao.DtInclusao = DateTime.Now;
        transacao.Id = null;
        transacao.Descricao = transacao.Descricao.ToUpperInvariant();

        await _repository.AddAsync(transacao, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

        _logger.LogInformation(
            "Transaction {TransactionId} created for account {AccountId}.",
            transacao.Id,
            transacaoDto.ContaId);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var transacoes = await _repository.GetAsync(cancellationToken).ConfigureAwait(false);
        if (!transacoes.Any())
            return NotFound("Lista de transacões nula ou vazia.");

        var transacoesDto = _mapper.Map<IEnumerable<TransacaoDTO>>(transacoes);
        return Ok(transacoesDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var accountActive = await _baseRepository
            .GetSituacaoAsync(id, cancellationToken)
            .ConfigureAwait(false);
        if (!accountActive)
            return BadRequest("Conta inativa e/ou excluída!");

        var transacoes = await _repository.GetTransacaoAsync(id, cancellationToken).ConfigureAwait(false);
        if (!transacoes.Any())
            return NotFound("Lista de transacões nula ou vazia.");

        var transacoesDto = _mapper.Map<IEnumerable<TransacaoDTO>>(transacoes);
        return Ok(transacoesDto);
    }
}
