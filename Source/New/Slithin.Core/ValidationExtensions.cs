using FluentValidation.Results;

namespace Slithin.Core;

public static class ValidationExtensions
{
    public static string AsString(this List<ValidationFailure> errors)
    {
        return string.Join('\n', errors);
    }
}
