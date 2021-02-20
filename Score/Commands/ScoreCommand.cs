using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Score.Models;
using Score.Services;
using Score.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;
using Score.Models;

namespace Score.Commands
{
    [Description("Score a NuGet package.")]
        public sealed class ScoreCommand : AsyncCommand<ScoreCommand.Settings>
        {
            public sealed class Settings : ScoreSettings
            {
                
            }

            public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
            {
                NuGetService nuGetService = new NuGetService();
                PackageContext packageContext = new PackageContext(settings);
                //if packageContext does not have a version, we need to get latest version.
                //But let's make it required for now
                //then we can get the nuspec from the package.
                packageContext.NuspecReader = await nuGetService.GetNuspecFromPackage(packageContext);
                packageContext.PackageMetadata = await nuGetService.GetNuGetPackageMetadataFromPackage(packageContext);
                
                //Create a Score & pass the packagecontext
                Models.Score score = new Models.Score()
                {
                    ScoreReport = new ScoreReport()
                    {
                        FollowsNuGetConventions = new List<ScoreSection>()
                        {
                            new ScoreSection()
                            {
                                Title = "Provides valid .nuspec",
                                CurrentScore = 2,
                                MaxScore = 5,
                                Status = true,
                                Summaries = new List<Summary>()
                                {
                                    new Summary()
                                    {
                                        Issue = "The package description is too short.",
                                        Resolution = "Add more detail to the description field of the .nuspec."
                                    }
                                }
                            },
                            new ScoreSection()
                            {
                                Title = "Provides valid README",
                                CurrentScore = 0,
                                MaxScore = 5
                            },
                            new ScoreSection()
                            {
                                Title = "Provides valid CHANGELOG",
                                CurrentScore = 5,
                                MaxScore = 5
                            }          
                        }
                    }
                };
                ScoreDumper.DumpScore(score);
                ScoreDumper.DumpSettings(settings);
                return 0;
            }
    }
}