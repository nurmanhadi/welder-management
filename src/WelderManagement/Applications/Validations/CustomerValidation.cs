using FluentValidation;
using WelderManagement.Applications.Dtos;

namespace WelderManagement.Applications.Validations;

public class CreateCustomerValidation : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidation()
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
