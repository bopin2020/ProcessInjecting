// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using Models;
using PEProfiles;

namespace ProcessInjecting
{
    public class InitMEF
    {
        private static void ParseMetadata(IService service)
        {
            Console.WriteLine("Name:\t" + service.Injectionmetadata.Name);
            Console.WriteLine("Description:\t" + service.Injectionmetadata.Description);
            Console.WriteLine("References:");
            foreach (var item in service.Injectionmetadata.References)
            {
                Console.WriteLine("\t" + item);
            }
            Console.WriteLine("Principles:");
            foreach (var item in service.Injectionmetadata.Principles)
            {
                Console.WriteLine("\t" + item);
            }
        }
        internal static void Run(string processkind)
        {
            if (processkind.Equals("ProcessHollowing")) { Misc.process_Hollowing_flag = true; };
            if (processkind.Equals("ProcessDoppelganging")) { Misc.process_Doppelganging_flag = true; };

            var catalog = new DirectoryCatalog(@"./InjectionPlugins", "*.dll");
            using (CompositionContainer container = new CompositionContainer(catalog))
            {
                IEnumerable<Lazy<IService>> services = container.GetExports<IService>();
                // IEnumerable<IService> services = container.GetExportedValues<IService>();
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
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
