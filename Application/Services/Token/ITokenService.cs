using ModeloBackEndAPI.Domain.Model;

namespace ModeloBackEndAPI.Application.Services.Token
{
    /// <summary>
    /// Define as operações para criação de tokens JWT.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Executa a operação <c>Criar Token</c>.
        /// </summary>
        string CriarToken(Usuario usuario);
    }
}
