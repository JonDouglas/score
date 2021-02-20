using System;
using Score.Models;
using Spectre.Console;
using Spectre.Console.Cli;

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

        public static void DumpScore(Models.Score score)
        {
            //Score -> ScoreReport -> 5 List<ScoreSections> (Iterate) -> ScoreSection (Tree & Nodes & Table)
            var table = new Table();
            table
                .Expand()
                .Border(TableBorder.Minimal)
                .AddColumn(new TableColumn("[u]Follows NuGet Conventions[/]"))
                .AddColumn(new TableColumn("[u]Score[/]"))
                .AddColumn(new TableColumn("[u]Total Score[/]"));
            
            foreach (var scoreSection in score.ScoreReport.FollowsNuGetConventions)
            {
                //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                var scoreSectionTree = new Tree(new Markup(scoreSection.Title));

                if (scoreSection.Status)
                {
                    foreach (var scoreSectionSummary in scoreSection.Summaries)
                    {
                        var issueNode = scoreSectionTree.AddNode($"[yellow]{scoreSectionSummary.Issue}[/]");
                        issueNode.AddNode(
                            $"[red]{scoreSectionSummary.Resolution}[/]");
                    }
                }

                table
                    .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                        new Markup($"[bold]{scoreSection.MaxScore}[/]"));
            }

            AnsiConsole.Render(table);
        }

    }
}