using System.Text;
using Microsoft.Extensions.Configuration;

namespace EnvConvert
{
    public class Converter
    {
        private readonly OutputFormat _format;
        private readonly YamlFormat _yamlFormat;
        private readonly Separator _separator;
        private readonly bool _includeEmpty;

        public Converter(OutputFormat format = OutputFormat.dotenv,
                         YamlFormat yamlFormat = YamlFormat.dockercompose,
                         Separator separator = Separator.colon,
                         bool includeEmpty = true)
        {
            _format = format;
            _yamlFormat = yamlFormat;
            _includeEmpty = includeEmpty;
            _separator = separator;
        }

        /// <summary>
        /// Convert JSON string into environment variables format
        /// 
        /// Modified from original JsonToEnvironmentConverter
        /// Source: https://github.com/flcdrg/JsonToEnvironmentConverter/blob/master/JsonToEnvironmentConverter/Pages/Index.cshtml.cs
        /// </summary>
        public string FromJSON(string input)
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

                    string format = _format switch
                    {
                        OutputFormat.yaml when _yamlFormat == YamlFormat.dockercompose => "- name: \"{0}\"\n" + "  value: \"{1}\"",
                        OutputFormat.yaml when _yamlFormat == YamlFormat.kubernetes =>
                            "{{\r\n    \"name\": \"{0}\",\r\n    \"value\": \"{1}\",\r\n    \"slotSetting\": false\r\n}},",
                        OutputFormat.yaml => "\"{0}\": \"{1}\"",
                        _ => "{0}={1}"
                    };

                    foreach ((string key, string value) in configurationRoot.AsEnumerable()
                                                                            .Where(pair => _includeEmpty || !string.IsNullOrEmpty(pair.Value))
                                                                            .OrderBy(pair => pair.Key))
                    {
                        string resultKey = (_separator == Separator.underscore) ? key.Replace(":", "__") : key;
                        sb.AppendFormat(format, resultKey, value);
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