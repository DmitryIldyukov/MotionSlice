namespace Auth.Domain.Entities
{
    public class RefreshToken : Entity
    {
        private const int ExpiryDays = 7;

        public Guid UserId { get; init; }
        public string TokenHash { get; init; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByTokenHash { get; private set; }

        public bool IsRevoked => RevokedAt is not null;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;

        private RefreshToken()
        { }

        public RefreshToken( Guid userId, string tokenHash )
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = DateTime.UtcNow.AddDays( ExpiryDays );
        }

        public void Revoke( string? replacedByTokenHash = null )
        {
            if ( RevokedAt != null )
            {
                return;
            }

            RevokedAt = DateTime.UtcNow;
            ReplacedByTokenHash = replacedByTokenHash;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}