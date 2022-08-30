# EnvConvert

.NET CLI tool for converting JSON into various environment variables format.

## Installation

[View `EnvConvert` package on NuGet](https://www.nuget.org/packages/EnvConvert)

```bash
# Example: Install as a global tool
dotnet tool install --global EnvConvert
```

## Usage

```bash
dotnet tool envconvert
envconvert # If `$HOME/.dotnet/tools` is added to PATH
```

```bash
> envconvert --help

Description:
  CLI Tools for converting configurations into environment variables format

Usage:
  EnvConvert [command] [options]

Options:
  -f, --file <file>                                          Path to the input file
  -o, --out <out>                                            Name of the output file [default: STDOUT]
  --format <docker|dotenv|yaml>                              Output format [default: dotenv]
  --yaml-format <azureappsettings|dockercompose|kubernetes>  YAML Subformat (only valid for `--format yaml`) [default: dockercompose]
  -s, --separator <colon|underscore>                         Nested value separator [default: colon]
  --include-empty                                            Include empty values [default: True]
  --version                                                  Show version information
  -?, -h, --help                                             Show help and usage information

Commands:
  json  Convert from JSON format
```

### Convert from JSON

**Input file: `input.json`**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Database=master;Server=(local);Integrated Security=SSPI;"
  },
  "Property": "Value",
  "Tags": [".NET", { "Type": "Tools" }]
}
```

**Convert from JSON with default options**

```sh
> envconvert json --file input.json --out output
```

**Output file: `output`**

```env
ConnectionStrings=
ConnectionStrings:DefaultConnection=Database=master;Server=(local);Integrated Security=SSPI;
Property=Value
Tags=
Tags:0=.NET
Tags:1=
Tags:1:Type=Tools\n
```
