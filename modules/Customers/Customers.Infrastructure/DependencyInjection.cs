using Customers.Core.Contracts;
using Customers.Infrastructure.Data;
using Customers.Infrastructure.Data.Migrations;
using Customers.Infrastructure.Repositories;
using Customers.Infrastructure.Services;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICustomerConnectionFactory, CustomerConnection>();

        services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddMySql()
            .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
            .ScanIn(typeof(CreateTableCustomer).Assembly).For.All())
        .BuildServiceProvider(false);

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();
        return services;
    }
    public static IServiceCollection AddCustomerMigrations(this IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
        return services;
    }
}
