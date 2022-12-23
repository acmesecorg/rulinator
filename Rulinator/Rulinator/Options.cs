using CommandLine;

namespace Rulinator
{
    public class Options
    {
        public Options() 
        {
            FilePath = "";
        }

        [Value(0, HelpText = "Paths of file(s) to turn into rules")]
        public string FilePath { get; set; }

        [Option("int-start", HelpText = "Start integer")]
        public int IntegerStart { get; set; }

        [Option("int-end", HelpText = "End integer")]
        public int IntegerEnd { get; set; }

        [Option("prepend", HelpText = "Create prepend instead of append rules")]
        public bool Prepend { get; set; }

        [Option('p', "passthrough", HelpText = "Include a passthrough rule (:) at the beginning of the file")]
        public bool Passthrough { get; set; }
    }
}
