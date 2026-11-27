using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class ExtratoRepository : IExtratoRepository
    {
        private readonly Contexto _db;

        public ExtratoRepository(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        #region EXTRATO

        public async Task<IEnumerable<Transacao>> GetExtratoAsync(
            int id,
            int periodo,
            CancellationToken cancellationToken = default)
        {

            var dataLimite = DateTime.Now.AddDays(-periodo);
            var extrato = await _db.Transacao
                .Where(w => (w.ContaId == id || w.ContaId_Destino == id) && w.DtInclusao >= dataLimite)
                .OrderByDescending(d => d.DtInclusao)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return extrato ?? Enumerable.Empty<Transacao>();
        }

        #endregion

    }
}