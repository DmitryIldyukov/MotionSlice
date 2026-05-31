using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EntityDefinitions.RefreshTokens;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure( EntityTypeBuilder<RefreshToken> builder )
    {
        builder.ToTable( "refresh_tokens" )
            .HasKey( rt => rt.Id );

        builder.Property( rt => rt.Id )
            .HasColumnName( "refresh_token_id" )
            .IsRequired();

        builder.Property( rt => rt.UserId )
            .HasColumnName( "user_id" )
            .IsRequired();

        builder.Property( rt => rt.TokenHash )
            .HasColumnName( "token_hash" )
            .IsRequired();

        builder.Property( rt => rt.ExpiresAt )
            .HasColumnName( "expires_at" )
            .IsRequired();

        builder.Property( rt => rt.RevokedAt )
            .HasColumnName( "revoked_at" );

        builder.Property( rt => rt.ReplacedByTokenHash )
            .HasColumnName( "replaced_by_token_hash" );

        builder.HasIndex( rt => rt.TokenHash ).IsUnique();
    }
}