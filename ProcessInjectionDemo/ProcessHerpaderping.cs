// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessInjectionDemo
{
    /// <summary>
    /// https://jxy-s.github.io/herpaderping/
    /// https://github.com/jxy-s/herpaderping#process-herpaderping
    /// Process Herpaderping is a method of obscuring the intentions of a process by modifying the content on disk after the image has been mapped.
    /// This results in curious behavior by security products and the OS itself.
    /// </summary>
    partial class ProcessHerpaderping
    {
        /*
         * 
         * 介绍:
         * https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/ntddk/nf-ntddk-pssetcreateprocessnotifyroutineex
         * PsSetCreateProcessNotifyRoutineEx
         * 
         * https://docs.microsoft.com/en-us/windows-hardware/drivers/kernel/irp-mj-cleanup
         * IRP_MJ_CLEANUP   
         * 
        1. 通常安全产品会在进程创建时采取行动，在Windows内核 PsSetCreateProcessNotifyRoutineEx注册一个回调
           此时，安全产品会检查被映射的可执行文件，决定该进程是否值得被执行
          PsSetCreateProcessNotifyRoutineEx回调只有在初始线程被插入时才会执行，并不是在进程创建时。

        2.按照这种思路，攻击者可以手动创建和映射一个进程，更改原始的文件内容，然后创建initial thread;
         还有的产品使用写入时扫描方法，包括监控文件写入。

       
        */
    }

    partial class ProcessHerpaderping
    {
        // https://github.com/jxy-s/herpaderping/blob/main/source/ProcessHerpaderping/herpaderp.cpp
    }
}
