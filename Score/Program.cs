using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Score.Commands;
using Spectre.Console.Cli;

namespace Score
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.SetApplicationName("dotnet-score");
                config.ValidateExamples();
                config.AddExample(new[] {"score", "<PACKAGE_NAME>"});
                config.AddCommand<ScoreCommand>("score");
            });

            return await app.RunAsync(args);
        }
    }
}