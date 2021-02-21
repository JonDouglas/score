using FluentValidation;
using Score.Models;

namespace Score.Validations
{
    public class NuspecValidator : AbstractValidator<PackageContext>
    {
        public NuspecValidator()
        {
            RuleFor(x => x.NuspecReader.GetVersion().OriginalVersion).Length(0, 64)
                .WithName("<licenseUrl> is deprecated.")
                .WithMessage("Add a license to your package.");
            
            RuleFor(x => x.NuspecReader.GetLicenseUrl()).NotEmpty()
                .WithName("<licenseUrl> is deprecated.")
                .WithMessage("Add a license to your package.");
            
            RuleFor(x => x.NuspecReader.GetLicenseMetadata()).NotEmpty()
                .WithName("License is missing.")
                .WithMessage("Add a license to your package.");
            
            RuleFor(x => x.NuspecReader.GetProjectUrl()).NotEmpty()
                .WithName("<projectUrl> is missing.")
                .WithMessage("Add a <projectUrl> to your package.");
            
            RuleFor(x => x.NuspecReader.GetRepositoryMetadata()).NotEmpty()
                .WithName("<repository> is missing")
                .WithMessage("Add a <repository> to your package.");
            
            RuleFor(x => x.NuspecReader.GetOwners()).NotEmpty()
                .WithName("<owners> is missing.")
                .WithMessage("Add <owners> to your .nuspec");
            RuleFor(x => x.NuspecReader.GetCopyright()).NotEmpty()
                .WithName("<copyright> is missing.")
                .WithMessage("Add <copyright> to your .nuspec");
            RuleFor(x => x.NuspecReader.GetDescription()).NotEmpty()
                .WithName("<description> is missing.")
                .WithMessage("Add a <description> to your .nuspec");
            RuleFor(x => x.NuspecReader.GetDescription()).Length(60, 180)
                .WithName("<description> is too short")
                .WithMessage("Add more detail to the description field of .nuspec. Use 60 to 180 characters" +
                             " to describe the package, what it does, and its target use case.");

            RuleFor(x => x.NuspecReader.GetIcon()).NotEmpty().WithName("<icon> is missing.").WithMessage("Add an icon to your package and reference it within an <icon> element.");
   
            RuleFor(x => x.NuspecReader.GetIconUrl()).NotEmpty().WithName("<iconUrl> is deprecated. Use <icon> instead.").WithMessage("Add an icon to your package and reference it within an <icon> element.");


            RuleFor(x => x.NuspecReader.GetTags()).NotEmpty().WithName("<tags> is missing.").WithMessage("Add tags to the <tags> element to describe your package");
        }
    }
}