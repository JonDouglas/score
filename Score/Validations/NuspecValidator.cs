using FluentValidation;
using NuGet.Packaging;

namespace Score.Validations
{
    public class NuspecValidator : AbstractValidator<NuspecReader>
    {
        public NuspecValidator()
        {
            RuleFor(x => x.GetDescription()).Length(60, 180)
                .WithMessage("Add more detail to the description field of .nuspec. Use 60 to 180 characters" +
                             " to describe the package, what it does, and its target use case.");

            RuleFor(x => x.GetIconUrl()).NotEmpty().WithMessage("Add an icon for your package");

            RuleFor(x => x.GetTags()).NotEmpty().WithMessage("Add more tags to describe your package");
        }
    }
}