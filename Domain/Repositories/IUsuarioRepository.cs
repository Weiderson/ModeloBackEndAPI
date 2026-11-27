using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAsync(CancellationToken cancellationToken = default);

        Task<Usuario> GetAsync(string email, CancellationToken cancellationToken = default);

        Task<Usuario?> GetTrackedAsync(string email, CancellationToken cancellationToken = default);

        Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task DeleteAsync(Usuario usuario, CancellationToken cancellationToken = default);

        Task<Usuario> UsuarioExisteAsync(string email, CancellationToken cancellationToken = default);

        Task<Usuario> UsuarioExisteExcluidoAsync(string email, CancellationToken cancellationToken = default);

    }
}
