using Score.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Linq;

namespace Score.Utilities
{
    public static class ScoreDumper
    {
        public static void DumpSettings(CommandSettings settings)
        {
            var table = new Table().RoundedBorder();
            table.AddColumn("[grey]Name[/]");
            table.AddColumn("[grey]Value[/]");

            var properties = settings.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(settings)
                    ?.ToString()
                    ?.Replace("[", "[[");

                table.AddRow(
                    property.Name,
                    value ?? "[grey]null[/]");
            }

            AnsiConsole.Render(table);
        }

        static Table CreateTable(string title, List<ScoreSection> sections)
        {
            var table = new Table();
            table
                .Collapse()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[u]" + title + "[/]").Footer("[bold u].NET Score[/]"))
                .AddColumn(new TableColumn("[u]Score[/]").Footer($"[bold u]{sections.Sum(x => x.CurrentScore)}[/]"))
                .AddColumn(new TableColumn("[u]Total Score[/]").Footer($"[bold u]{sections.Sum(x => x.MaxScore)}[/]"));

            foreach (var scoreSection in sections)
            {
                //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                var scoreSectionTree = new Tree(new Markup($"[bold]{scoreSection.Title}[/]"));

                if (scoreSection.Summaries != null)
                    foreach (var scoreSectionSummary in scoreSection?.Summaries)
                    {
                        var issueNode = scoreSectionTree.AddNode($"[yellow]{scoreSectionSummary.Issue}[/]");
                        issueNode.AddNode(
                            $"[red]{scoreSectionSummary.Resolution}[/]");
                    }

                table
                    .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                        new Markup($"[bold]{scoreSection.MaxScore}[/]"));
            }

            return table;
        }

        public static void DumpScore(Models.Score score)
        {
            if (score?.ScoreReport == null)
                return;

            AnsiConsole.Render(new FigletText(".NET SCORE").Centered().Color(Color.Purple));
            //Score -> ScoreReport -> 5 List<ScoreSections> (Iterate) -> ScoreSection (Tree & Nodes & Table)

            var nugetConventionsTable = CreateTable("Follows NuGet Conventions", score.ScoreReport.FollowsNuGetConventions);
            AnsiConsole.Render(nugetConventionsTable);

            //Add a BreakdownChart() when 1.0.0 is released in Spectre.Console.

            var providesDocumentationTable = CreateTable("Provides Documentation", score.ScoreReport.ProvidesDocumentation);
            AnsiConsole.Render(providesDocumentationTable);

            var supportsMultiplePlatformsTable = CreateTable("Supports Multiple Platforms", score.ScoreReport.SupportsMultiplePlatforms);
            AnsiConsole.Render(supportsMultiplePlatformsTable);

            var passStaticAnalysisTable = CreateTable("Pass Static Analysis", score.ScoreReport.PassStaticAnalysis);
            AnsiConsole.Render(passStaticAnalysisTable);

            var upToDateDependenciesTable = CreateTable("Up-to-date Dependencies", score.ScoreReport.SupportUpToDateDependencies);
            AnsiConsole.Render(upToDateDependenciesTable);
        }
    }
}