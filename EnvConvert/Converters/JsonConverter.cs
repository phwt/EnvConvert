using Microsoft.Extensions.Configuration;

namespace EnvConvert
{
    public class JsonConverter : AbstractConverter
    {
        public JsonConverter(OutputFormat format,
                             YamlFormat yamlFormat,
                             Separator separator,
                             bool includeEmpty) : base(format, yamlFormat, separator, includeEmpty) { }

        /// <summary>
        /// Convert JSON string into environment variables format
        /// </summary>
        public override string Convert(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    var builder = new ConfigurationBuilder();
                    var stream = new MemoryStream(input.Length);
                    var sw = new StreamWriter(stream);
                    sw.Write(input);
                    sw.Flush();
                    stream.Position = 0;
                    builder.AddJsonStream(stream);

                    var configurationRoot = builder.Build();
                    base.ApplyFormat(configurationRoot.AsEnumerable());
                }
                catch (Exception e)
                {
                    throw new Exception($"Unable to convert from JSON: {e.ToString()}");
                }
            }

            throw new Exception("JSON input is empty");
        }
    }
}