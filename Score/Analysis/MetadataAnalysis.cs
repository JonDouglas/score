using NuGet.Packaging;
using Score.Models;

namespace Score.Analysis
{
    public static class MetadataAnalysis
    {
        static bool SupportUpdatedDependencies(PackageContext context)
        {
            //Go through each Nuspec check & write a summary if any best practice is missing.
            return true;
        }
    }
}