using Customers.Endpoints.Dtos;
using FluentValidation;

namespace Customers.Endpoints.Validations;

public class UpdateCustomerValidation : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidation()
    {
        RuleFor(x => x.Name).Length(1, 100);
        RuleFor(x => x.Phone).Length(1, 20);
        RuleFor(x => x.Address).Length(1, 500);
    }
}
