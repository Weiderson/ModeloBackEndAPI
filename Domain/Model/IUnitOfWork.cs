using System.Data;

namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Define as operações de transação e persistência.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Persiste as alterações pendentes no contexto.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Inicia uma transação do banco de dados.
        /// </summary>
        IDbTransaction BeginTransaction();
    }
}