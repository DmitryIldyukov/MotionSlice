using System.Reflection;
using Auth.Domain.Entities;
using FluentAssertions;

namespace Auth.UnitTests.Domain;

public class RefreshTokenTests
{
    [Fact]
    public void Constructor_SetsUserIdAndTokenHash()
    {
        Guid userId = Guid.NewGuid();

        RefreshToken token = new RefreshToken( userId, "hash" );

        token.UserId.Should().Be( userId );
        token.TokenHash.Should().Be( "hash" );
    }

    [Fact]
    public void NewToken_IsActive()
    {
        RefreshToken token = new RefreshToken( Guid.NewGuid(), "hash" );

        token.IsActive.Should().BeTrue();
        token.IsRevoked.Should().BeFalse();
        token.IsExpired.Should().BeFalse();
        token.RevokedAt.Should().BeNull();
        token.ReplacedByTokenHash.Should().BeNull();
    }

    [Fact]
    public void NewToken_ExpiresInSevenDays()
    {
        RefreshToken token = new RefreshToken( Guid.NewGuid(), "hash" );

        token.ExpiresAt.Should().BeCloseTo( DateTime.UtcNow.AddDays( 7 ), TimeSpan.FromSeconds( 5 ) );
    }

    [Fact]
    public void Revoke_SetsRevokedAtAndReplacedByHash()
    {
        RefreshToken token = new RefreshToken( Guid.NewGuid(), "hash" );

        token.Revoke( "new-hash" );

        token.RevokedAt.Should().NotBeNull();
        token.ReplacedByTokenHash.Should().Be( "new-hash" );
    }

    [Fact]
    public void Revoke_MakesTokenInactive()
    {
        RefreshToken token = new RefreshToken( Guid.NewGuid(), "hash" );

        token.Revoke();

        token.IsRevoked.Should().BeTrue();
        token.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Revoke_WithoutReplacement_LeavesReplacedByNull()
    {
        RefreshToken token = new RefreshToken( Guid.NewGuid(), "hash" );

        token.Revoke();

        token.ReplacedByTokenHash.Should().BeNull();
    }

    [Fact]
    public void Revoke_CalledTwice_IsIdempotent()
    {
        RefreshToken token = new RefreshToken( Guid.NewGuid(), "hash" );

        token.Revoke( "first-replacement" );
        DateTime? firstRevokedAt = token.RevokedAt;

        token.Revoke( "second-replacement" );

        // Повторный отзыв ничего не меняет: ни время, ни ссылку на заменивший токен.
        token.RevokedAt.Should().Be( firstRevokedAt );
        token.ReplacedByTokenHash.Should().Be( "first-replacement" );
    }

    [Fact]
    public void ExpiredToken_IsNotActive()
    {
        RefreshToken token = CreateExpiredToken();

        token.IsExpired.Should().BeTrue();
        token.IsActive.Should().BeFalse();
    }

    // У RefreshToken нет абстракции времени (используется DateTime.UtcNow напрямую),
    // поэтому смоделировать протухший токен можно только через reflection.
    // См. заметку про TimeProvider в обсуждении — это уберёт необходимость в хаке.
    private static RefreshToken CreateExpiredToken()
    {
        RefreshToken token = new RefreshToken( Guid.NewGuid(), "hash" );

        PropertyInfo expiresAt = typeof( RefreshToken )
            .GetProperty( nameof( RefreshToken.ExpiresAt ) )!;
        expiresAt.SetValue( token, DateTime.UtcNow.AddDays( -1 ) );

        return token;
    }
}
