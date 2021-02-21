﻿using System.Threading.Tasks;
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
            var settings = new ScoreCommand.Settings {PackageName = "Xamarin.Android.CirclePageIndicator", PackageVersion = "1.1.1"};
            var nuGetService = new NuGetService();
            var packageContext = new PackageContext(settings);
            packageContext.NuspecReader = await nuGetService.GetNuspecFromPackage(packageContext);
            packageContext.PackageMetadata = await nuGetService.GetNuGetPackageMetadataFromPackage(packageContext);

            var scoreService = new ScoreService();
            var score = await scoreService.ScorePackage(packageContext);
            //var result = await nuGetService.GetValidNuSpecScoreSectionAsync(packageContext);

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
            //         }
            //     }
            // };
            ScoreDumper.DumpScore(score);
        }
    }
}