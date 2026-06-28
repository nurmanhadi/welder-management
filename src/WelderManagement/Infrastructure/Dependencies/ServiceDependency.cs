using WelderManagement.Domain.Contracts.Clients;
using WelderManagement.Domain.Contracts.Repositories;
using WelderManagement.Domain.Contracts.Services;
using WelderManagement.Infrastructure.Repositories;
using WelderManagement.Infrastructure.Rpc;
using WelderManagement.Infrastructure.Services;

namespace WelderManagement.Infrastructure.Dependencies;

public static class ServiceDependency
{
    public static IServiceCollection AddServiceDependency(this IServiceCollection services)
    {

        // repository
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        // services
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProjectService, ProjectService>();

        // clients
        services.AddScoped<ICustomerClient, CustomerRpc>();

        return services;
    }
}
