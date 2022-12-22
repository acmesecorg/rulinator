using CommandLine.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rulinator
{
    internal class Options
    {
        [Value(0, HelpText = "Paths of file(s) to turn into rules")]
        public string FilePath { get; set; }
    }
}
