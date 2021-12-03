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

namespace ProcessInjecting
{
    class Program
    {
        // CommandLine Parser stole from SharpC2 TeamServer and credit to @RastaMouse
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args).MapResult(RunOptions, HandleParseErrors);

            PPLQuery.ParseInfos();
            Console.ReadKey();
        }
        public static bool IsProfile { get; set; }
        private static async Task RunOptions(Options opts)
        {
            if (File.Exists(opts.ProfilePath))
            {
                InitProfiles.Run(opts.ProfilePath);
                // 初始化成功 后面才可以调用 PEProfile pe
                IsProfile = true;
            }
            if (!typeof(ProcessKind).GetMembers().ToArray().Select(t => t.Name).Contains(opts.Processkind))
            {
                Console.WriteLine(opts.Processkind);
                return;
            }
            /// 初始化MEF插件 调用进程注入实例
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
