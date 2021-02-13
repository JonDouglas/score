using System.Collections.Generic;

namespace Score.Models
{
    public class ScoreSection
    {
        public int Id { get; set; }
        public int CurrentScore { get; set; }
        public int MaxScore { get; set; }
        public string Title { get; set; }
        public List<Summary> Summaries { get; set; }
        public bool Status { get; set; }
    }

    public class Summary
    {
        public string Issue { get; set; }
        public string Resolution { get; set; }
    }
}