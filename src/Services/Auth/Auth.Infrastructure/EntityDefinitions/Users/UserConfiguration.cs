using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.EntityDefinitions.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure( EntityTypeBuilder<User> builder )
    {
        builder.ToTable( "users" )
            .HasKey( u => u.Id );

        builder.Property( u => u.Id )
            .HasColumnName( "user_id" )
            .IsRequired();

        builder.Property( u => u.Email )
            .HasColumnName( "email" )
            .IsRequired()
            .HasMaxLength( 255 );

        builder.HasMany( u => u.ExternalLogins )
            .WithOne()
            .HasForeignKey( l => l.UserId )
            .OnDelete( DeleteBehavior.Cascade );

        builder.Navigation( u => u.ExternalLogins )
            .UsePropertyAccessMode( PropertyAccessMode.Field );

        builder.HasIndex( u => u.Email ).IsUnique();
    }
}