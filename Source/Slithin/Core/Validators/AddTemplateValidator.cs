using System;
using FluentValidation;
using Slithin.ViewModels.Modals;

namespace Slithin.Core.Validators;

public class AddTemplateValidator : AbstractValidator<AddTemplateModalViewModel>
{
    public AddTemplateValidator()
    {
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x.Filename).Custom(FilenameValidator);
        RuleFor(x => x.IconCode).NotNull();
        RuleFor(x => x.SelectedCategory).NotNull();
    }

    private void FilenameValidator(string arg1, ValidationContext<AddTemplateModalViewModel> arg2)
    {
        if (!arg2.InstanceToValidate.UseTemplateEditor)
        {
            if (string.IsNullOrEmpty(arg1))
            {
                arg2.AddFailure("Filename", "Filename should be a valid path and not empty");
            }
        }
    }
}
