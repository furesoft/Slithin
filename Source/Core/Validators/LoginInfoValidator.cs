using FluentValidation;
using Slithin.Models;

namespace Slithin.Core.Validators
{
    public class LoginInfoValidator : AbstractValidator<LoginInfo>
    {
        public LoginInfoValidator()
        {
            RuleFor(x => x.IP).NotEmpty()
                .Matches(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
