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
        
        public static void DumpScore()
        {
            var tree = new Tree(new Markup(Emoji.Known.CheckMarkButton + "Provides valid .nuspec"));

// Add some nodes
            var foo = tree.AddNode("[yellow]The package description is too short.[/]");
            foo.AddNode("Add more detail to the description field of pubspec.yaml. Use 60 to 180 characters to describe the package, what it does, and its target use case.");
            
            var table = new Table()
                .Expand()
                .Border(TableBorder.Minimal)
                .AddColumn(new TableColumn("[u]Follows NuGet Conventions[/]"))
                .AddColumn(new TableColumn("[u]Score[/]"))
                .AddColumn(new TableColumn("[u]Total Score[/]"))
                .AddRow(tree, new Markup("10"), new Markup("[bold]10[/]"))
                .AddRow(new Markup(Emoji.Known.CrossMark + "Provides valid README.md"), new Markup("0"), new Markup("[bold]5[/]"))
                .AddRow(new Markup(Emoji.Known.CheckMarkButton + "Provides valid RELEASENOTES.md"), new Markup("5"), new Markup("[bold]5[/]"));

            AnsiConsole.Render(table);
        }
    }
}