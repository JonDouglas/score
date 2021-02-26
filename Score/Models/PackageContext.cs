using System.Collections.Generic;
using System.IO;
using NuGet.Frameworks;
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
            NuGetFrameworkDocumentationList = new List<NuGetFrameworkDocumentation>();
        }

        public string PackageName { get; set; }
        public string PackageVersion { get; set; }
        public NuGetVersion NuGetVersion { get; set; }
        public NuspecReader NuspecReader { get; set; }
        public IPackageSearchMetadata PackageMetadata { get; set; }
        
        public PackageArchiveReader PackageArchiveReader { get; set; }
        
        public List<NuGetFrameworkDocumentation> NuGetFrameworkDocumentationList { get; set; }
        
    }

    public class NuGetFrameworkDocumentation
    {
        public NuGetFrameworkDocumentation()
        {
            
        }
        public NuGetFramework NuGetFramework { get; set; }
        public double PublicApiDocumentationPercent { get; set; }
    }
}