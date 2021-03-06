using System.Threading.Tasks;
using Score.Commands;
using Spectre.Console.Cli;

namespace Score
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var app = new CommandApp();
            app.SetDefaultCommand<ScoreCommand>();
            app.Configure(config =>
            {
                config.SetApplicationName("score");
                config.ValidateExamples();
                config.AddExample(new[] {"<PACKAGE_NAME>", "<VERSION>"});
            });

            return await app.RunAsync(args);
        }
    }
}