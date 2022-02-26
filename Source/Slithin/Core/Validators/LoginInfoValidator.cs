using FluentValidation;
using Slithin.Models;

namespace Slithin.Core.Validators;

public class LoginInfoValidator : AbstractValidator<LoginInfo>
{
    public LoginInfoValidator()
    {
        RuleFor(x => x.IP).NotEmpty()
            .Matches(IPAddress.Pattern);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(4);
    }
}
