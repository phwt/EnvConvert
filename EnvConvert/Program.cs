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
            // Root command and options
            var rootCommand = new RootCommand("CLI Tools for converting configurations into environment variables format");

            var fileOption = new Option<FileInfo>(new string[] { "--file", "-f" }, "Path to the input file");
            var outputOption = new Option<string?>(new string[] { "--out", "-o" }, "Name of the output file [default: STDOUT]");

            var formatOption = new Option<OutputFormat>("--format", () => OutputFormat.dotenv, "Output format");
            var yamlFormatOption = new Option<YamlFormat>("--yaml-format", () => YamlFormat.dockercompose, "YAML Subformat (only valid for `--format yaml`)");
            var separatorOption = new Option<Separator>(new string[] { "--separator", "-s" }, () => Separator.colon, "Nested value separator");
            var includeEmptyOption = new Option<Boolean>("--include-empty", () => true, "Include empty values");

            rootCommand.AddGlobalOption(fileOption);
            rootCommand.AddGlobalOption(outputOption);

            rootCommand.AddGlobalOption(formatOption);
            rootCommand.AddGlobalOption(yamlFormatOption);
            rootCommand.AddGlobalOption(separatorOption);
            rootCommand.AddGlobalOption(includeEmptyOption);

            // Subcommands and handlers
            var jsonCommand = new Command("json", "Convert from JSON format");
            rootCommand.Add(jsonCommand);

            // TODO: Find an easier way to pass a lot of options
            jsonCommand.SetHandler((fileOption, outputOption, formatOption, yamlFormatOption, separatorOption, includeEmptyOption) =>
                InputOutputHandler(fileOption, outputOption, new Converter(formatOption, yamlFormatOption, separatorOption, includeEmptyOption).FromJSON),
            fileOption, outputOption, formatOption, yamlFormatOption, separatorOption, includeEmptyOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
