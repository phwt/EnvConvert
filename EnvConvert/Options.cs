namespace EnvConvert
{
    public enum OutputFormat
    {
        dotenv,
        docker,
        yaml
    }

    public enum YamlFormat
    {
        dockercompose,
        kubernetes,
        azureappsettings
    }

    public enum Separator
    {
        colon,
        underscore
    }
}