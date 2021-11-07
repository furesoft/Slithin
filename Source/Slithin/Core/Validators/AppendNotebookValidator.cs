using FluentValidation;
using Slithin.ViewModels.Modals;

namespace Slithin.Core.Validators
{
    public class AppendNotebookValidator : AbstractValidator<AppendNotebookModalViewModel>
    {
        public AppendNotebookValidator()
        {
            RuleFor(x => x.Pages).Must(_ => _.Count > 0).WithMessage("You have to add at least 1 new page");
        }
    }
}
