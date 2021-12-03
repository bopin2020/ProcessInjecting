// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ProcessInjectionDemo
{ 
    /// <summary>
    /// https://www.cnblogs.com/ndyxb/p/12905357.html
    /// 傀儡进程
    /// 1. 以挂起方式打开目标进程 将shellcode写入目标进程，修改执行流程。
    /// </summary>
    public class ZombieProcess : ProcessBase
    {
        public ZombieProcess(String Name)
        {
            ProcessTechniqueName = Name;
        }
        /// <summary>
        /// 1.使用CreateProcess以挂起方式 CREATE_SUSPENDED创建进程
        /// 2. 调用VirtualAllocEx函数申请RWX内存
        /// 3. WriteProcessMemory将shellcode写入内存
        /// 4. GetThreadContext 设置获取标志为CONTEXT_FULL 即获取新进程中所有线程上下文
        /// 5. 修改线程上下文 EIP值为内存首地址  SetThreadContext函数设置回主线程
        /// 6. ResumeThread恢复主线程
        /// </summary>
        public override void Run()
        {

            throw new NotImplementedException();
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32.dll")]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetThreadContext(IntPtr hThread, IntPtr lpContext);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetThreadContext(IntPtr hThread, ref Context lpContext);
    }
}
