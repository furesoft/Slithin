using FluentValidation;
using Slithin.Core.Services;
using Slithin.Models;

namespace Slithin.Core.Validators;

public class LoginInfoValidator : AbstractValidator<LoginInfo>
{
    private readonly ILocalisationService _localisationService;

    public LoginInfoValidator(ILocalisationService localisationService)
    {
        _localisationService = localisationService;

        RuleFor(x => x.IP).NotEmpty()
            .Matches(IPAddress.Pattern).WithMessage(localisationService.GetString("Use a correct IP Address. Example: 127.0.0.2:22"));

        RuleFor(x => x.Password).Custom(PasswordValidator).MinimumLength(4);
    }

    private void PasswordValidator(string password, ValidationContext<LoginInfo> arg2)
    {
        if (!arg2.InstanceToValidate.UsesKey)
        {
            if (string.IsNullOrEmpty(password))
            {
                arg2.AddFailure("Password",
                    _localisationService.GetString("The device is not configured using SSH Keys. Enter a password."));
            }
        }
    }
}
