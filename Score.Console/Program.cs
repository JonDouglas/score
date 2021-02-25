using System.Threading.Tasks;
using Score.Commands;
using Score.Models;
using Score.Services;
using Score.Utilities;

namespace Score.Console
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var settings = new ScoreCommand.Settings {PackageName = "Autofac", PackageVersion = "6.1.0"};
            var nuGetService = new NuGetService();
            var packageContext = new PackageContext(settings);
            packageContext.NuspecReader = await nuGetService.GetNuspecFromPackage(packageContext);
            packageContext.PackageMetadata = await nuGetService.GetNuGetPackageMetadataFromPackage(packageContext);

            var scoreService = new ScoreService();
            var score = await scoreService.ScorePackage(packageContext);

            ScoreDumper.DumpScore(score);
        }
    }
}