using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.API;

public class Program
{
    public static void Main( string[] args )
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

        string? connectionString = builder.Configuration.GetConnectionString( "PostgresConnection" );
        builder.Services.AddDbContext<AuthDbContext>( options =>
        {
            options.UseNpgsql( connectionString );
        } );

        WebApplication app = builder.Build();

        app.UseHttpsRedirection();

        app.Run();
    }
}
