using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Domain.Repositories
{
    public interface ITransacaoRepository
    {
        Task AddAsync(Transacao transacao, CancellationToken cancellationToken = default);

        Task<IEnumerable<Transacao>> GetAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Transacao>> GetTransacaoAsync(int contaid, CancellationToken cancellationToken = default);

        Task UpdateSaldoAsync(int contaId, decimal saldo_atualizado, CancellationToken cancellationToken = default);

        Task UpdateContaDestinoAsync(int ContaId_Destino, decimal valor, CancellationToken cancellationToken = default);
    }
}