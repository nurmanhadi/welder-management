using FluentValidation;

namespace Customers.Endpoints;

public class AddCustomerValidation : AbstractValidator<AddCustomerDto>
{
    public AddCustomerValidation()
    {
        RuleFor(x => x.Name).NotEmpty().Length(1, 100);
        RuleFor(x => x.Phone).NotEmpty().Length(1, 20);
        RuleFor(x => x.Address).NotEmpty().Length(1, 500);
    }
}

public class UpdateCustomerValidation : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidation()
    {
        RuleFor(x => x.Name).Length(1, 100);
        RuleFor(x => x.Phone).Length(1, 20);
        RuleFor(x => x.Address).Length(1, 500);
    }
}
