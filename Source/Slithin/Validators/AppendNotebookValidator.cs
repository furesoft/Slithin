using FluentValidation;

namespace Slithin.Validators;

public class AppendNotebookValidator : AbstractValidator<AppendNotebookModalViewModel>
{
    public AppendNotebookValidator(ILocalisationService localisationService)
    {
        RuleFor(x => x.Pages).Must(_ => _.Count > 0)
            .WithMessage(localisationService.GetString("You have to add at least 1 new page"));
    }
}
