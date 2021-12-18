using System;

namespace Models
{
    public class PEProfile
    {

        public CommonModule Common { get; set; } = new CommonModule();
        public GenericModle Generic { get; set; } = new GenericModle();
        public MiscModule Misc { get; set; } = new MiscModule();


        public sealed class CommonModule
        { 
            public string ProfileName { get; set; }

            public string Description { get; set; }

            public string Author { get; set; }

            public string Url { get; set; }

            public string Date { get; set; }

        }

        public sealed class GenericModle
        {
            public bool userwx { get; set; }
            public bool dinvoke { get; set; }
            public bool syscall { get; set; }
            public bool hellgates { get; set; }
            public bool patchAmsi { get; set; }
            public bool patchEtw { get; set; }
            public bool Blockdlls { get; set; }
            public Byte XOR { get; set; }
            public int sleep_time { get; set; }

            public string[] unhooks { get; set; }
            public string spwanto { get; set; }
        }

        public sealed class MiscModule
        { 
            
        }
    }
}
