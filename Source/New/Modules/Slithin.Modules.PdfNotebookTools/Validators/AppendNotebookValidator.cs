using FluentValidation;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.PdfNotebookTools.ViewModels;

namespace Slithin.Validators;

public class AppendNotebookValidator : AbstractValidator<AppendNotebookModalViewModel>
{
    public AppendNotebookValidator()
    {
        RuleFor(x => x.Pages).Must(_ => _.Count > 0)
            .WithLocalisedMessage("You have to add at least 1 new page");
    }
}
