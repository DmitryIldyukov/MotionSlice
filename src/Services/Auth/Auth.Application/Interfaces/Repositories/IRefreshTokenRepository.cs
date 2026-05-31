using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync( string tokenHash, CancellationToken cancellationToken = default );
    Task AddAsync( RefreshToken refreshToken, CancellationToken cancellationToken = default );
}
