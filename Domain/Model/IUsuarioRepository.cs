namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Define o contrato público de <c>IUsuarioRepository</c>.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Obtém os registros de forma assíncrona.
        /// </summary>
        Task<IEnumerable<Usuario>> GetAsync();

        /// <summary>
        /// Obtém o usuário associado ao endereço de e-mail informado.
        /// </summary>
        Task<Usuario?> GetAsync(string email);

        /// <summary>
        /// Adiciona o registro informado de forma assíncrona.
        /// </summary>
        Task AddAsync(Usuario usuario);

        /// <summary>
        /// Atualiza o registro informado de forma assíncrona.
        /// </summary>
        Task UpdateAsync(Usuario usuario);

        /// <summary>
        /// Exclui ou marca o registro como excluído, conforme a versão da API.
        /// </summary>
        Task DeleteAsync(Usuario usuario);

        /// <summary>
        /// Verifica se existe um usuário com o e-mail informado.
        /// </summary>
        Task<Usuario?> UsuarioExisteAsync(string email);

        /// <summary>
        /// Localiza um usuário ativo pelo e-mail informado.
        /// </summary>
        Task<Usuario?> UsuarioExisteExcluidoAsync(string email);

    }
}
