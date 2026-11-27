using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Application.Interfaces
{
    public interface ITokenService
    {
        string CriarToken(Usuario usuario);
    }
}
