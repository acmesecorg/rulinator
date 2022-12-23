# rulinator
Create Hashcat rule files from wordlists, integers or dates. Each word in the file supplied is converted to an append rule or optionally prepend rule.

For example, the word `acmesec` is turned into the append rule `$a$c$m$e$s$e$c` or optionally the prepend rule `^c^e^s^e^m^c^a`

## License
Rulinator is licensed under the MIT license. Refer to [license.txt](https://github.com/metacrackorg/metacrack/blob/main/LICENSE) for more information.

## Latest builds 

| Platform | Link |
| --- | --- |
| win-x64 | [rulinator-win-x64-1.1.0.7z](https://github.com/acmesecorg/rulinator/raw/main/Rulinator/Builds/rulinator-win-x64-1.1.0.7z)|
| linux-x64 | [rulinator-linux-x64-1.1.0.7z](https://github.com/acmesecorg/rulinator/raw/main/Rulinator/Builds/rulinator-linux-x64-1.1.0.7z)|
  
## Usage

Create *words.append.rule* append rule file from a file of words<br>
`rulinator words.txt`

Create *words.prepend.rule* prepend rule file from a file of words<br>
`rulinator words.txt --prepend`

Use the `-p` or `--passthough` option to include a passthough `:` rule at the top of the output file.

To generate a rule file that appends or prepends a range of integers to an attack, use the `--integer-end` and optionally `--integer-start` parameters <br>
`rulinator --integer-end 999 -p`
