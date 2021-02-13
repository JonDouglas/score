using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Score.Commands;

namespace Score.Models
{
    public class PackageContext
    {
        public string PackageName { get; set; }
        public string PackageVersion { get; set; }
        public NuGetVersion NuGetVersion { get; set; }
        public NuspecReader NuspecReader { get; set; }
        public IPackageSearchMetadata PackageMetadata { get; set; }

        public PackageContext(ScoreCommand.Settings settings)
        {
            PackageName = settings?.PackageName;
            PackageVersion = settings?.PackageVersion;
            NuGetVersion = new NuGetVersion(PackageVersion);
        }

        
        
    }
    
    
}