using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync( string email, CancellationToken cancellationToken = default );
    Task<bool> ExistsByEmailAsync( string email, CancellationToken ct = default );
    Task<User?> GetByIdAsync( Guid id, CancellationToken cancellationToken = default );
    Task<User?> GetByExternalLoginAsync( string provider, string providerKey, CancellationToken cancellationToken = default );
    Task AddAsync( User user, CancellationToken cancellationToken = default );
}