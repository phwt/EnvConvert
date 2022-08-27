using System.CommandLine;

namespace EnvConvert
{
    internal class Program
    {
        private static void InputOutputHandler(FileInfo file, string? outputOption, Func<string, string> converter)
        {
            var result = converter(File.ReadAllText(file.FullName));
            if (String.IsNullOrWhiteSpace(outputOption))
            {
                System.Console.WriteLine(result);
            }
            else
            {
                File.WriteAllText(outputOption, result);
            }
        }

        private static async Task<int> Main(string[] args)
        {
            // Root and subcommands
            var rootCommand = new RootCommand("CLI Tools for converting configurations into environment variables format");
            var jsonCommand = new Command("json", "Convert from JSON format");

            rootCommand.Add(jsonCommand);

            // Global options
            var fileOption = new Option<FileInfo>(new string[] { "--file", "-f" }, "Path to the input file");
            var outputOption = new Option<string?>(new string[] { "--out", "-o" }, "Output file - defaults to STDOUT if unspecified");

            rootCommand.AddGlobalOption(fileOption);
            rootCommand.AddGlobalOption(outputOption);

            // Handlers
            jsonCommand.SetHandler((fileOption, outputOption) =>
                InputOutputHandler(fileOption, outputOption, new Converter().FromJSON),
            fileOption, outputOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
