using Auth.API.Endpoints;
using Auth.API.Extensions;
using Auth.Infrastructure;
using Auth.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            options.UseNpgsql( connectionString ).UseSnakeCaseNamingConvention();
        } );

        builder.Services.AddInfrastructure();

        builder.Services.AddAuthentication( options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            } )
            .AddJwtAuthentication( builder.Configuration )
            .AddGoogleAuthentication( builder.Configuration );

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();

        WebApplication app = builder.Build();

        using ( IServiceScope scope = app.Services.CreateScope() )
        {
            AuthDbContext dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
            dbContext.Database.Migrate();
        }

        if ( app.Environment.IsDevelopment() )
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapAuthEndpoints();

        app.Run();
    }
}