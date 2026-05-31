using Auth.Application.Interfaces;

namespace Auth.Infrastructure.Data;

public class UnitOfWork( AuthDbContext dbContext ) : IUnitOfWork
{
    public Task CommitAsync( CancellationToken cancellationToken = default )
    {
        return dbContext.SaveChangesAsync( cancellationToken );
    }
}