using FluentValidation;
using Slithin.Core.Services;
using Slithin.ViewModels.Modals;

namespace Slithin.Core.Validators;

public class AddTemplateValidator : AbstractValidator<AddTemplateModalViewModel>
{
    private readonly ILocalisationService _localisationService;

    public AddTemplateValidator(ILocalisationService localisationService)
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Filename).Custom(FilenameValidator);
        RuleFor(x => x.IconCode).NotNull();
        RuleFor(x => x.SelectedCategory).NotNull();
        _localisationService = localisationService;
    }

    private void FilenameValidator(string arg1, ValidationContext<AddTemplateModalViewModel> arg2)
    {
        if (!arg2.InstanceToValidate.UseTemplateEditor)
        {
            if (string.IsNullOrEmpty(arg1))
            {
                arg2.AddFailure(_localisationService.GetString("Filename"),
                    _localisationService.GetString("Filename should be a valid path and not empty"));
            }
        }
    }
}
