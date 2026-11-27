using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Domain.Repositories
{
    public interface IExtratoRepository
    {
        Task<IEnumerable<Transacao>> GetExtratoAsync(int id, int periodo, CancellationToken cancellationToken = default);
    }
}