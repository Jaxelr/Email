using Email.Models.Operations;
using FluentValidation;

namespace Email.Validation;

public class PostEmailValidator : AbstractValidator<PostEmailRequest>
{
    public PostEmailValidator()
    {
        RuleFor(x => x.From).EmailAddress().NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.Subject).NotEmpty();
    }
}
