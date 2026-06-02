using Auth.Domain.Entities;
using FluentAssertions;

namespace Auth.UnitTests.Domain;

public class UserTests
{
    [Theory]
    [InlineData( "Test@Example.COM", "test@example.com" )]
    [InlineData( "USER@MAIL.RU", "user@mail.ru" )]
    [InlineData( "already@lower.com", "already@lower.com" )]
    public void Constructor_NormalizesEmailToLowercase( string input, string expected )
    {
        User user = new User( input );

        user.Email.Should().Be( expected );
    }

    [Fact]
    public void AddExternalLogin_ReturnsLoginWithCorrectData()
    {
        User user = new User( "user@example.com" );

        ExternalLogin login = user.AddExternalLogin( "Google", "google-key-123" );

        login.UserId.Should().Be( user.Id );
        login.Provider.Should().Be( "Google" );
        login.ProviderKey.Should().Be( "google-key-123" );
    }

    [Fact]
    public void AddExternalLogin_AddsLoginToCollection()
    {
        User user = new User( "user@example.com" );

        ExternalLogin login = user.AddExternalLogin( "Google", "google-key-123" );

        user.ExternalLogins.Should().ContainSingle().Which.Should().Be( login );
    }

    [Fact]
    public void AddExternalLogin_DuplicateProviderAndKey_Throws()
    {
        User user = new User( "user@example.com" );
        user.AddExternalLogin( "Google", "google-key-123" );

        Action act = () => user.AddExternalLogin( "Google", "google-key-123" );

        act.Should().Throw<InvalidOperationException>();
        user.ExternalLogins.Should().HaveCount( 1 );
    }

    [Fact]
    public void AddExternalLogin_SameProviderDifferentKey_IsAllowed()
    {
        User user = new User( "user@example.com" );

        user.AddExternalLogin( "Google", "key-1" );
        user.AddExternalLogin( "Google", "key-2" );

        user.ExternalLogins.Should().HaveCount( 2 );
    }

    [Fact]
    public void AddExternalLogin_RefreshesUpdatedAt()
    {
        User user = new User( "user@example.com" );

        user.AddExternalLogin( "Google", "google-key-123" );

        user.UpdatedAt.Should().BeCloseTo( DateTime.UtcNow, TimeSpan.FromSeconds( 2 ) );
    }
}
