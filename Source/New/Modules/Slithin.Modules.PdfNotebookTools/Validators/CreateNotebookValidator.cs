using FluentValidation;
using Slithin.Core.Services;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.PdfNotebookTools.ViewModels;
using Slithin.ViewModels.Modals.Tools;

namespace Slithin.Validators;

public class CreateNotebookValidator : AbstractValidator<CreateNotebookModalViewModel>
{
    public CreateNotebookValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithLocalisedMessage("Title cannot be empty");
        RuleFor(x => x.Pages).Must(_ => _.Count > 0)
            .WithLocalisedMessage("A notebook has to have at least 1 page");
    }
}
