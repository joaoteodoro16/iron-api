using System.Reflection;
using FluentValidation;
using Iron.Aplication.Usecases.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Iron.Aplication.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<CreateUserUsecase>();
        services.AddScoped<AuthUserUsecase>();
        services.AddScoped<RefreshTokenUsecase>();

        return services;
    }
}
