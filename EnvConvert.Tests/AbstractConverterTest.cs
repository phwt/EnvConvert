using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace EnvConvert.Tests;

public class AbstractConverterTest
{
    internal class TestConverter : AbstractConverter
    {
        public TestConverter(OutputFormat format,
                             YamlFormat yamlFormat,
                             Separator separator,
                             bool includeEmpty) : base(format, yamlFormat, separator, includeEmpty) { }

        public override string Convert(string input) => throw new System.NotImplementedException();

        public string PublicApplyFormat(IEnumerable<KeyValuePair<string, string>> values, char defaultSeparator = ':') => base.ApplyFormat(values, defaultSeparator);
    }

    private readonly Dictionary<string, string> _inputConfiguration = new Dictionary<string, string>()
    {
        { "ConnectionStrings", "" },
        { "ConnectionStrings:DefaultConnection", "Database=master;Server=(local);Integrated Security=SSPI;" },
        { "Tags", "" },
        { "Tags:0", ".NET" },
        { "Tags:1", "" },
        { "Tags:1:Type", "Tools" },
    };

    [Fact]
    public void ApplyFormatDotenvTest()
    {
        var result = new TestConverter(OutputFormat.dotenv, default, default, true).PublicApplyFormat(_inputConfiguration);

        result.Should().BeEquivalentTo("ConnectionStrings=\n"
            + "ConnectionStrings:DefaultConnection=Database=master;Server=(local);Integrated Security=SSPI;\n"
            + "Tags=\n"
            + "Tags:0=.NET\n"
            + "Tags:1=\n"
            + "Tags:1:Type=Tools\n");
    }

    [Fact]
    public void ApplyFormatYamlComposeTest()
    {
        var result = new TestConverter(OutputFormat.yaml, YamlFormat.dockercompose, default, true).PublicApplyFormat(_inputConfiguration);

        result.Should().BeEquivalentTo("\"ConnectionStrings\": \"\"\n"
            + "\"ConnectionStrings:DefaultConnection\": \"Database=master;Server=(local);Integrated Security=SSPI;\"\n"
            + "\"Tags\": \"\"\n"
            + "\"Tags:0\": \".NET\"\n"
            + "\"Tags:1\": \"\"\n"
            + "\"Tags:1:Type\": \"Tools\"\n");
    }

    [Fact]
    public void ApplyFormatYamlKubernetesTest()
    {
        var result = new TestConverter(OutputFormat.yaml, YamlFormat.kubernetes, default, true).PublicApplyFormat(_inputConfiguration);

        result.Should().BeEquivalentTo("- name: \"ConnectionStrings\"\n"
            + "  value: \"\"\n"
            + "- name: \"ConnectionStrings:DefaultConnection\"\n"
            + "  value: \"Database=master;Server=(local);Integrated Security=SSPI;\"\n"
            + "- name: \"Tags\"\n"
            + "  value: \"\"\n"
            + "- name: \"Tags:0\"\n"
            + "  value: \".NET\"\n"
            + "- name: \"Tags:1\"\n"
            + "  value: \"\"\n"
            + "- name: \"Tags:1:Type\"\n"
            + "  value: \"Tools\"\n");
    }

    [Fact]
    public void ApplyFormatYamlAzureAppSettingsTest()
    {
        var result = new TestConverter(OutputFormat.yaml, YamlFormat.azureappsettings, default, true).PublicApplyFormat(_inputConfiguration);

        result.Should().BeEquivalentTo("{\r\n"
            + "    \"name\": \"ConnectionStrings\",\r\n"
            + "    \"value\": \"\",\r\n"
            + "    \"slotSetting\": false\r\n"
            + "},\n"
            + "{\r\n"
            + "    \"name\": \"ConnectionStrings:DefaultConnection\",\r\n"
            + "    \"value\": \"Database=master;Server=(local);Integrated Security=SSPI;\",\r\n"
            + "    \"slotSetting\": false\r\n"
            + "},\n"
            + "{\r\n"
            + "    \"name\": \"Tags\",\r\n"
            + "    \"value\": \"\",\r\n"
            + "    \"slotSetting\": false\r\n"
            + "},\n"
            + "{\r\n"
            + "    \"name\": \"Tags:0\",\r\n"
            + "    \"value\": \".NET\",\r\n"
            + "    \"slotSetting\": false\r\n"
            + "},\n"
            + "{\r\n"
            + "    \"name\": \"Tags:1\",\r\n"
            + "    \"value\": \"\",\r\n"
            + "    \"slotSetting\": false\r\n"
            + "},\n"
            + "{\r\n"
            + "    \"name\": \"Tags:1:Type\",\r\n"
            + "    \"value\": \"Tools\",\r\n"
            + "    \"slotSetting\": false\r\n"
            + "},\n"
        );
    }

    [Fact]
    public void ApplyFormatUnderscoreTest()
    {
        var result = new TestConverter(OutputFormat.dotenv, default, Separator.underscore, true).PublicApplyFormat(_inputConfiguration);

        result.Should().BeEquivalentTo("ConnectionStrings=\n"
            + "ConnectionStrings__DefaultConnection=Database=master;Server=(local);Integrated Security=SSPI;\n"
            + "Tags=\n"
            + "Tags__0=.NET\n"
            + "Tags__1=\n"
            + "Tags__1__Type=Tools\n");
    }

    [Fact]
    public void ApplyFormatIgnoreEmptyTest()
    {
        var result = new TestConverter(OutputFormat.dotenv, default, default, false).PublicApplyFormat(_inputConfiguration);

        result.Should().BeEquivalentTo("ConnectionStrings:DefaultConnection=Database=master;Server=(local);Integrated Security=SSPI;\n"
            + "Tags:0=.NET\n"
            + "Tags:1:Type=Tools\n");
    }
}