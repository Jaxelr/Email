﻿using Em = Email.Models;
using FluentValidation;

namespace Email.Validation;

public class PostEmailValidator : AbstractValidator<Em.Email>
{
    public PostEmailValidator()
    {
        RuleFor(x => x.From).EmailAddress().NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();
        RuleFor(x => x.Subject).NotEmpty();
    }
}
