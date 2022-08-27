# EnvConvert

.NET CLI tool for converting JSON into various environment variables format.

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
