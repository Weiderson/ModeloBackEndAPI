using DesafioBackendAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly Contexto _db;

        public BaseRepository(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<bool> GetSituacaoAsync(int contaID, CancellationToken cancellationToken = default)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(d => d.Id == contaID, cancellationToken).ConfigureAwait(false);

            if (conta == null)
                throw new KeyNotFoundException("Conta inexistente.");

            var situacao = conta.DtExclusao == null && conta.Situacao == "Ativa";

            return situacao;
        }

        public async Task<decimal> GetSaldoAsync(int id, CancellationToken cancellationToken = default)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(s => s.Id == id, cancellationToken).ConfigureAwait(false);
            if (conta == null)
                throw new KeyNotFoundException("Conta inexistente.");

            return conta.Saldo;
        }

        public async Task<DateTime> GetDtInclusaoAsync(int id, CancellationToken cancellationToken = default)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(s => s.Id == id, cancellationToken).ConfigureAwait(false);

            if (conta == null)
                throw new KeyNotFoundException("Conta inexistente.");

            var dataInclusao = conta.DtInclusao;

            return conta.DtInclusao;
        }
    }
}
