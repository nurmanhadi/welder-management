using FluentValidation;
using WelderManagement.Applications.Dtos;

namespace WelderManagement.Applications.Validations;

public class CreateDraftProjectValidation : AbstractValidator<CreateDraftProjectDto>
{
    public CreateDraftProjectValidation()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().Length(1, 100);
        RuleFor(x => x.Description).NotEmpty().Length(1, 500);
        RuleFor(x => x.Cost).NotEmpty();
    }
}
public class UpdateProjectValidation : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectValidation()
    {
        RuleFor(x => x.Title).Length(1, 100);
        RuleFor(x => x.Description).Length(1, 500);
        RuleFor(x => x.Cost).GreaterThan(0);
        RuleFor(x => x.Status).IsInEnum();
    }
}
