using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Score.Commands;

namespace Score.Models
{
    public class PackageContext
    {
        public PackageContext(ScoreCommand.Settings settings)
        {
            PackageName = settings?.PackageName;
            PackageVersion = settings?.PackageVersion;
            NuGetVersion = new NuGetVersion(PackageVersion);
        }

        public string PackageName { get; set; }
        public string PackageVersion { get; set; }
        public NuGetVersion NuGetVersion { get; set; }
        public NuspecReader NuspecReader { get; set; }
        public IPackageSearchMetadata PackageMetadata { get; set; }
    }
}