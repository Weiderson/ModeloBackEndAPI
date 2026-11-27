namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Define o contrato público de <c>IContaRepository</c>.
    /// </summary>
    public interface IContaRepository
    {
        /// <summary>
        /// Adiciona o registro informado de forma assíncrona.
        /// </summary>
        Task AddAsync(Conta conta);

        /// <summary>
        /// Obtém os registros de forma assíncrona.
        /// </summary>
        Task<IEnumerable<Conta>> GetAsync();

        /// <summary>
        /// Obtém os registros de forma assíncrona.
        /// </summary>
        Task<Conta?> GetAsync(int id);

        /// <summary>
        /// Exclui ou marca o registro como excluído, conforme a versão da API.
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Atualiza o registro informado de forma assíncrona.
        /// </summary>
        Task UpdateAsync(Conta conta);

    }
}