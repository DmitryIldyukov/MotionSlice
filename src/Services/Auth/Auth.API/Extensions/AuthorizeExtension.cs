using System.Text;
using Auth.API.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Extensions;

public static class AuthorizeExtension
{
    public const string GoogleExternalScheme = "External";

    public static AuthenticationBuilder AddJwtAuthentication(
        this AuthenticationBuilder builder,
        IConfiguration configuration )
    {
        JwtOptions jwtOptions = configuration.GetSection( nameof( JwtOptions ) ).Get<JwtOptions>() 
                                ?? throw new InvalidOperationException( $"Configuration section '{nameof( JwtOptions )}' is missing." );

        builder.AddJwtBearer( options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( jwtOptions.SecretKey ) ),
                ClockSkew = TimeSpan.Zero,
            };
        } );

        builder.Services.AddAuthorization();

        return builder;
    }

    public static AuthenticationBuilder AddGoogleAuthentication(
        this AuthenticationBuilder builder,
        IConfiguration configuration )
    {
        builder.AddCookie( GoogleExternalScheme );

        builder.AddGoogle( options =>
        {
            options.SignInScheme = GoogleExternalScheme;

            string? clientId = configuration[ "Authentication:Google:ClientId" ];
            if ( string.IsNullOrEmpty( clientId ) )
            {
                throw new InvalidOperationException( "Configuration 'Authentication:Google:ClientId' is missing." );
            }

            string? clientSecret = configuration[ "Authentication:Google:ClientSecret" ];
            if ( string.IsNullOrEmpty( clientSecret ) )
            {
                throw new InvalidOperationException( "Configuration 'Authentication:Google:ClientSecret' is missing." );
            }

            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
        } );

        return builder;
    }
}