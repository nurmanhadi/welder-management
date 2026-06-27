using FluentValidation;
using Projects.Endpoints.Dtos;

namespace Projects.Endpoints.Validations;

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
