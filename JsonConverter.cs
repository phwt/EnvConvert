using System.Text;
using Microsoft.Extensions.Configuration;

namespace EnvConvert
{
    public class JsonConverter
    {
        private string Format { get; set; } = "Docker";
        private bool IncludeEmpty { get; set; } = true;
        private string Separator { get; set; } = "Colon";
        private string YamlFormat { get; set; } = "DockerCompose";

        public string Convert(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var builder = new ConfigurationBuilder();
                var stream = new MemoryStream(input.Length);
                var sw = new StreamWriter(stream);
                sw.Write(input);
                sw.Flush();
                stream.Position = 0;
                builder.AddJsonStream(stream);

                try
                {
                    var configurationRoot = builder.Build();

                    var sb = new StringBuilder();

                    string format = Format switch
                    {
                        "Yaml" when YamlFormat == "Kubernetes" => "- name: \"{0}\"\n" + "  value: \"{1}\"",
                        "Yaml" when YamlFormat == "AzureAppSettings" =>
                            "{{\r\n    \"name\": \"{0}\",\r\n    \"value\": \"{1}\",\r\n    \"slotSetting\": false\r\n}},",
                        "Yaml" => "\"{0}\": \"{1}\"",
                        _ => "{0}={1}"
                    };

                    foreach ((string key, string value) in configurationRoot.AsEnumerable()
                        .Where(pair => IncludeEmpty || !string.IsNullOrEmpty(pair.Value))
                        .OrderBy(pair => pair.Key))
                    {
                        string key2 = key;
                        if (Separator == "Underscore")
                        {
                            key2 = key2.Replace(":", "__");
                        }
                        sb.AppendFormat(format, key2, value);
                        sb.AppendLine();
                    }

                    return sb.ToString();

                }
                catch
                {
                    throw new Exception("Unable to convert JSON");
                }
            }
            throw new Exception("JSON input is empty");
        }
    }
}