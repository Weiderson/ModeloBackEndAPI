using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Domain.Repositories
{
    public interface IContaRepository
    {
        Task AddAsync(Conta conta, CancellationToken cancellationToken = default);

        Task<IEnumerable<Conta>> GetAsync(CancellationToken cancellationToken = default);

        Task<Conta> GetAsync(int id, CancellationToken cancellationToken = default);

        Task<Conta?> GetTrackedAsync(int id, CancellationToken cancellationToken = default);

        Task<Conta?> GetByAgencyAndNumberAsync(string agency, string number, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task UpdateAsync(Conta conta, CancellationToken cancellationToken = default);

    }
}