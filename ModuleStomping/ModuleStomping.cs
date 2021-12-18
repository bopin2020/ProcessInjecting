using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleStomping
{
    [Export(typeof(IService))]
    [ExportMetadata("name", "ModuleStomping")]
    public class ModuleStomping :  IService
    {
        public Metadata Injectionmetadata { get; set; }

        public List<Dictionary<ProcessKind, Object[]>> paras { get; set; }


        public ModuleStomping()
        {
            Injectionmetadata = new Metadata
            {
                Name = "Module Stomping",
                Description = "Module Stomping (which also seems to by the names Module Overloading and Dll Hollowing)" +
                                "is a shellcode injection technique",
                References = new string[] {
                    "https://github.com/countercept/ModuleStomping",
                    "https://blog.f-secure.com/hiding-malicious-code-with-module-stomping/",
                    "https://offensivedefence.co.uk/posts/module-stomping/",
                    "https://www.ired.team/offensive-security/code-injection-process-injection/modulestomping-dll-hollowing-shellcode-injection",

                },
                Principles = new string[] {
                    "1. Create a process or open a handle to an existing process",
                    "2. Fore that process to load a legitimate DLL from disk",
                    "3. Write the shellcode somewhere into the DLL",
                    "4. Kick off execution using CreateRemoteThread or other (eg.UserQueueAPC also works)"
                },
                Opinion = @"原理: 注入shellcode或dll到合法的dll中,然后启用线程执行shellcode;关键点是从磁盘加载合法的dll
                            然后在合法dll内创建远程线程"
            };
            paras = new List<Dictionary<ProcessKind, object[]>>();
            paras.Add(new Dictionary<ProcessKind, object[]>()
            {
                { ProcessKind.ModuleStomping, new object[]{  } },
            });
        }

        public bool Init()
        {
            bool IsCreateProcess = false;
            string processname = "";
            byte[] shellcode = null;
            PEProfile pe = new PEProfile();
            foreach (var item in paras[0].Values)
            {
                int length = item.Length;
                processname = item[0].ToString();
                shellcode = (byte[])item[1];
                pe = (PEProfile)item[length - 1];
            }
            // TODO:  完善Module Stomping

            Console.WriteLine("Invoke Module Stomping");
            return true;
        }
    }
}
