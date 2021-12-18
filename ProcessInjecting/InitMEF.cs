// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using Models;
using PEProfiles;
using ProcessInjecting.common;

namespace ProcessInjecting
{
    public class InitMEF
    {
        private static void ParseMetadata(IService service)
        {
            CommonUtils.ConsoleSide.print_under("Name:\t\t");
            Console.WriteLine(service.Injectionmetadata.Name);
            CommonUtils.ConsoleSide.print_under("Description:\t");
            Console.WriteLine(service.Injectionmetadata.Description);
            CommonUtils.ConsoleSide.print_under("References:\t\n");
            foreach (var item in service.Injectionmetadata.References)
            {
                Console.WriteLine("\t\t" + item);
            }
            CommonUtils.ConsoleSide.print_under("Principles:\t\n");
            foreach (var item in service.Injectionmetadata.Principles)
            {
                Console.WriteLine("\t\t" + item);
            }
        }
        internal static void Run(string processkind)
        {
            if (processkind.Equals("ProcessHollowing")) { Misc.process_Hollowing_flag = true; };
            if (processkind.Equals("ProcessDoppelganging")) { Misc.process_Doppelganging_flag = true; };
            if (processkind.Equals("ModuleStomping")) { Misc.process_ModuleStomping = true; };

            var catalog = new DirectoryCatalog(@"./InjectionPlugins", "*.dll");
            using (CompositionContainer container = new CompositionContainer(catalog))
            {
                IEnumerable<Lazy<IService>> services = container.GetExports<IService>();
                if (services != null)
                {
                    foreach (var service in services)
                    {
                        if (service.Value.GetType().ToString().Contains("ProcessHollowing") && Misc.process_Hollowing_flag)
                        {
                            ParseMetadata(service.Value);

                            Object[] args = new object[] { Misc.HollowedProcessX85, Misc.shellcode, Profile.pe };
                            // service.Value.paras[0].Values.ToArray()[0] = args;
                            service.Value.paras[0][ProcessKind.ProcessHollowing] = args;
                            service.Value.Init();
                        }
                        else if (service.Value.GetType().ToString().Contains("ProcessDoppelganging") && Misc.process_Doppelganging_flag)
                        {
                            ParseMetadata(service.Value);

                            Object[] args = new object[] { Misc.HollowedProcessX85, Misc.shellcode, Profile.pe };
                            // service.Value.paras[0].Values.ToArray()[0] = args;
                            service.Value.paras[0][ProcessKind.ProcessDoppelganging] = args;
                            service.Value.Init();
                        }
                        else if (service.Value.GetType().ToString().Contains("ModuleStomping") && Misc.process_ModuleStomping)
                        {
                            ParseMetadata(service.Value);
                            // 每一种注入技术参数传递
                            Object[] args = new object[] { true, Misc.shellcode, Profile.pe };
                            // service.Value.paras[0].Values.ToArray()[0] = args;
                            service.Value.paras[0][ProcessKind.ModuleStomping] = args;
                            service.Value.Init();
                        }
                        else
                        {
                            //CommonUtils.ConsoleSide.Underline("underline");
                            //CommonUtils.ConsoleSide.print_warn(processkind + " not found");
                        }
                    }
                }
            }
        }
    }
}
