using System.ComponentModel;
using System.Threading.Tasks;
using Score.Models;
using Score.Services;
using Score.Utilities;
using Spectre.Console.Cli;

namespace Score.Commands
{
    [Description("Score a NuGet package.")]
    public sealed class ScoreCommand : AsyncCommand<ScoreCommand.Settings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var nuGetService = new NuGetService();
            var packageContext = new PackageContext(settings);
            //if packageContext does not have a version, we need to get latest version.
            //But let's make it required for now
            //then we can get the nuspec from the package.
            packageContext.NuspecReader = await nuGetService.GetNuspecFromPackage(packageContext);
            packageContext.PackageMetadata = await nuGetService.GetNuGetPackageMetadataFromPackage(packageContext);

            var scoreService = new ScoreService();
            var score = await scoreService.ScorePackage(packageContext);

            //Create a Score & pass the packagecontext
            // Models.Score score = new Models.Score()
            // {
            //     ScoreReport = new ScoreReport()
            //     {
            //         FollowsNuGetConventions = new List<ScoreSection>()
            //         {
            //             new ScoreSection()
            //             {
            //                 Title = "Provides valid .nuspec",
            //                 CurrentScore = 2,
            //                 MaxScore = 5,
            //                 Status = true,
            //                 Summaries = new List<Summary>()
            //                 {
            //                     new Summary()
            //                     {
            //                         Issue = "The package description is too short.",
            //                         Resolution = "Add more detail to the description field of the .nuspec."
            //                     }
            //                 }
            //             },
            //             new ScoreSection()
            //             {
            //                 Title = "Provides valid README",
            //                 CurrentScore = 0,
            //                 MaxScore = 5
            //             },
            //             new ScoreSection()
            //             {
            //                 Title = "Provides valid CHANGELOG",
            //                 CurrentScore = 5,
            //                 MaxScore = 5
            //             }          
            //         },
            //         ProvidesDocumentation = new List<ScoreSection>()
            //         {
            //             new ScoreSection()
            //             {
            //                 Title = "Package has example",
            //                 CurrentScore = 0,
            //                 MaxScore = 10,
            //                 Status = true,
            //                 Summaries = new List<Summary>()
            //                 {
            //                     new Summary()
            //                     {
            //                         Issue = "The package contains no example.",
            //                         Resolution = "Add an example/ to your package layout."
            //                     }
            //                 }
            //             },
            //             new ScoreSection()
            //             {
            //                 Title = "20% or more public API is commented",
            //                 CurrentScore = 0,
            //                 MaxScore = 10
            //             }
            //         }
            //     }
            // };
            ScoreDumper.DumpScore(score);
            ScoreDumper.DumpSettings(settings);
            return 0;
        }

        public sealed class Settings : ScoreSettings
        {
        }
    }
}