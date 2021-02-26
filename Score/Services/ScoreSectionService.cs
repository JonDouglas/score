using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Score.Models;
using Score.Validations;
using Score.Validations.NuGetConventions;
using Score.Validations.ProvidesDocumentation;

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
            List<FluentValidation.Results.ValidationResult> validationResults = new List<FluentValidation.Results.ValidationResult>();
            foreach (var framework in context.NuGetFrameworkDocumentationList)
            {
                var validator = new ApiDocumentationValidator();
                var results = await validator.ValidateAsync(framework);
                validationResults.Add(results);
                //Start to score the nuspec vs. the results.
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

            int score = 0;
            bool status;

            if (validationResults.Any(x => x.Errors.Any()))
            {
                score = 0;
                status = false;
            }
            else
            {
                score = 10;
                status = true;
            }

            return new ScoreSection()
            {
                Title = "Public API has XML Documentation",
                MaxScore = 10,
                CurrentScore = score,
                Status = false,
                Summaries = summaries
            };
        }
    }
}