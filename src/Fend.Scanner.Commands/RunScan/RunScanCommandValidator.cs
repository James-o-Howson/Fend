using FluentValidation;

namespace Fend.Commands.RunScan;

internal sealed class RunScanCommandValidator : AbstractValidator<RunScanCommand>
{
    public RunScanCommandValidator()
    {
        RuleFor(c => c.Target)
            .Must(DirectoryExists)
            .When(c => c.Target is not null)
            .WithMessage("Target must be a directory");
        
        RuleFor(c => c.OutputPath)
            .Must(DirectoryExists)
            .When(c => c.Target is not null)
            .WithMessage("Output Path must be a directory");
    }
    
    private static bool DirectoryExists(string? value) => !string.IsNullOrWhiteSpace(value) && Directory.Exists(value);
}