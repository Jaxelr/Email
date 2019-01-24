using Api.Model.Email.Operations;
using ServiceStack.FluentValidation;

namespace Api.Logic.Validators
{
    public class EmailValidator : AbstractValidator<SendEmail>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Email.From).NotEmpty();
            RuleFor(x => x.Email.To).NotEmpty();
        }
    }
}