using System.Text;
using Auth.API.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Extensions;

public static class AuthorizeExtension
{
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
}
