using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Mono.Cecil;
using NuGet.Frameworks;
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
            var scoreSectionService = new ScoreSectionService();
            //Provides a valid .nuspec file. 10 score
            var nuspecScoreSection = await scoreSectionService.GetValidNuSpecScoreSectionAsync(context);
            //Provides a valid README file. 5 score
            //This doesn't exist so when it does we'll add it.
            //Provides a valid CHANGELOG/RELEASENOTES file. 5 score
            var releaseNotesScoreSection = await scoreSectionService.GetReleaseNoteScoreSectionAsync(context);
            return new List<ScoreSection> {nuspecScoreSection, releaseNotesScoreSection};
        }

        private async Task<List<ScoreSection>> ProvidesDocumentationAsync(PackageContext context)
        {
            //Provides an example. 10 score
            
            //20% of more of public API has XML comments. 10 score
            //Load the assembly from disk (downloaded & Assembly)
            //Read more on https://www.mono-project.com/docs/tools+libraries/libraries/Mono.Cecil/faq/
            
            foreach (var target in context.PackageArchiveReader.GetLibItems())
            {
                int members = 0;
                int documentedMembers = 0;
                var assemblyStringPath = context.PackageArchiveReader.GetFiles().Where(x => x.Contains(target.TargetFramework.GetShortFolderName())).Where(x => x.EndsWith(".dll")).FirstOrDefault();
                await using var assemblyStream = context.PackageArchiveReader.GetStream(assemblyStringPath);

                await using(var assemblyMemoryStream = new MemoryStream()) {
                    await assemblyStream.CopyToAsync(assemblyMemoryStream);
                    assemblyMemoryStream.Position = 0;
                    AssemblyDefinition ad = AssemblyDefinition.ReadAssembly(assemblyMemoryStream);
                    foreach (TypeDefinition type in ad.MainModule.Types) {
                        if (type.IsPublic)
                        {
                            members += type.Methods.Count;
                            members += type.Fields.Count;
                            members += type.Properties.Count;
                        }
                    }
                }
            
                var xmlStringPath = context.PackageArchiveReader.GetFiles().Where(x => x.Contains(target.TargetFramework.GetShortFolderName())).Where(x => x.EndsWith(".xml")).FirstOrDefault();
                await using var xmlStream = context.PackageArchiveReader.GetStream(xmlStringPath);
            
                await using(var xmlMemoryStream = new MemoryStream()) {
                    await xmlStream.CopyToAsync(xmlMemoryStream);
                    xmlMemoryStream.Position = 0;
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(xmlMemoryStream);
                    documentedMembers = xmlDocument.GetElementsByTagName("member").Count;

                }

                NuGetFrameworkDocumentation frameworkDocumentation = new NuGetFrameworkDocumentation();
                frameworkDocumentation.NuGetFramework = target.TargetFramework;
                frameworkDocumentation.PublicApiDocumentationPercent = ((double) documentedMembers / members);

                context.NuGetFrameworkDocumentationList.Add(frameworkDocumentation);
            }
            
            
            var scoreSectionService = new ScoreSectionService();
            var apiDocumentationScoreSection = await scoreSectionService.GetApiDocumentationScoreSectionAsync(context);
            return new List<ScoreSection> { apiDocumentationScoreSection};
        }

        private async Task<List<ScoreSection>> SupportsMultiplePlatformsAsync(PackageContext context)
        {
            //Supports all possible platforms. 20 score
            //Check manually for netstandard2.0 & net5.0 lib shortnames
            //Give 10 points to each
            var libFolders = await context.PackageArchiveReader.GetLibItemsAsync(new CancellationToken());
            foreach (var lib in libFolders)
            {
                if (lib.TargetFramework.GetShortFolderName() == "netstandard2.0" ||
                    lib.TargetFramework.GetShortFolderName() == "net5.0")
                {
                    var t = "Yes";
                }
            }
            
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