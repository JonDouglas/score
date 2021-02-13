using System.ComponentModel;
using Spectre.Console.Cli;

namespace Score.Commands
{
    public abstract class ScoreSettings : CommandSettings
    {
        [CommandArgument(0, "<PACKAGE_NAME>")]
        [Description("The package name to score.")]
        public string PackageName { get; set; }
        
        [CommandArgument(1,"<VERSION>")]
        [Description("The version of the package to add.")]
        public string PackageVersion { get; set; }
    }
}