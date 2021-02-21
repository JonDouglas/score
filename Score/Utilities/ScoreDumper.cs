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
            var nugetConventionsTable = new Table();
            nugetConventionsTable
                .Expand()
                .Border(TableBorder.Minimal)
                .AddColumn(new TableColumn("[u]Follows NuGet Conventions[/]"))
                .AddColumn(new TableColumn("[u]Score[/]"))
                .AddColumn(new TableColumn("[u]Total Score[/]"));

            if (score?.ScoreReport != null)
            {
                foreach (var scoreSection in score.ScoreReport?.FollowsNuGetConventions)
                {
                    //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                    var scoreSectionTree = new Tree(new Markup(scoreSection.Title));

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

                var providesDocumentationTable = new Table();
                providesDocumentationTable
                    .Expand()
                    .Border(TableBorder.Minimal)
                    .AddColumn(new TableColumn("[u]Provides Documentation[/]"))
                    .AddColumn(new TableColumn("[u]Score[/]"))
                    .AddColumn(new TableColumn("[u]Total Score[/]"));

                if (score?.ScoreReport != null)
                {
                    foreach (var scoreSection in score.ScoreReport?.ProvidesDocumentation)
                    {
                        //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                        var scoreSectionTree = new Tree(new Markup(scoreSection.Title));

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

                    var supportMultiplePlatformsTable = new Table();
                    supportMultiplePlatformsTable
                        .Expand()
                        .Border(TableBorder.Minimal)
                        .AddColumn(new TableColumn("[u]Support Multiple Platforms[/]"))
                        .AddColumn(new TableColumn("[u]Score[/]"))
                        .AddColumn(new TableColumn("[u]Total Score[/]"));

                    if (score?.ScoreReport != null)
                    {
                        foreach (var scoreSection in score.ScoreReport?.SupportsMultiplePlatforms)
                        {
                            //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                            var scoreSectionTree = new Tree(new Markup(scoreSection.Title));

                            if (scoreSection.Summaries != null)
                                foreach (var scoreSectionSummary in scoreSection?.Summaries)
                                {
                                    var issueNode = scoreSectionTree.AddNode($"[yellow]{scoreSectionSummary.Issue}[/]");
                                    issueNode.AddNode(
                                        $"[red]{scoreSectionSummary.Resolution}[/]");
                                }

                            supportMultiplePlatformsTable
                                .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                                    new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                        }

                        AnsiConsole.Render(supportMultiplePlatformsTable);

                        var passStaticAnalysisTable = new Table();
                        passStaticAnalysisTable
                            .Expand()
                            .Border(TableBorder.Minimal)
                            .AddColumn(new TableColumn("[u]Pass Static Analysis[/]"))
                            .AddColumn(new TableColumn("[u]Score[/]"))
                            .AddColumn(new TableColumn("[u]Total Score[/]"));

                        if (score?.ScoreReport != null)
                        {
                            foreach (var scoreSection in score.ScoreReport?.PassStaticAnalysis)
                            {
                                //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                                var scoreSectionTree = new Tree(new Markup(scoreSection.Title));

                                if (scoreSection.Summaries != null)
                                    foreach (var scoreSectionSummary in scoreSection?.Summaries)
                                    {
                                        var issueNode =
                                            scoreSectionTree.AddNode($"[yellow]{scoreSectionSummary.Issue}[/]");
                                        issueNode.AddNode(
                                            $"[red]{scoreSectionSummary.Resolution}[/]");
                                    }

                                passStaticAnalysisTable
                                    .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                                        new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                            }

                            AnsiConsole.Render(passStaticAnalysisTable);

                            var supportUpToDateDependenciesTable = new Table();
                            supportUpToDateDependenciesTable
                                .Expand()
                                .Border(TableBorder.Minimal)
                                .AddColumn(new TableColumn("[u]Support Up-to-date Dependencies[/]"))
                                .AddColumn(new TableColumn("[u]Score[/]"))
                                .AddColumn(new TableColumn("[u]Total Score[/]"));

                            if (score?.ScoreReport != null)
                                foreach (var scoreSection in score.ScoreReport?.SupportUpToDateDependencies)
                                {
                                    //If the scoreSection has no issues, just do the tree markup, otherwise add issues / resolution
                                    var scoreSectionTree = new Tree(new Markup(scoreSection.Title));

                                    if (scoreSection.Summaries != null)
                                        foreach (var scoreSectionSummary in scoreSection?.Summaries)
                                        {
                                            var issueNode =
                                                scoreSectionTree.AddNode($"[yellow]{scoreSectionSummary.Issue}[/]");
                                            issueNode.AddNode(
                                                $"[red]{scoreSectionSummary.Resolution}[/]");
                                        }

                                    supportUpToDateDependenciesTable
                                        .AddRow(scoreSectionTree, new Markup($"{scoreSection.CurrentScore}"),
                                            new Markup($"[bold]{scoreSection.MaxScore}[/]"));
                                }

                            AnsiConsole.Render(supportUpToDateDependenciesTable);
                        }
                    }
                }
            }
        }
    }
}