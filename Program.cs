using System.CommandLine;

namespace EnvConvert
{
    internal class Program
    {
        private static async Task<int> Main(string[] args)
        {
            var fileOption = new Option<FileInfo?>(name: "--file", description: "Input file");

            var rootCommand = new RootCommand("CLI Tools for converting configurations into environment variables format");
            rootCommand.AddOption(fileOption);
            rootCommand.SetHandler((file) =>
            {
                if (file != null)
                {
                    var jsonStr = File.ReadAllText(file.FullName);
                    var jc = new JsonConverter();
                    var res = jc.Convert(jsonStr);
                    System.Console.WriteLine(res);
                }
            },
            fileOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
