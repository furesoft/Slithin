using FluentValidation;
using Slithin.Core.Services;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Templates.UI.ViewModels;
using Slithin.ViewModels.Modals;

namespace Slithin.Validators;

public class AddTemplateValidator : AbstractValidator<AddTemplateModalViewModel>
{
    
    public AddTemplateValidator(ILocalisationService localisationService)
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithLocalizedMessage("Name should not be empty");

        RuleFor(x => x.Filename).NotNull().WithLocalizedMessage("Filename is not set");
        RuleFor(x => x.IconCode).NotNull().WithLocalizedMessage("An IconCode must be selected");
        RuleFor(x => x.SelectedCategory).NotNull().WithLocalizedMessage("A Category must be selected");
    }
}
