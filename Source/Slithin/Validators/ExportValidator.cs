using System.IO;
using FluentValidation;

namespace Slithin.Validators;

public class ExportValidator : AbstractValidator<ExportModalViewModel>
{
    private readonly ILocalisationService _localisationService;

    public ExportValidator(ILocalisationService localisationService)
    {
        _localisationService = localisationService;

        RuleFor(x => x.PagesSelector).NotEmpty()
            .WithMessage(_localisationService.GetString("Select at least one page"));

        RuleFor(x => x.ExportPath)
            .NotNull().WithMessage(_localisationService.GetString("No export path specified"))
            .Custom(ExportPathValidator);
    }

    private void ExportPathValidator(string input, ValidationContext<ExportModalViewModel> context)
    {
        if (!Directory.Exists(input))
        {
            context.AddFailure(_localisationService.GetString("Export Path"),
                _localisationService.GetString("Directory does not exist"));
        }
    }
}
