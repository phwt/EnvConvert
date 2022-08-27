using System.CommandLine;

namespace EnvConvert
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var fileOption = new Option<FileInfo?>(new string[] { "--file", "-f" }, "Input file");

            var rootCommand = new RootCommand("CLI Tools for converting configurations into environment variables format");
            rootCommand.AddOption(fileOption);
            rootCommand.SetHandler((file) =>
            {
                if (file != null)
                {
                    var jsonString = File.ReadAllText(file.FullName);
                    var result = new Converter().FromJSON(jsonString);
                    System.Console.WriteLine(result);
                }
            },
            fileOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
