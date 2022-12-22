using CommandLine;

namespace Rulinator
{
    public class Options
    {
        [Value(0, HelpText = "Paths of file(s) to turn into rules")]
        public string FilePath { get; set; }
    }
}
