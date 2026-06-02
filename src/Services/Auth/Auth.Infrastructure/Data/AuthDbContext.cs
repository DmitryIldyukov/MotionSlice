using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthDbContext : DbContext
{
    #region DbSets

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    #endregion

    public AuthDbContext( DbContextOptions<AuthDbContext> options )
        : base( options )
    { }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfigurationsFromAssembly( GetType().Assembly );
    }
}