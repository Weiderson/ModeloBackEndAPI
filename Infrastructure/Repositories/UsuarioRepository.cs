using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Contexto _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioRepository(Contexto db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Usuario> GetAsync(string email, CancellationToken cancellationToken = default)
        {
            var usuario = await UsuarioExisteAsync(email, cancellationToken).ConfigureAwait(false);
            return usuario;
        }

        public async Task<Usuario?> GetTrackedAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _db.Usuario.AsTracking()
                .FirstOrDefaultAsync(usuario => usuario.Email == email, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Usuario>> GetAsync(CancellationToken cancellationToken = default)
        {
            var usuarios = await _db.Usuario
                .AsNoTracking()
                .Where(u => u.DtExclusao == null)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            return usuarios;
        }

        public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            await _db.Usuario.AddAsync(usuario, cancellationToken).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            _db.Update(usuario);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Usuario usuario, CancellationToken cancellationToken = default)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string? caminho = httpContext?.Request.Path.ToString();

            if (_db.Entry(usuario).State == EntityState.Detached)
                _db.Attach(usuario);

            if (!string.IsNullOrEmpty(caminho) && caminho.StartsWith("/api/v1/"))
            {
                usuario.DtExclusao = DateTime.Now;
            }
            else if (!string.IsNullOrEmpty(caminho) && caminho.StartsWith("/api/v2/"))
            {
                usuario.DtExclusao = DateTime.Now;
                _db.Usuario.Remove(usuario);
            }

            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<Usuario> UsuarioExisteAsync(string email, CancellationToken cancellationToken = default)
        {
            var usuario = await _db.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken)
                .ConfigureAwait(false);
            return usuario;
        }

        public async Task<Usuario> UsuarioExisteExcluidoAsync(string email, CancellationToken cancellationToken = default)
        {
            var usuario = await _db.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.DtExclusao == null, cancellationToken)
                .ConfigureAwait(false);
            return usuario;
        }
    }

}