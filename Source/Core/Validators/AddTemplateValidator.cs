using FluentValidation;
using Slithin.ViewModels.Modals;

namespace Slithin.Core.Validators
{
    public class AddTemplateValidator : AbstractValidator<AddTemplateModalViewModel>
    {
        public AddTemplateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Filename).NotEmpty();
            RuleFor(x => x.IconCode).NotNull();
            RuleFor(x => x.SelectedCategory).NotNull();
        }
    }
}
