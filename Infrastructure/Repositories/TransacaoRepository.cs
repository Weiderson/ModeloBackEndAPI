using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly Contexto _db;

        public TransacaoRepository(Contexto db)
        {
            _db = db;
        }


        #region TRANSAÇÃO

        public async Task AddAsync(Transacao transacao, CancellationToken cancellationToken = default)
        {
            await _db.Transacao.AddAsync(transacao, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Transacao>> GetAsync(CancellationToken cancellationToken = default)
        {
            var transacaoes = await _db.Transacao.ToListAsync(cancellationToken).ConfigureAwait(false);
            return transacaoes;
        }

        public async Task<IEnumerable<Transacao>> GetTransacaoAsync(int ContaId, CancellationToken cancellationToken = default)
        {
            var transacaoes = await _db.Transacao
                .Where(t => t.ContaId == ContaId || t.ContaId_Destino == ContaId)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            return transacaoes;
        }

        public async Task UpdateSaldoAsync(int contaID, Decimal saldo_atualizado, CancellationToken cancellationToken = default)
        {
            var _conta = await _db.Conta.AsTracking()
                .FirstOrDefaultAsync(d => d.Id == contaID, cancellationToken)
                .ConfigureAwait(false);

            if (_conta == null)
                throw new KeyNotFoundException("Conta inexistente.");

            _conta.Saldo = saldo_atualizado;

        }

        public async Task UpdateContaDestinoAsync(int ContaId_Destino, decimal valor, CancellationToken cancellationToken = default)
        {
            var _conta = await _db.Conta.AsTracking()
                .FirstOrDefaultAsync(c => c.Id == ContaId_Destino, cancellationToken)
                .ConfigureAwait(false);

            if (_conta == null)
                throw new KeyNotFoundException("Conta de destino inexistente.");

            _conta.Saldo += valor;

        }


        #endregion

    }
}