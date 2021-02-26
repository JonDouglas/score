using System;
using FluentValidation;
using Score.Models;

namespace Score.Validations.ProvidesDocumentation
{
    public class ApiDocumentationValidator: AbstractValidator<NuGetFrameworkDocumentation>
    {
        public ApiDocumentationValidator()
        {
            RuleFor(x => x.PublicApiDocumentationPercent).GreaterThanOrEqualTo(0.2)
                .OverridePropertyName("Insufficient Public API XML Documentation (< 20%)")
                .WithMessage(x => $"Add more <summary> comments to your public APIs (Currently {Math.Round(x.PublicApiDocumentationPercent * 100, 2)}%)");
        }
    }
}