namespace DesafioBackendAPI.Domain.Repositories
{
    public interface IBaseRepository
    {
        Task<bool> GetSituacaoAsync(int contaId, CancellationToken cancellationToken = default);

        Task<Decimal> GetSaldoAsync(int contaId, CancellationToken cancellationToken = default);

        Task<DateTime> GetDtInclusaoAsync(int contaId, CancellationToken cancellationToken = default);
    }
}
