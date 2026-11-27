using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly Contexto _db;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ContaRepository(Contexto db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        #region CRUD

        public async Task AddAsync(Conta conta, CancellationToken cancellationToken = default)
        {
            await _db.Conta.AddAsync(conta, cancellationToken).ConfigureAwait(false);
            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Conta>> GetAsync(CancellationToken cancellationToken = default)
        {
            var contas = await _db.Conta.ToListAsync(cancellationToken).ConfigureAwait(false);
            return contas;
        }

        public async Task<Conta> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(f => f.Id == id, cancellationToken).ConfigureAwait(false);
            return conta;
        }

        public async Task<Conta?> GetTrackedAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Conta.AsTracking()
                .FirstOrDefaultAsync(conta => conta.Id == id, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Conta?> GetByAgencyAndNumberAsync(
            string agency,
            string number,
            CancellationToken cancellationToken = default)
        {
            return await _db.Conta.AsNoTracking()
                .FirstOrDefaultAsync(conta =>
                    conta.Agencia.Trim() == agency.Trim() && conta.Numero.Trim() == number.Trim(),
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var caminho = httpContext?.Request.Path.ToString();

            if (caminho == null)
                throw new InvalidOperationException("Request path is unavailable.");

            var conta = await GetTrackedAsync(id, cancellationToken).ConfigureAwait(false);
            if (conta is null)
                throw new KeyNotFoundException("Conta inexistente.");

            if (caminho.StartsWith("/api/v1"))
            {
                conta.DtExclusao = DateTime.Now;
                _db.Conta.Update(conta);
            }
            else if (caminho.StartsWith("/api/v2"))
            {
                conta.DtExclusao = DateTime.Now;
                _db.Conta.Remove(conta);
            }

            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Conta conta, CancellationToken cancellationToken = default)
        {
            _db.Conta.Update(conta);

            await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        #endregion CRUD
    }
}