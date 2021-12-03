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
                Console.WriteLine("userwx:" + Profile.pe.userwx);
                Console.WriteLine("syscall:" + Profile.pe.dinvoke);
                Console.WriteLine("syscall:" + Profile.pe.syscall);
                Console.WriteLine("hellgates:" + Profile.pe.hellgates);
                Console.WriteLine("patchAmsi:" + Profile.pe.patchAmsi);
                Console.WriteLine("patchEtw:" + Profile.pe.patchEtw);
                Console.WriteLine("Blockdlls:" + Profile.pe.Blockdlls);
                Console.WriteLine("XOR:" + Profile.pe.XOR);
                Console.WriteLine("sleep_time:" + Profile.pe.sleep_time);
                Console.WriteLine("spwanto:" + Profile.pe.spwanto);
                Console.WriteLine(Profile.pe.unhooks[0] + "\n" + Profile.pe.unhooks[1] + "\n" + Profile.pe.unhooks[2] + "\n");
#endif
            }
        }
    }
}
