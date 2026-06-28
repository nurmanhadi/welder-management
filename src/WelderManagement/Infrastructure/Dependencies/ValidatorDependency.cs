using FluentValidation;
using WelderManagement.Applications.Dtos;

namespace WelderManagement.Infrastructure.Dependencies;

public static class ValidatorDependency
{
    public static IServiceCollection AddValidatorDependency(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateCustomerDto>();

        return services;
    }
}
