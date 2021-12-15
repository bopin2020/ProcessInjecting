using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace ProcessDoppelganging
{
    [Export(typeof(IService))]
    [ExportMetadata("name", "ProcessDoppelganging")]
    public class ProcessDoppelganging : IService
    {
        public Metadata Injectionmetadata { get; set; }

        public List<Dictionary<ProcessKind, Object[]>> paras { get; set; }

        public ProcessDoppelganging()
        {
            Injectionmetadata = new Metadata
            {
                Name = "Process Doppelganging",
                Description = "Gives an ability to run a malicous executable under the cover of a legitimate one\nAlthough they both serve the same goal of process impersonation, they differ in implementation and make use of different API functions",
                References = new string[] {
                        "https://attack.mitre.org/techniques/T1055/013/",
                        "https://www.ired.team/offensive-security/code-injection-process-injection/process-doppelganging",
                        "https://www.blackhat.com/docs/eu-17/materials/eu-17-Liberman-Lost-In-Transaction-Process-Doppelganging.pdf",
                        "https://github.com/3gstudent/Inject-dll-by-Process-Doppelganging",
                        "https://gist.github.com/hfiref0x/a9911a0b70b473281c9da5daea9a177f",
                        "https://github.com/hasherezade/process_doppelganging",
                        "https://hshrzd.wordpress.com/2017/12/18/process-doppelganging-a-new-way-to-impersonate-a-process/",
                        "https://www.youtube.com/watch?v=Cch8dvp836w",
                        "Also, it is worth mentioning that Tal Liberman is an author of the AtomBombing injection",
                        "C# implement=>",
                        "https://github.com/n1xbyte/Toolkit/blob/69c24f8cdd898f20ac3a79e81cc6bb61de8fa197/C%23%20Stuff/ProcessDopplegang.cs"
                    },
                Principles = new string[] {
                        "1. CreateTransaction  for creating a transaction",
                        "2. CreateFileTransaction for creating a dummy file inside of this transaction and this dummy which was used to be stored our malicious payload",
                        "3. NtSectionSection for creating memory section for making a base of creating new process",
                        "4. RollbackTransaction for invoking roll back as well as closing our dummy file after creating the section",
                        "5. NtCreateProcessEx for creating a fileless process without filename path.",
                        "6. And the operating system creates a new process for tough work we also link a series of structs the the PEB. Finally create remote thread invokes the process from EntryPoint",
                        "[How to detect?] =>\nCompares if the image loaded in the memory matches the corresponding file on the disk",
                    }
            };
            paras = new List<Dictionary<ProcessKind, object[]>>();
            paras.Add(new Dictionary<ProcessKind, object[]>()
            {
                { ProcessKind.ProcessDoppelganging,new object[]{  } },
            });
        }

        public bool Init()
        {

            Console.WriteLine("Invoke process doppelganging");
            return true;
        }
    }
    public sealed class Loader
    {
        public static void start(string startfile,string endfile)
        {

        }
    }
}
