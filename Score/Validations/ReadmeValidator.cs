using FluentValidation;
using NuGet.Packaging;
using Score.Models;

namespace Score.Validations
{
    public class ReadmeValidator : AbstractValidator<PackageContext>
    {
        public ReadmeValidator()
        {
            RuleFor(x => x.NuspecReader.GetReleaseNotes()).NotEmpty().WithMessage("Provide some recent Release Notes.");
        }
    }
}