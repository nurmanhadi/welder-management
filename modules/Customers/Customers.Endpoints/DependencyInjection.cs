using Customers.Core;
using FluentValidation;

namespace Customers.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomersEndpoints(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddCustomerValidation>();
        return services;
    }
}
