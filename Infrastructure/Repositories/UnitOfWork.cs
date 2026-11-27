using DesafioBackendAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Contexto _db;

        public UnitOfWork(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _db.Database.BeginTransactionAsync(cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _db.SaveChangesAsync(cancellationToken);
        }

    }
}
