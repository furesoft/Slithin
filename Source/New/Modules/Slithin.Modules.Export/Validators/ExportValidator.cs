using FluentValidation;
using Slithin.Modules.Export.ViewModels;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Export.Validators;

public class ExportValidator : AbstractValidator<ExportModalViewModel>
{
    public ExportValidator(ILocalisationService localisationService)
    {
        RuleFor(x => x.PagesSelector).NotEmpty()
            .WithLocalisedMessage("Select at least one page");

        RuleFor(x => x.ExportPath)
            .NotNull()
            .WithLocalisedMessage("No export path specified");
    }
}
