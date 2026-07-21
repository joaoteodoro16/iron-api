using FluentValidation;
using Iron.Aplication.DTOs.Auth.Request;

namespace Iron.Aplication.Validators;

public class AuthUserRequestValidator : AbstractValidator<AuthUserRequest>
{
    public AuthUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
