using Auth.Application.Interfaces.Repositories;
using Auth.Infrastructure.Data;
using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.EntityDefinitions.RefreshTokens;

public class RefreshTokenRepository( AuthDbContext dbContext ) : IRefreshTokenRepository
{
    public async Task AddAsync( RefreshToken refreshToken, CancellationToken cancellationToken = default )
    {
        await dbContext.RefreshTokens.AddAsync( refreshToken, cancellationToken );
    }

    public async Task<RefreshToken?> GetByTokenHashAsync( string tokenHash, CancellationToken cancellationToken = default )
    {
        return await dbContext.RefreshTokens.FirstOrDefaultAsync( rt => rt.TokenHash == tokenHash, cancellationToken );
    }
}