using System.Collections.Generic;
using System.Threading.Tasks;
using Score.Models;

namespace Score.Services
{
    public class ScoreService
    {
        private async Task<List<ScoreSection>> FollowsNuGetConventionsAsync(PackageContext context)
        {
            NuGetService nuGetService = new NuGetService();
            //Provides a valid .nuspec file. 10 score
            ScoreSection nuspecScoreSection = await nuGetService.GetValidNuSpecScoreSectionAsync(context);
            //Provides a valid README file. 5 score
            //This doesn't exist so when it does we'll add it.
            //Provides a valid CHANGELOG/RELEASENOTES file. 5 score
            ScoreSection releaseNotesScoreSection = await nuGetService.GetReleaseNoteScoreSectionAsync(context);
            return new List<ScoreSection>() {nuspecScoreSection, releaseNotesScoreSection};
        }

        private async Task<List<ScoreSection>> ProvidesDocumentationAsync(PackageContext context)
        {
            //Provides an example. 10 score
            
            //20% of more of public API has XML comments. 10 score
            return new List<ScoreSection>();
        }

        private async Task<List<ScoreSection>> SupportsMultiplePlatformsAsync(PackageContext context)
        {
            //Supports all possible platforms. 20 score
            return new List<ScoreSection>();
        }

        private async Task<List<ScoreSection>> PassStaticAnalysisAsync(PackageContext context)
        {
            //Code has no errors, warnings, lints, or formatting issues. 30 score
            return new List<ScoreSection>();
        }

        private async Task<List<ScoreSection>> SupportUpdatedDependenciesAsync(PackageContext context)
        {
            //All package dependencies are supported in the latest version. 10 score
            
            //Package supports latest .NET SDKs. 10 score
            return new List<ScoreSection>();
        }
    }
}