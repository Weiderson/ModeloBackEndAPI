namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Define o contrato público de <c>IExtratroRepository</c>.
    /// </summary>
    public interface IExtratroRepository
    {
        /// <summary>
        /// Obtém as transações da conta dentro do período informado.
        /// </summary>
        Task<IEnumerable<Transacao>> GetExtratoAsync(int id, int periodo);
    }
}