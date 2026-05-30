namespace Auth.API.Endpoints;

public static class AuthEndpoints
{
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
    }
}
