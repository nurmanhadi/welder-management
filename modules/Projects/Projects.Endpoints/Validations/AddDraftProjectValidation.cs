using FluentValidation;
using Projects.Endpoints.Dtos;

namespace Projects.Endpoints.Validations;

public class AddDraftProjectValidation : AbstractValidator<AddDraftProjectDto>
{
    public AddDraftProjectValidation()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().Length(1, 100);
        RuleFor(x => x.Description).NotEmpty().Length(1, 500);
        RuleFor(x => x.Cost).NotEmpty();
    }
}
