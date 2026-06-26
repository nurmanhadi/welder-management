using Customers.Endpoints.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Customers.Endpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomersEndpoints(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddCustomerValidation>();
        return services;
    }
}
