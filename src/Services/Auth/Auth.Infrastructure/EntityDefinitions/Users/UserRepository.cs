using Auth.Application.Interfaces.Repositories;
using Auth.Infrastructure.Data;
using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.EntityDefinitions.Users;

public class UserRepository( AuthDbContext dbContext ) : IUserRepository
{
    public async Task AddAsync( User user, CancellationToken cancellationToken = default )
    {
        await dbContext.Users.AddAsync( user, cancellationToken );
    }

    public async Task<User?> GetByEmailAsync( string email, CancellationToken cancellationToken = default )
    {
        return await dbContext.Users
            .Include( u => u.ExternalLogins )
            .FirstOrDefaultAsync( u => u.Email == email, cancellationToken );
    }

    public async Task<bool> ExistsByEmailAsync( string email, CancellationToken ct = default )
    {
        return await dbContext.Users.AnyAsync( u => u.Email == email, ct );
    }

    public async Task<User?> GetByExternalLoginAsync( string provider, string providerKey, CancellationToken cancellationToken = default )
    {
        return await dbContext.Users
            .AsNoTracking()
            .Include( u => u.ExternalLogins )
            .FirstOrDefaultAsync( u => u.ExternalLogins.Any( el => el.Provider == provider && el.ProviderKey == providerKey ), cancellationToken );
    }

    public async Task<User?> GetByIdAsync( Guid id, CancellationToken cancellationToken = default )
    {
        return await dbContext.Users
            .Include( u => u.ExternalLogins )
            .FirstOrDefaultAsync( u => u.Id == id, cancellationToken );
    }
}