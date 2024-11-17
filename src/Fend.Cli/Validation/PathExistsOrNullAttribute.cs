using System.ComponentModel.DataAnnotations;

namespace Fend.Cli.Validation;

internal sealed class PathExistsOrNullAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case null:
            case string path when Directory.Exists(path) || Directory.Exists(path):
                return ValidationResult.Success;
            default:
                return new ValidationResult($"The path '{value}' is not found.");
        }
    }
}