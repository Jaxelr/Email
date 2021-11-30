using Email.Models.Operations;
using FluentValidation;

namespace Email.Validation;

public class PostEmailValidator : AbstractValidator<PostEmailRequest>
{
    public PostEmailValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
    }
}
