using Auth.API.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace Auth.API.Endpoints;

public static class AuthEndpoints
{
    private const string GoogleCallbackEndpointName = "GoogleCallback";

    public static void MapAuthEndpoints( this IEndpointRouteBuilder app )
    {
        RouteGroupBuilder group = app.MapGroup( "/auth" );

        group.MapPost( "/register", () =>
        {
            throw new NotImplementedException();
        } );

        group.MapPost( "/login", () =>
        {
            throw new NotImplementedException();
        } );

        group.MapPost( "/refresh-token", () =>
        {
            throw new NotImplementedException();
        } );

        group.MapPost( "/logout", () =>
        {
            throw new NotImplementedException();
        } );

        group.MapGet( "login/google", ( LinkGenerator links, HttpContext context ) =>
        {
            string? redirectUrl = links.GetUriByName( context, GoogleCallbackEndpointName );
            AuthenticationProperties properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Results.Challenge( properties, [ GoogleDefaults.AuthenticationScheme ] );
        } );

        group.MapGet( "login/google/callback", async ( HttpContext context ) =>
        {
            AuthenticateResult result = await context.AuthenticateAsync( AuthorizeExtension.GoogleExternalScheme );

            return Results.Ok();
        } )
        .WithName( GoogleCallbackEndpointName );
    }
}
