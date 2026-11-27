namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Define o contrato público de <c>IBaseRepository</c>.
    /// </summary>
    public interface IBaseRepository
    {
        /// <summary>
        /// Verifica se a conta está ativa e não excluída.
        /// </summary>
        Task<bool> GetSituacaoAsync(int contaId);

        /// <summary>
        /// Obtém o saldo atual da conta de forma assíncrona.
        /// </summary>
        Task<Decimal> GetSaldoAsync(int contaId);

        /// <summary>
        /// Obtém a data de inclusão da conta.
        /// </summary>
        Task<DateTime> GetDtInclusaoAsync(int contaId);
    }
}
