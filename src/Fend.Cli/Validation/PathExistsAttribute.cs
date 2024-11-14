using System.ComponentModel.DataAnnotations;

namespace Fend.Cli.Validation;

internal sealed class PathExistsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string path && (Directory.Exists(path) || Directory.Exists(path)))
        {
            return ValidationResult.Success;
        }
        
        return new ValidationResult($"The path '{value}' is not found.");
    }
}