using FluentValidation;
using Score.Models;

namespace Score.Validations
{
    public class NuspecValidator : AbstractValidator<PackageContext>
    {
        public NuspecValidator()
        {
            RuleFor(x => x.NuspecReader.GetDescription()).Length(60, 180)
                .WithMessage("Add more detail to the description field of .nuspec. Use 60 to 180 characters" +
                             " to describe the package, what it does, and its target use case.");
            When(x => x.NuspecReader.GetIcon() != null,
                () =>
                {
                    RuleFor(x => x.NuspecReader.GetIcon()).NotEmpty().WithMessage("Add an icon for your package");
                }).Otherwise(() =>
            {
                RuleFor(x => x.NuspecReader.GetIconUrl()).NotEmpty().WithMessage("Add an icon for your package");
            });

            RuleFor(x => x.NuspecReader.GetTags()).NotEmpty().WithMessage("Add more tags to describe your package");
        }
    }
}