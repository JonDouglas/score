using System.Linq;
using Spectre.Console;
using Spectre.Console.Rendering;
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
            AnsiConsole.Render(new FigletText(".NET SCORE").Centered().Color(Color.Purple));
            //Score -> ScoreReport -> 5 List<ScoreSections> (Iterate) -> ScoreSection (Tree & Nodes & Table)
            var nugetConventionsTable = new Table();
            nugetConventionsTable
                .Collapse()
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[u]Follows NuGet Conventions[/]").Footer("[bold u].NET Score[/]"))
                .AddColumn(new TableColumn("[u]Score[/]").Footer($"[bold u]{score.ScoreReport.FollowsNuGetConventions.Sum(x => x.CurrentScore)}[/]"))
                .AddColumn(new TableColumn("[u]Total Score[/]").Footer($"[bold u]{score.ScoreReport.FollowsNuGetConventions.Sum(x => x.MaxScore)}[/]"));

            if (score?.ScoreReport != null)
            {
                foreach (var scoreSection in score.ScoreReport?.FollowsNuGetConventions)
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

                    nugetConventionsTable
                        .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                            new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                }

                AnsiConsole.Render(nugetConventionsTable);

                //Add a BreakdownChart() when 1.0.0 is released in Spectre.Console.

                var providesDocumentationTable = new Table();
                providesDocumentationTable
                    .Collapse()
                    .Border(TableBorder.Rounded)
                    .AddColumn(new TableColumn("[u]Provides Documentation[/]").Footer("[bold u].NET Score[/]"))
                    .AddColumn(new TableColumn("[u]Score[/]").Footer($"[bold u]{score.ScoreReport.ProvidesDocumentation.Sum(x => x.CurrentScore)}[/]"))
                    .AddColumn(new TableColumn("[u]Total Score[/]").Footer($"[bold u]{score.ScoreReport.ProvidesDocumentation.Sum(x => x.MaxScore)}[/]"));

                if (score?.ScoreReport != null)
                {
                    foreach (var scoreSection in score.ScoreReport?.ProvidesDocumentation)
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

                        providesDocumentationTable
                            .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                                new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                    }

                    AnsiConsole.Render(providesDocumentationTable);

                    var supportsMultiplePlatformsTable = new Table();
                    supportsMultiplePlatformsTable
                        .Collapse()
                        .Border(TableBorder.Rounded)
                        .AddColumn(new TableColumn("[u]Supports Multiple Platforms[/]").Footer("[bold u].NET Score[/]"))
                        .AddColumn(new TableColumn("[u]Score[/]").Footer($"[bold u]{score.ScoreReport.SupportsMultiplePlatforms.Sum(x => x.CurrentScore)}[/]"))
                        .AddColumn(new TableColumn("[u]Total Score[/]").Footer($"[bold u]{score.ScoreReport.SupportsMultiplePlatforms.Sum(x => x.MaxScore)}[/]"));

                    if (score?.ScoreReport != null)
                    {
                        foreach (var scoreSection in score.ScoreReport?.SupportsMultiplePlatforms)
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

                            supportsMultiplePlatformsTable
                                .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                                    new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                        }

                        AnsiConsole.Render(supportsMultiplePlatformsTable);
                    

                        // var passStaticAnalysisTable = new Table();
                        // passStaticAnalysisTable
                        //     .Expand()
                        //     .Border(TableBorder.Minimal)
                        //     .AddColumn(new TableColumn("[u]Pass Static Analysis[/]"))
                        //     .AddColumn(new TableColumn("[u]Score[/]"))
                        //     .AddColumn(new TableColumn("[u]Total Score[/]"));
                        //
                        // if (score?.ScoreReport != null)
                        // {
                        //     foreach (var scoreSection in score.ScoreReport?.PassStaticAnalysis)
                        //     {
                        //         //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                        //         var scoreSectionTree = new Tree(new Markup(scoreSection.Title));
                        //
                        //         if (scoreSection.Summaries != null)
                        //             foreach (var scoreSectionSummary in scoreSection?.Summaries)
                        //             {
                        //                 var issueNode =
                        //                     scoreSectionTree.AddNode($"[yellow]{scoreSectionSummary.Issue}[/]");
                        //                 issueNode.AddNode(
                        //                     $"[red]{scoreSectionSummary.Resolution}[/]");
                        //             }
                        //
                        //         passStaticAnalysisTable
                        //             .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                        //                 new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                        //     }
                        //
                        //     AnsiConsole.Render(passStaticAnalysisTable);

                        var upToDateDependenciesTable = new Table();
                        upToDateDependenciesTable
                            .Collapse()
                            .Border(TableBorder.Rounded)
                            .AddColumn(new TableColumn("[u]Up-to-date Dependencies[/]").Footer("[bold u].NET Score[/]"))
                            .AddColumn(new TableColumn("[u]Score[/]").Footer($"[bold u]{score.ScoreReport.SupportUpToDateDependencies.Sum(x => x.CurrentScore)}[/]"))
                            .AddColumn(new TableColumn("[u]Total Score[/]").Footer($"[bold u]{score.ScoreReport.SupportUpToDateDependencies.Sum(x => x.MaxScore)}[/]"));

                        if (score?.ScoreReport != null)
                        {
                            foreach (var scoreSection in score.ScoreReport?.SupportUpToDateDependencies)
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

                                upToDateDependenciesTable
                                    .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                                        new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                            }

                            AnsiConsole.Render(upToDateDependenciesTable);
                        }
                    }
                }
            }
        }
    }
}