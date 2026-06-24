using System.Reflection;
using Customers.Core;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
                .AddMySql()
                .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        services.AddScoped<ICustomerConnectionFactory, CustomerConnection>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
}
