using ModeloBackEndAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace ModeloBackEndAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa as operações de persistência de <c>Extrato</c>.
    /// </summary>
    public class ExtratoRepository : IExtratroRepository
    {
        private readonly Contexto _db;

        /// <summary>
        /// Inicializa uma nova instância de <c>Extrato Repository</c>.
        /// </summary>
        public ExtratoRepository(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        #region EXTRATO

        /// <summary>
        /// Obtém as transações da conta dentro do período informado.
        /// </summary>
        public async Task<IEnumerable<Transacao>> GetExtratoAsync(int id, int periodo)
        {

            var dataLimite = DateTime.Now.AddDays(-periodo);
            var extrato = await _db.Transacao
                .Where(w =>
                    (w.ContaId == id && w.DtInclusao >= dataLimite) ||
                    (w.ContaId_Destino == id && w.DtInclusao >= dataLimite))
                .OrderByDescending(d => d.DtInclusao)
                .ToListAsync()
                .ConfigureAwait(false);

            return extrato ?? Enumerable.Empty<Transacao>();
        }

        #endregion

    }
}