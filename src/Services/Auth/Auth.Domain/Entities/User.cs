namespace Auth.Domain.Entities;

public class User : Entity
{
    private readonly List<ExternalLogin> _externalLogins = new();

    public string Email { get; private set; } = null!;
    public IReadOnlyCollection<ExternalLogin> ExternalLogins => _externalLogins.AsReadOnly();

    private User()
    { }

    public User( string email )
    {
        Email = email;
    }

    public ExternalLogin AddExternalLogin( string provider, string providerKey )
    {
        bool alreadyLinked = _externalLogins
            .Any( l => l.Provider == provider && l.ProviderKey == providerKey );

        if ( alreadyLinked )
        {
            throw new InvalidOperationException( $"External login '{provider}' is already linked to this user." );
        }

        ExternalLogin login = new ExternalLogin( Id, provider, providerKey );
        _externalLogins.Add( login );
        UpdatedAt = DateTime.UtcNow;

        return login;
    }
}
