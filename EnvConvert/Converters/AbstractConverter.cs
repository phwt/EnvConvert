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
    }
}