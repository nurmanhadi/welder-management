using FluentMigrator.Runner;
using WelderManagement.Infrastructure.Data.Migrations;

namespace WelderManagement.Infrastructure.Dependencies;

public static class MigrationDependency
{
    public static IServiceCollection AddMigrationDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
            .AddMySql()
            .WithGlobalConnectionString(configuration.GetConnectionString("DefaultConnection"))
            .ScanIn(typeof(CreateTableCustomer).Assembly).For.All())
        .BuildServiceProvider(false);

        using var scope = services.BuildServiceProvider().CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();

        return services;
    }
}
