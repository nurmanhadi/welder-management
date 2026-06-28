using WelderManagement.Infrastructure.Data;

namespace WelderManagement.Infrastructure.Dependencies;

public static class ConnectionDependency
{
    public static IServiceCollection AddConnectionDependency(this IServiceCollection services)
    {
        services.AddScoped<IConnectionFactory, ConnectionFactory>();
        return services;
    }
}
