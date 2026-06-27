using Customers.Endpoints.Dtos;
using FluentValidation;

namespace Customers.Endpoints.Validations;

public class AddCustomerValidation : AbstractValidator<AddCustomerDto>
{
    public AddCustomerValidation()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 100);
        RuleFor(x => x.Phone).NotEmpty().Length(1, 20);
        RuleFor(x => x.Address).NotEmpty().Length(1, 500);
    }
}
