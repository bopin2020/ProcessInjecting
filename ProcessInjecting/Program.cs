// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Models;
using System.IO;
using ProcessInjecting.common;

namespace ProcessInjecting
{
    class Program
    {
        public static bool IsProfile { get; set; }

        /// <summary>
        /// CommandLine Parser stole from SharpC2 TeamServer and credit to @RastaMouse
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args).MapResult(RunOptions, HandleParseErrors);

            Console.ReadKey();
        }

        private static async Task RunOptions(Options opts)
        {
            if (File.Exists(opts.ProfilePath))
            {
                CommonUtils.ConsoleSide.Underline("======解析profile======>");
                InitProfiles.Run(opts.ProfilePath);
                IsProfile = true;
            }
            if (!typeof(ProcessKind).GetMembers().ToArray().Select(t => t.Name).Contains(opts.Processkind))
            {
                Console.WriteLine(opts.Processkind);
                return;
            }
            if (opts.Demo.Equals("Demo"))
            {
                InitMEF.Run(opts.Processkind);
            }
        }

        private static async Task HandleParseErrors(IEnumerable<Error> errs)
        {
#if DEBUG
            foreach (var err in errs)
                await Console.Error.WriteLineAsync(err?.ToString());
#endif
        }
    }
}
