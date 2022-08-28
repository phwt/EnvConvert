using System.Text;

namespace EnvConvert
{
    public abstract class AbstractConverter
    {
        protected readonly OutputFormat _format;
        protected readonly YamlFormat _yamlFormat;
        protected readonly Separator _separator;
        protected readonly bool _includeEmpty;

        public AbstractConverter(OutputFormat format = OutputFormat.dotenv,
                                 YamlFormat yamlFormat = YamlFormat.dockercompose,
                                 Separator separator = Separator.colon,
                                 bool includeEmpty = true)
        {
            _format = format;
            _yamlFormat = yamlFormat;
            _includeEmpty = includeEmpty;
            _separator = separator;
        }

        abstract public string Convert(string input);

        /// <summary>
        /// Apply formatting to the key/value pair in the format according to the options
        /// </summary>
        /// <returns>Formatted string</returns>
        protected string ApplyFormat(IEnumerable<KeyValuePair<string, string>> values, char defaultSeparator = ':')
        {
            var format = _format switch
            {
                OutputFormat.yaml when _yamlFormat == YamlFormat.kubernetes => "- name: \"{0}\"\n" + "  value: \"{1}\"",
                OutputFormat.yaml when _yamlFormat == YamlFormat.azureappsettings => "{{\r\n    \"name\": \"{0}\",\r\n    \"value\": \"{1}\",\r\n    \"slotSetting\": false\r\n}},",
                OutputFormat.yaml => "\"{0}\": \"{1}\"",
                _ => "{0}={1}"
            };

            var sb = new StringBuilder();
            var filteredValue = values.Where(pair => _includeEmpty || !string.IsNullOrEmpty(pair.Value))
                                      .OrderBy(pair => pair.Key);

            foreach ((string key, string value) in filteredValue)
            {
                string resultKey = _separator switch
                {
                    Separator.colon => key.Replace(defaultSeparator.ToString(), ":"),
                    Separator.underscore => key.Replace(defaultSeparator.ToString(), "__"),
                    _ => key
                };

                sb.AppendFormat(format, resultKey, value);
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}