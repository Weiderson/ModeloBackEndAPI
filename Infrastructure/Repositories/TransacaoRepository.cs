using ModeloBackEndAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ModeloBackEndAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa as operações de persistência de <c>Transacao</c>.
    /// </summary>
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly Contexto _db;

        /// <summary>
        /// Inicializa uma nova instância de <c>Transacao Repository</c>.
        /// </summary>
        public TransacaoRepository(Contexto db)
        {
            _db = db;
        }


        #region TRANSAÇÃO

        /// <summary>
        /// Adiciona o registro informado de forma assíncrona.
        /// </summary>
        public async Task AddAsync(Transacao transacao)
        {
            await _db.Transacao.AddAsync(transacao); // Use AddAsync instead of Add for asynchronous operation
        }

        /// <summary>
        /// Obtém os registros de forma assíncrona.
        /// </summary>
        public async Task<IEnumerable<Transacao>> GetAsync()
        {
            var transacaoes = await _db.Transacao.ToListAsync().ConfigureAwait(false);
            return transacaoes;
        }

        /// <summary>
        /// Obtém as transações associadas à conta.
        /// </summary>
        public async Task<IEnumerable<Transacao>> GetTransacaoAsync(int ContaId)
        {
            var transacaoes = await _db.Transacao.Where(t => t.ContaId == ContaId || t.ContaId_Destino == ContaId).ToListAsync().ConfigureAwait(false);
            return transacaoes;
        }

        /// <summary>
        /// Atualiza o saldo da conta de origem.
        /// </summary>
        public async Task UpdateSaldoAsync(int contaID, Decimal saldo_atualizado)
        {
            var _conta = await _db.Conta.FirstOrDefaultAsync(d => d.Id == contaID).ConfigureAwait(false);

            if (_conta == null)
                throw new Exception("Conta inexistente.");

            _conta.Saldo = saldo_atualizado;

            _db.Conta.Update(_conta);
        }

        /// <summary>
        /// Credita o valor informado na conta de destino.
        /// </summary>
        public async Task UpdateContaDestinoAsync(int ContaId_Destino, decimal valor)
        {
            var _conta = await _db.Conta.FirstOrDefaultAsync(c => c.Id == ContaId_Destino).ConfigureAwait(false);

            if (_conta == null)
                throw new Exception("Conta de destino inexistente.");

            _conta.Saldo += valor;

            _db.Conta.Update(_conta);
        }


        #endregion

    }
}