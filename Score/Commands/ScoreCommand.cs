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
            
            ScoreDumper.DumpScore(score);
            //ScoreDumper.DumpSettings(settings);
            return 0;
        }

        public sealed class Settings : ScoreSettings
        {
        }
    }
}