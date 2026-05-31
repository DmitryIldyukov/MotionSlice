using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EntityDefinitions.ExternalLogins;

public class ExternalLoginConfiguration : IEntityTypeConfiguration<ExternalLogin>
{
    public void Configure( EntityTypeBuilder<ExternalLogin> builder )
    {
        builder.ToTable( "external_logins" )
            .HasKey( el => el.Id );

        builder.Property( el => el.Id )
            .HasColumnName( "external_login_id" )
            .IsRequired();

        builder.Property( el => el.UserId )
            .HasColumnName( "user_id" )
            .IsRequired();

        builder.Property( el => el.Provider )
            .HasColumnName( "provider" )
            .IsRequired();

        builder.Property( el => el.ProviderKey )
            .HasColumnName( "provider_key" )
            .IsRequired();

        builder.HasIndex( el => new { el.Provider, el.ProviderKey } ).IsUnique();
    }
}