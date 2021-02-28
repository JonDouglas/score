using FluentValidation;
using NuGet.Frameworks;
using Score.Models;

namespace Score.Validations.SupportsMultiplePlatforms
{
    public class MultiplePlatformsValidator: AbstractValidator<NuGetFramework>
    {
        public MultiplePlatformsValidator()
        {
            RuleFor(x => x.GetShortFolderName()).Must(f => f.Contains("net5.0") || f.Contains("netstandard2.0"))
                .OverridePropertyName("Not supporting enough platforms")
                .WithMessage(x => $"Multi-target your library to support net5.0/ & netstandard2.0/");
        }
        
    }
}