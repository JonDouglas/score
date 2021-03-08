using System.Threading.Tasks;
using Score.Commands;
using Score.Models;
using Score.Services;
using Score.Utilities;
using Spectre.Console;
using VerifyXunit;
using Xunit;

[UsesVerify]
public class Tests
{
    [Fact]
    public async Task DumpScore()
    {
        var settings = new ScoreCommand.Settings {PackageName = "Newtonsoft.Json", PackageVersion = "12.0.3"};
        var nuGetService = new NuGetService();
        var packageContext = new PackageContext(settings);
        packageContext.NuspecReader = await nuGetService.GetNuspecFromPackage(packageContext);
        packageContext.PackageMetadata = await nuGetService.GetNuGetPackageMetadataFromPackage(packageContext);

        var scoreService = new ScoreService();
        var score = await scoreService.ScorePackage(packageContext);

        AnsiConsole.Record();
        ScoreDumper.DumpScore(score);

        await Verifier.Verify(AnsiConsole.ExportText());
    }
}