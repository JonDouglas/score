using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Score.Models;
using Score.Validations;
using Score.Validations.NuGetConventions;
using Score.Validations.ProvidesDocumentation;
using Score.Validations.SupportsMultiplePlatforms;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
            foreach (var nugetFramework in context.NuGetFrameworkDocumentationList)
            {
                var validator = new ApiDocumentationValidator();
                var results = await validator.ValidateAsync(nugetFramework);
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

            return new ScoreSection()
            {
                Title = "Public API has XML Documentation",
                MaxScore = 10,
                CurrentScore = summaries.Count > 0 ? 0 : 10,
                Status = true,
                Summaries = summaries
            };

        }

        public async Task<ScoreSection> GetMultiplePlatformsScoreSectionAsync(PackageContext context)
        {
            List<Summary> summaries = new List<Summary>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isLatest = false;
            bool isCompatible = false;

            if(context.NuGetFrameworkDocumentationList.Any(x => x.NuGetFramework.GetShortFolderName() == "net5.0"))
            {
                isLatest = true;
            }

            if (context.NuGetFrameworkDocumentationList.Any(x =>
                x.NuGetFramework.GetShortFolderName() == "netstandard2.0"))
            {
                isCompatible = true;
            }
            foreach (var platform in context.NuGetFrameworkDocumentationList)
            {
                if (platform.NuGetFramework.GetShortFolderName() == "net5.0" ||
                    platform.NuGetFramework.GetShortFolderName() == "netstandard2.0")
                {
                    var validator = new MultiplePlatformsValidator();
                    var results = await validator.ValidateAsync(platform.NuGetFramework);

                    validationResults.Add(results);
                }
            }

            Summary summary = new Summary();
            if (!isCompatible && !isLatest)
            {
                summary = new Summary()
                {
                    Issue = "Target the most compatible & latest target frameworks",
                    Resolution = "Multi-target your library for both 'netstandard2.0' and 'net5.0'"
                };
            }
            else if (isCompatible && !isLatest)
            {
                summary = new Summary()
                {
                    Issue = "Target the latest target framework",
                    Resolution = "Multi-target your library for 'net5.0'"
                };
            }
            else if (isLatest && !isCompatible)
            {
                summary = new Summary()
                {
                    Issue = "Target the most compatible target framework",
                    Resolution = "Multi-target your library for 'netstandard2.0'"
                };
            }

            summaries.Add(summary);


            return new ScoreSection()
            {
                Title = "Target the most compatible & latest frameworks.",
                MaxScore = 20,
                CurrentScore = validationResults.Count > 0 ? validationResults.Count * 10 : 0,
                Status = true,
                Summaries = summaries
            };
        }

        public async Task<ScoreSection> GetStaticAnalysisScoreSectionAsync(PackageContext context)
        {
            return new ScoreSection();
        }

        public async Task<ScoreSection> GetUpToDateDependenciesScoreSectionAsync(PackageContext context)
        {
            //All package dependencies are supported in the latest version. 10 score
            //Let's do either all dependencies are updated fully or no points.
            NuGetService nuGetService = new NuGetService();
            var packageDependencies = context.PackageMetadata.DependencySets;
            List<bool> updatedPackages = new List<bool>();
            List<Summary> summaries = new List<Summary>();
            foreach (var dependency in packageDependencies)
            {
                foreach (var package in dependency.Packages)
                {
                    var latestVersion =
                        await nuGetService.GetLatestNuGetVersion(package.Id, package.VersionRange.MinVersion);
                    var result = await nuGetService.IsLatestNuGetVersion(package.Id, package.VersionRange.MinVersion);
                    if (!result)
                    {
                        updatedPackages.Add(result);
                        Summary summary = new Summary()
                        {
                            Issue = $"{dependency.TargetFramework.GetShortFolderName() + "/" + package.Id} is not the latest version ({package.VersionRange.MinVersion}).",
                            Resolution = $"Update {dependency.TargetFramework.GetShortFolderName() + "/" + package.Id} to the latest stable version ({latestVersion})."
                        };
                        summaries.Add(summary);
                        //create a summary if the package is not up to date.
                        //Microsoft.Csharp is version 4.3.0, update to 4.7.0
                    }
                }
            }
            return new ScoreSection()
            {
                Title = "Has up-to-date dependencies",
                MaxScore = 10,
                CurrentScore = updatedPackages.Count > 0 ? 0 : 10,
                Status = true,
                Summaries = summaries
            };
        }

        public async Task<ScoreSection> GetOptimizedLibrariesAsync(PackageContext context)
        {
            NuGetService nuGetService = new NuGetService();
            var libraries = await context.PackageArchiveReader.GetLibItemsAsync(default);
            var coreAsm = typeof(object).Assembly;
            var dir = Path.GetDirectoryName(coreAsm.Location);
            string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
            var par = new PathAssemblyResolver(runtimeAssemblies);

            List<string> debugLibraries = new List<string>();
            List<Summary> summaries = new List<Summary>();
            foreach (var lib in libraries)
            {
                var dlls = lib.Items.Where(f => StringComparer.OrdinalIgnoreCase.Equals(Path.GetExtension(f), ".dll"));
                foreach (var dll in dlls)
                {
                    using var mlc = new MetadataLoadContext(par, coreAsm.FullName);
                    using Stream dllStream = context.PackageArchiveReader.GetStream(dll);
                    using var ms = new MemoryStream();
                    await dllStream.CopyToAsync(ms);

                    var asm = mlc.LoadFromStream(ms);

                    var cads = asm.GetCustomAttributesData();
                    bool isOptimized = true;
                    foreach (var cad in cads)
                    {
                        var name = cad.AttributeType.FullName;
                        if (name == typeof(DebuggableAttribute).FullName)
                        {
                            var args = cad.ConstructorArguments;
                            switch (args.Count)
                            {
                                case 1:
                                    var val = (int)args[0].Value;
                                    isOptimized = (val & 0x100) == 0;
                                    break;
                                case 2:
                                    isOptimized = (bool)args[1].Value == false;
                                    break;
                                default:
                                    // who knows
                                    break;
                            }
                            break;
                        }
                    }
                    if (!isOptimized)
                    {
                        debugLibraries.Add(dll);
                        var summary = new Summary()
                        {
                            Issue = $"Assembly {dll} is not built with optimizations enabled.",
                            Resolution = "Build with optimizations enabled, ie 'release' build."
                        };
                        summaries.Add(summary);
                    }
                }
            }

            return new ScoreSection()
            {
                Title = "Has optimized libraries",
                MaxScore = 10,
                CurrentScore = debugLibraries.Count > 0 ? 0 : 10,
                Status = true,
                Summaries = summaries
            };
        }
    }
}