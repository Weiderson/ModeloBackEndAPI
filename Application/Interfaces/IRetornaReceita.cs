namespace DesafioBackendAPI.Application.Interfaces;

public interface IRetornaReceita
{
    Task<string?> RetornaNome(string cnpj, CancellationToken cancellationToken = default);
}
