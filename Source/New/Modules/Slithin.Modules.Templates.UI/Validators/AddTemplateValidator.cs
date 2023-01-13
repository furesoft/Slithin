using FluentValidation;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Templates.UI.ViewModels;

namespace Slithin.Validators;

public class AddTemplateValidator : AbstractValidator<AddTemplateModalViewModel>
{
    public AddTemplateValidator(ILocalisationService localisationService)
    {
        RuleFor(x => x.Name).NotEmpty()
            .WithLocalisedMessage("Name should not be empty");

        RuleFor(x => x.Filename).NotNull().WithLocalisedMessage("Filename is not set");
        RuleFor(x => x.IconCode).NotNull().WithLocalisedMessage("An IconCode must be selected");
        RuleFor(x => x.SelectedCategory).NotNull().WithLocalisedMessage("A Category must be selected");
    }
}
