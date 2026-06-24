using Customers.Core;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomersInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ICustomerConnectionFactory, CustomerConnection>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
}
