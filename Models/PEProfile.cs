using System;

namespace Models
{
    public class PEProfile
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
}
