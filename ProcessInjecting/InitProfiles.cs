// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using PEProfiles;
using System;

namespace ProcessInjecting
{
    public class InitProfiles
    {
        public static void Run(string filename)
        {
            if (Profile.Execute(filename))
            {
#if DEBUG
                Console.WriteLine("userwx:" + Profile.pe.Generic.userwx);
                Console.WriteLine("syscall:" + Profile.pe.Generic.dinvoke);
                Console.WriteLine("syscall:" + Profile.pe.Generic.syscall);
                Console.WriteLine("hellgates:" + Profile.pe.Generic.hellgates);
                Console.WriteLine("patchAmsi:" + Profile.pe.Generic.patchAmsi);
                Console.WriteLine("patchEtw:" + Profile.pe.Generic.patchEtw);
                Console.WriteLine("Blockdlls:" + Profile.pe.Generic.Blockdlls);
                Console.WriteLine("XOR:" + Profile.pe.Generic.XOR);
                Console.WriteLine("sleep_time:" + Profile.pe.Generic.sleep_time);
                Console.WriteLine("spwanto:" + Profile.pe.Generic.spwanto);
                Console.WriteLine(Profile.pe.Generic.unhooks[0] + "\n" + Profile.pe.Generic.unhooks[1] + "\n" + Profile.pe.Generic.unhooks[2] + "\n");
#endif
            }
        }
    }
}
