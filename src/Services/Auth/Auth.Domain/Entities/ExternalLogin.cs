namespace Auth.Domain.Entities;

public class ExternalLogin : Entity
{
    public Guid UserId { get; init; }
    public string Provider { get; init; } = null!;
    public string ProviderKey { get; init; } = null!;

    private ExternalLogin()
    { }

    internal ExternalLogin( Guid userId, string provider, string providerKey )
    {
        UserId = userId;
        Provider = provider;
        ProviderKey = providerKey;
    }
}
