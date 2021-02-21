using System.Collections.Generic;
using System.Threading.Tasks;
using Score.Models;

namespace Score.Services
{
    public class ScoreService
    {
        public async Task<Models.Score> ScorePackage(PackageContext context)
        {
            var score = new Models.Score();
            score.ScoreReport = new ScoreReport
            {
                FollowsNuGetConventions = await FollowsNuGetConventionsAsync(context),
                ProvidesDocumentation = await ProvidesDocumentationAsync(context),
                PassStaticAnalysis = await PassStaticAnalysisAsync(context),
                SupportsMultiplePlatforms = await SupportsMultiplePlatformsAsync(context),
                SupportUpToDateDependencies = await SupportUpToDateDependenciesAsync(context)
            };

            return score;
        }

        private async Task<List<ScoreSection>> FollowsNuGetConventionsAsync(PackageContext context)
        {
            var nuGetService = new NuGetService();
            //Provides a valid .nuspec file. 10 score
            var nuspecScoreSection = await nuGetService.GetValidNuSpecScoreSectionAsync(context);
            //Provides a valid README file. 5 score
            //This doesn't exist so when it does we'll add it.
            //Provides a valid CHANGELOG/RELEASENOTES file. 5 score
            var releaseNotesScoreSection = await nuGetService.GetReleaseNoteScoreSectionAsync(context);
            return new List<ScoreSection> {nuspecScoreSection, releaseNotesScoreSection};
        }

        private async Task<List<ScoreSection>> ProvidesDocumentationAsync(PackageContext context)
        {
            //Provides an example. 10 score

            //20% of more of public API has XML comments. 10 score
            return new();
        }

        private async Task<List<ScoreSection>> SupportsMultiplePlatformsAsync(PackageContext context)
        {
            //Supports all possible platforms. 20 score
            return new();
        }

        private async Task<List<ScoreSection>> PassStaticAnalysisAsync(PackageContext context)
        {
            //Code has no errors, warnings, lints, or formatting issues. 30 score
            //Code does all the health checks for assemblies.
            return new();
        }

        private async Task<List<ScoreSection>> SupportUpToDateDependenciesAsync(PackageContext context)
        {
            //All package dependencies are supported in the latest version. 10 score

            //Package supports latest .NET SDKs. 10 score
            return new();
        }
    }
}