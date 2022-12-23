using CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Rulinator
{
    internal class Program
    {
        [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(Options))]
        static void Main(string[] args)
        {
            //Parse command line 
            var parsed = Parser.Default.ParseArguments<Options>(args);
            var options = parsed.Value;

            if (parsed.Tag == ParserResultType.NotParsed)
            {
                ConsoleUtil.WriteError("One or more arguments were invalid");
                return;
            }

            //Check file exists
            var currentDirectory = Directory.GetCurrentDirectory();
            var mergedPath = Path.Combine(currentDirectory, options.FilePath);
            var hasInput = File.Exists(mergedPath);
            var hasIntegers = options.IntegerEnd > options.IntegerStart;
            var isAppend = !options.Prepend;
            var isPrepend = options.Prepend;
            var finalName = "";
            var fileLength = 0L;

            //Determine if we need to do anything
            if (!hasInput && !hasIntegers) return;

            //Get file details
            if (hasInput)
            {
                var fileInfo = new FileInfo(mergedPath);
                finalName = fileInfo.Name.Replace(fileInfo.Extension, "");
                fileLength = fileInfo.Length;
            }
            else
            {
                finalName = "rulinator";
                if (hasIntegers) finalName = "integer";
            }

            var appendName = $"{finalName}.append.rule";
            var prependName = $"{finalName}.prepend.rule";
            var appendPath = Path.Combine(currentDirectory, appendName);
            var prependPath = Path.Combine(currentDirectory, prependName);

            //Check if paths exist
            if (File.Exists(appendPath) && isAppend)
            {
                ConsoleUtil.WriteError($"File {appendName} already exists.");
                return;
            }
            if (File.Exists(prependPath) && isPrepend)
            {
                ConsoleUtil.WriteError($"File {prependName} already exists.");
                return;
            }

            var appends = new List<String>();
            var prepends = new List<string>();

            //First pass, write out lines
            if (hasInput)
            {
                using var reader = new StreamReader(mergedPath);
                var count = 0;

                while (!reader.EndOfStream)
                {
                    var word = reader.ReadLine();
                    if (word == null) continue;

                    if (isAppend) appends.Add(Map(word, '$'));
                    if (isPrepend) prepends.Add(Map(word.Reverse(), '^'));
                    
                    count++;

                    if (count % 100 == 0) ConsoleUtil.WriteProgress("Processing file", reader.BaseStream.Position, fileLength);
                }
            }

            //Write out integers
            if (hasIntegers)
            {
                for (var i = options.IntegerStart; i<= options.IntegerEnd; i++)
                {
                    var word = i.ToString();

                    if (isAppend) appends.Add(Map(word, '$'));
                    if (isPrepend) prepends.Add(Map(word.Reverse(), '^'));

                    if (i % 100 == 0) ConsoleUtil.WriteProgress("Processing integers", options.IntegerEnd - i, options.IntegerEnd - options.IntegerStart);
                }
            }

            if (appends.Count > 0)
            {
                File.AppendAllLines(appendPath, appends, Encoding.UTF8);
                ConsoleUtil.WriteMessage($"Wrote {appends.Count} lines to {appendName}");
            }

            if (prepends.Count > 0)
            {
                ConsoleUtil.WriteMessage($"Wrote {prepends.Count} lines to {prependName}");
                File.AppendAllLines(prependPath, prepends, Encoding.UTF8);
            }
        }

        static string Map(IEnumerable<char> word, char d)
        {
            var result = new StringBuilder();

            foreach (var c in word)
            {
                result.Append(d);
                result.Append(c);
            }

            return result.ToString();
        }
    }
}