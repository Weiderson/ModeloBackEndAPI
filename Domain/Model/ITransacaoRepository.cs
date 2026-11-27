using ModeloBackEndAPI.Domain.Model;

namespace ModeloBackEndAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Define o contrato público de <c>ITransacaoRepository</c>.
    /// </summary>
    public interface ITransacaoRepository
    {
        /// <summary>
        /// Adiciona o registro informado de forma assíncrona.
        /// </summary>
        Task AddAsync(Transacao transacao);

        /// <summary>
        /// Obtém os registros de forma assíncrona.
        /// </summary>
        Task<IEnumerable<Transacao>> GetAsync();

        /// <summary>
        /// Obtém as transações associadas à conta.
        /// </summary>
        Task<IEnumerable<Transacao>> GetTransacaoAsync(int contaid);

        /// <summary>
        /// Atualiza o saldo da conta de origem.
        /// </summary>
        Task UpdateSaldoAsync(int contaId, decimal saldo_atualizado);

        /// <summary>
        /// Credita o valor informado na conta de destino.
        /// </summary>
        Task UpdateContaDestinoAsync(int ContaId_Destino, decimal valor);
    }
}