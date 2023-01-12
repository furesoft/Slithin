using FluentValidation;
using Slithin.Modules.Export;
using Slithin.Modules.I18N.Models;

namespace Slithin.Validators;

public class ExportValidator : AbstractValidator<ExportModalViewModel>
{
    private readonly ILocalisationService _localisationService;

    public ExportValidator(ILocalisationService localisationService)
    {
        _localisationService = localisationService;

        RuleFor(x => x.PagesSelector).NotEmpty()
            .WithLocalizedMessage("Select at least one page");

        RuleFor(x => x.ExportPath)
            .NotNull()
            .WithLocalizedMessage("No export path specified");
    }
}
