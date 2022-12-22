using CommandLine;
using System.Diagnostics.CodeAnalysis;
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

            //Validate the file paths
            if (string.IsNullOrEmpty(options.FilePath))
            {
                ConsoleUtil.WriteError("A file path must be supplied");
                return;
            }

            //Check file exists
            var currentDirectory = Directory.GetCurrentDirectory();
            var mergedPath = Path.Combine(currentDirectory, options.FilePath);
            if (!System.IO.File.Exists(mergedPath))
            {
                ConsoleUtil.WriteError($"File {mergedPath} not found");
                return;
            }

            var fileInfo = new FileInfo(mergedPath);
            var finalName = fileInfo.Name.Replace(fileInfo.Extension, "");
            var appendName = $"{finalName}.append.rule";
            var prependName = $"{finalName}.prepend.rule";
            var appendPath = Path.Combine(currentDirectory, appendName);
            var prependPath = Path.Combine(currentDirectory, prependName);

            //Check if paths exist
            if (File.Exists(appendPath))
            {
                ConsoleUtil.WriteError($"File {appendName} already exists.");
                return;
            }
            if (File.Exists(prependPath))
            {
                ConsoleUtil.WriteError($"File {prependName} already exists.");
                return;
            }

            var appends = new List<String>();
            var prepends = new List<string>();

            //First pass, write out lines
            using var reader = new StreamReader(mergedPath);
            var count = 0;

            while (!reader.EndOfStream)
            {
                var word = reader.ReadLine();

                if (word == null) continue;

                var append = new StringBuilder();
                var prepend = new StringBuilder();

                foreach (var c in word)
                {
                    append.Append('$');
                    append.Append(c);
                }

                var reversed = word.Reverse();

                foreach (var c in reversed)
                {
                    prepend.Append('^');
                    prepend.Append(c);
                }

                appends.Add(append.ToString());
                prepends.Add(prepend.ToString());

                count++;

                if (count % 100 == 0) ConsoleUtil.WriteProgress("Processing", reader.BaseStream.Position, fileInfo.Length);
            }



            File.AppendAllLines(appendPath, appends);
            File.AppendAllLines(prependPath, prepends);

            ConsoleUtil.WriteMessage($"Wrote {appends.Count} lines to {appendName}");
            ConsoleUtil.WriteMessage($"Wrote {prepends.Count} lines to {prependName}");
        }
    }
}