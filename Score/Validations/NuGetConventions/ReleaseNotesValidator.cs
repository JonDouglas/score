using FluentValidation;
using Score.Models;

namespace Score.Validations
{
    public class ReleaseNotesValidator: AbstractValidator<PackageContext>
    {
        public ReleaseNotesValidator()
        {
            RuleFor(x => x.NuspecReader.GetReleaseNotes()).NotEmpty()
                .WithName("<releaseNotes> is empty.")
                .WithMessage("Add a <releaseNotes> element to your .nuspec");
        }
    }
}