using System;
using System.Threading.Tasks;
using Score.Commands;
using Score.Models;
using Score.Services;

namespace Score.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ScoreCommand.Settings settings = new ScoreCommand.Settings()
                {PackageName = "AutoFac", PackageVersion = "6.1.0"};
            NuGetService nuGetService = new NuGetService();
            PackageContext packageContext = new PackageContext(settings);
            packageContext.NuspecReader = await nuGetService.GetNuspecFromPackage(packageContext);
            packageContext.PackageMetadata = await nuGetService.GetNuGetPackageMetadataFromPackage(packageContext);
            var result = await nuGetService.GetValidNuSpecScoreSectionAsync(packageContext);

        }
    }
}