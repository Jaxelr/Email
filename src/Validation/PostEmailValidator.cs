using EmailService.Entities.Operations;
using FluentValidation;

namespace EmailService.Validation
{
    public class PostEmailValidator : AbstractValidator<PostEmailRequest>
    {
        public PostEmailValidator()
        {
            RuleFor(x => x.From).NotEmpty();
            RuleFor(x => x.To).NotEmpty();
        }
    }
}
