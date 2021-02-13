using FluentValidation;
using NuGet.Packaging;

namespace Score.Validations
{
    public class ReadmeValidator : AbstractValidator<NuspecReader>
    {
        public ReadmeValidator()
        {
            RuleFor(x => x.GetReleaseNotes()).NotEmpty().WithMessage("Provide some recent Release Notes.");
        }
    }
}