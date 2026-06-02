using Microsoft.Extensions.DependencyInjection;
using Auth.Application.Interfaces;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.EntityDefinitions.RefreshTokens;
using Auth.Application.Interfaces.Repositories;
using Auth.Infrastructure.EntityDefinitions.Users;

namespace Auth.Infrastructure;

public static class Bindings
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services )
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}