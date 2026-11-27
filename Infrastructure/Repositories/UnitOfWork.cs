using ModeloBackEndAPI.Domain.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ModeloBackEndAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Representa <c>Unit Of Work</c>.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Contexto _db;

        /// <summary>
        /// Inicializa uma nova instância de <c>Unit Of Work</c>.
        /// </summary>
        public UnitOfWork(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Inicia uma transação do banco de dados.
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            var transaction = _db.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        /// <summary>
        /// Persiste as alterações pendentes no contexto.
        /// </summary>
        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }

    }
}
