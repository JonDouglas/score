using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Score.Analysis;
using Score.Commands;
using Score.Models;
using Score.Validations;
using Spectre.Console;

namespace Score.Services
{
    public class NuGetService
    {
        public async Task<NuGetVersion> GetLatestNuGetVersion(PackageContext context)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            IEnumerable<NuGetVersion> versions = await resource.GetAllVersionsAsync(
                context.PackageName,
                cache,
                logger,
                cancellationToken);

            var nuGetVersions = versions.ToList();
            foreach (NuGetVersion version in nuGetVersions)
            {
                Console.WriteLine($"Found version {version}");
            }

            return nuGetVersions.LastOrDefault();
        }

        public async Task<NuspecReader> DownloadNuGetPackage(ScoreSettings settings, NuGetVersion version)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            string packageId = settings.PackageName;
            NuGetVersion packageVersion = version;
            using MemoryStream packageStream = new MemoryStream();

            await resource.CopyNupkgToStreamAsync(
                packageId,
                packageVersion,
                packageStream,
                cache,
                logger,
                cancellationToken);

            Console.WriteLine($"Downloaded package {packageId} {packageVersion}");

            using PackageArchiveReader packageReader = new PackageArchiveReader(packageStream);
            NuspecReader nuspecReader = await packageReader.GetNuspecReaderAsync(cancellationToken);

            Console.WriteLine($"Tags: {nuspecReader.GetTags()}");
            Console.WriteLine($"Description: {nuspecReader.GetDescription()}");
            
            Console.WriteLine("Dependencies:");
            foreach (var dependencyGroup in nuspecReader.GetDependencyGroups())
            {
                Console.WriteLine($" - {dependencyGroup.TargetFramework.GetShortFolderName()}");
                foreach (var dependency in dependencyGroup.Packages)
                {
                    Console.WriteLine($"   > {dependency.Id} {dependency.VersionRange}");
                }
            }

            Console.WriteLine("Files:");
            foreach (var file in packageReader.GetFiles())
            {
                Console.WriteLine($" - {file}");
            }
            Console.WriteLine("Libs:");
            foreach (var lib in packageReader.GetLibItems())
            {
                Console.WriteLine($" - {lib.TargetFramework.GetShortFolderName()}");
            }
            return nuspecReader;
        }

        public async Task ReadNuGetPackageMetadata(NuspecReader nuspecReader)
        {
            // Create the table.
            var table = CreateTable(nuspecReader);

            // Render the table.
            AnsiConsole.Render(table);
        }

        public async Task<IPackageSearchMetadata> GetNuGetPackageMetadataFromPackage(PackageContext context)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;
            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            PackageMetadataResource resource = await repository.GetResourceAsync<PackageMetadataResource>(cancellationToken);

            var package = await resource.GetMetadataAsync(
                new PackageIdentity(context.PackageName, context.NuGetVersion),
                cache,
                logger,
                cancellationToken);

            return package;
        }
        
        private static Table CreateTable(NuspecReader nuspecReader)
        {

            var second = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Green)
                .AddColumn(new TableColumn("[u]Name[/]"))
                .AddColumn(new TableColumn("[u]Score[/]"))
                .AddColumn(new TableColumn("[u]Total[/]"))
                .AddRow(new Text(Emoji.Known.GlobeShowingEuropeAfrica), new Text(Emoji.Known.Frog + nuspecReader.GetDescription()), new Text(Emoji.Known.Rocket));

            return new Table()
                .Centered()
                .Border(TableBorder.DoubleEdge)
                .Title($"DOTNET [yellow]SCORE[/] - [red]{nuspecReader.GetId()}[/]")
                .Caption("TOTAL [yellow]SCORE[/]")
                .AddColumn(
                    new TableColumn(new Panel("[u]POPULARITY[/]").BorderColor(Color.Red)).Footer(
                        "[u]POPULARITY SCORE[/]"))
                .AddColumn(
                    new TableColumn(new Panel("[u]QUALITY[/]").BorderColor(Color.Green)).Footer("[u]QUALITY SCORE[/]"))
                .AddColumn(
                    new TableColumn(new Panel("[u]MAINTENANCE[/]").BorderColor(Color.Blue)).Footer(
                        "[u]MAINTENANCE SCORE[/]"))
                .AddRow(second, second, second);
        }
        
        public async Task<NuspecReader> GetNuspecFromPackage(PackageContext context)
        {
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;

            SourceCacheContext cache = new SourceCacheContext();
            SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            FindPackageByIdResource resource = await repository.GetResourceAsync<FindPackageByIdResource>();

            string packageId = context.PackageName;
            NuGetVersion packageVersion = context.NuGetVersion;
            using MemoryStream packageStream = new MemoryStream();

            await resource.CopyNupkgToStreamAsync(
                packageId,
                packageVersion,
                packageStream,
                cache,
                logger,
                cancellationToken);

            Console.WriteLine($"Downloaded package {packageId} {packageVersion}");

            using PackageArchiveReader packageReader = new PackageArchiveReader(packageStream);
            return await packageReader.GetNuspecReaderAsync(cancellationToken);
        }

        public async Task<ScoreSection> GetValidNuSpecScoreSectionAsync(PackageContext context)
        {
            NuspecValidator validator = new NuspecValidator();
            var results = await validator.ValidateAsync(context.NuspecReader);
            return new()
            {
                Title = "Provide a valid .nuspec",
                MaxScore = 10,
                CurrentScore = context.NuspecReader != null ? 10 : 0
            };
        }
        
        public async Task<ScoreSection> GetReleaseNoteScoreSectionAsync(PackageContext context)
        {
            return new()
            {
                Title = "Provide a valid RELEASENOTEs.md",
                MaxScore = 5,
                CurrentScore = NuspecAnalysis.ProvidesReleaseNotes(context) ? 5 : 0
            };
        }
        
    }
}