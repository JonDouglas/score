using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Score.Models;
using Score.Validations;
using Score.Validations.NuGetConventions;
using Score.Validations.ProvidesDocumentation;
using Score.Validations.SupportsMultiplePlatforms;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Score.Services
{
    public class ScoreSectionService
    {
        //Refactor these methods to take in PackageContext context, Validator, ref/out ScoreSection
        public async Task<ScoreSection> GetValidNuSpecScoreSectionAsync(PackageContext context)
        {
            var validator = new NuspecValidator();
            var results = await validator.ValidateAsync(context);
            //Start to score the nuspec vs. the results.
            List<Summary> summaries = new List<Summary>();
            foreach (var failure in results.Errors)
            {
                Summary summary = new Summary()
                {
                    Issue = failure.PropertyName,
                    Resolution = failure.ErrorMessage
                };
                summaries.Add(summary);
            }
            return new ScoreSection()
            {
                Title = "Has valid .nuspec",
                MaxScore = 10,
                CurrentScore = 10 - results.Errors.Count,
                Status = results.IsValid,
                Summaries = summaries
            };
        }

        public async Task<ScoreSection> GetReleaseNoteScoreSectionAsync(PackageContext context)
        {
            var validator = new ReleaseNotesValidator();
            var results = await validator.ValidateAsync(context);
            //Start to score the nuspec vs. the results.
            List<Summary> summaries = new List<Summary>();
            foreach (var failure in results.Errors)
            {
                Summary summary = new Summary()
                {
                    Issue = failure.PropertyName,
                    Resolution = failure.ErrorMessage
                };
                summaries.Add(summary);
            }
            return new ScoreSection()
            {
                Title = "Provide valid release notes",
                MaxScore = 10,
                CurrentScore = results.Errors.Count > 0 ? 0 : 10,
                Status = results.IsValid,
                Summaries = summaries
            };
        }
        
        public async Task<ScoreSection> GetApiDocumentationScoreSectionAsync(PackageContext context)
        {
            List<Summary> summaries = new List<Summary>();
            foreach (var nugetFramework in context.NuGetFrameworkDocumentationList)
            {
                var validator = new ApiDocumentationValidator();
                var results = await validator.ValidateAsync(nugetFramework);
                foreach (var failure in results.Errors)
                {
                    Summary summary = new Summary()
                    {
                        Issue = failure.PropertyName,
                        Resolution = failure.ErrorMessage
                    };
                    summaries.Add(summary);
                }
            }

            return new ScoreSection()
            {
                Title = "Public API has XML Documentation",
                MaxScore = 10,
                CurrentScore = summaries.Count > 0 ? 0 : 10,
                Status = true,
                Summaries = summaries
            };
            
        }
        
        public async Task<ScoreSection> GetMultiplePlatformsScoreSectionAsync(PackageContext context)
        {
            List<Summary> summaries = new List<Summary>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isLatest = false;
            bool isCompatible = false;
            if(context.NuGetFrameworkDocumentationList.Any(x => x.NuGetFramework.GetShortFolderName() == "net5.0"))
            {
                isLatest = true;
            }

            if (context.NuGetFrameworkDocumentationList.Any(x =>
                x.NuGetFramework.GetShortFolderName() == "netstandard2.0"))
            {
                isCompatible = true;
            }
            foreach (var platform in context.NuGetFrameworkDocumentationList)
            {
                if (platform.NuGetFramework.GetShortFolderName() == "net5.0" ||
                    platform.NuGetFramework.GetShortFolderName() == "netstandard2.0")
                {
                    var validator = new MultiplePlatformsValidator();
                    var results = await validator.ValidateAsync(platform.NuGetFramework);

                    validationResults.Add(results);
                }
            }
            
            Summary summary = new Summary();
            if (isCompatible && isLatest)
            {
                summary = new Summary()
                {
                    Issue = "Target the most compatible & latest target frameworks",
                    Resolution = "Multi-target your library for both 'netstandard2.0' and 'net5.0'"
                };
            }
            else if (isCompatible)
            {
                summary = new Summary()
                {
                    Issue = "Target the latest target framework",
                    Resolution = "Multi-target your library for 'net5.0'"
                };
            }
            else if (isLatest)
            {
                summary = new Summary()
                {
                    Issue = "Target the most compatible target framework",
                    Resolution = "Multi-target your library for 'netstandard2.0'"
                };
            }

            summaries.Add(summary);
            
            
            return new ScoreSection()
            {
                Title = "Target the most compatible & latest frameworks.",
                MaxScore = 20,
                CurrentScore = validationResults.Count > 0 ? validationResults.Count * 10 : 0,
                Status = true,
                Summaries = summaries
            };
        }
    }
}