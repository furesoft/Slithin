using FluentValidation;
using Slithin.Core.Services;
using Slithin.ViewModels.Modals;

namespace Slithin.Core.Validators;

public class AddTemplateValidator : AbstractValidator<AddTemplateModalViewModel>
{
    private readonly ILocalisationService _localisationService;

    public AddTemplateValidator(ILocalisationService localisationService)
    {
        _localisationService = localisationService;

        RuleFor(x => x.Name).NotEmpty()
            .WithMessage(_localisationService.GetString("Name should not be empty"));

        RuleFor(x => x.Filename).Custom(FilenameValidator);
        RuleFor(x => x.IconCode).NotNull().WithMessage(_localisationService.GetString("An IconCode must be selected"));
        RuleFor(x => x.SelectedCategory).NotNull().WithMessage(_localisationService.GetString("A Category must be selected"));
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
