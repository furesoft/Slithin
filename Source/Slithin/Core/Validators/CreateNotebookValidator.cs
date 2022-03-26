using FluentValidation;
using Slithin.Core.Services;
using Slithin.ViewModels.Modals.Tools;

namespace Slithin.Core.Validators;

public class CreateNotebookValidator : AbstractValidator<CreateNotebookModalViewModel>
{
    public CreateNotebookValidator(ILocalisationService localisationService)
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(localisationService.GetString("Title cannot be empty"));
        RuleFor(x => x.Pages).Must(_ => _.Count > 0)
            .WithMessage(localisationService.GetString("A notebook has to have at least 1 page"));
    }
}
