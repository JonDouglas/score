using System;
using NuGet.Packaging;
using Score.Models;

namespace Score.Analysis
{
    public static class NuspecAnalysis
    {
        public static bool IsValidNuspec(PackageContext context)
        {
            //Go through each Nuspec check & write a summary if any best practice is missing.
            return true;
        }
        
        public static bool ProvidesReleaseNotes(PackageContext context)
        {
            if (context.NuspecReader.GetReleaseNotes() != String.Empty)
            {
                return true;
            }
            
            return false;
        }
    }
}