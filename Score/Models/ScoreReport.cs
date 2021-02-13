using System.Collections.Generic;

namespace Score.Models
{
    public class ScoreReport
    {
        private List<ScoreSection> FollowsNuGetConventions { get; set; }
        private List<ScoreSection> ProvidesDocumentation { get; set; }
        private List<ScoreSection> SupportsMultiplePlatforms { get; set; }
        private List<ScoreSection> PassStaticAnalysis { get; set; }
        private List<ScoreSection> SupportUpdatedDependencies { get; set; }
    }
}