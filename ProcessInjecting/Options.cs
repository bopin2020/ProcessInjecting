// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessInjecting
{
    public class Options
    {
        [Option('p', "processkind", Required = true, HelpText = "Process Injection's type [ ProcessHollowing |  ProcessDoppelganging | ModuleStomping ]")]
        public string Processkind { get; set; }

        [Option('d', "demo", Required = true, HelpText = "Show you one demo for the specified process injection technique")]
        public string Demo { get; set; }

        [Option('y', "profile", Required = true, HelpText = "Opsec for Process Injection")]
        public string ProfilePath { get; set; }
    }
}
