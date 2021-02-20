using System.Collections.Generic;

namespace Score.Models
{
    public class ScoreReport
    {
        public List<ScoreSection> FollowsNuGetConventions { get; set; }
        public List<ScoreSection> ProvidesDocumentation { get; set; }
        public List<ScoreSection> SupportsMultiplePlatforms { get; set; }
        public List<ScoreSection> PassStaticAnalysis { get; set; }
        public List<ScoreSection> SupportUpdatedDependencies { get; set; }
    }
}