// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.IO;
using System.Text;
using Vanara.PInvoke;
using static Vanara.PInvoke.Kernel32;

namespace ProcessInjectionDemo
{
    /// <summary>
    /// https://buaq.net/go-81150.html
    /// https://bestofcpp.com/repo/hasherezade-process_ghosting
    /// https://www.elastic.co/blog/process-ghosting-a-new-executable-image-tampering-attack
    /// https://github.com/hasherezade/process_ghosting
    /// https://github.com/knightswd/ProcessGhosting     
    /// 在hasherezade基础上对关键函数进行了systemcall，堆内存操作api进行了unhook
    /// syscall 使用asm汇编语言编写
    /// </summary>
    partial class ProcessGhosting
    {
        #region 基础知识

        /*
        *
        unhook操作参考: https://github.com/knightswd/ProcessGhosting/blob/main/ProcessGhost/ProcessGhost.cpp#L17
        我们实现这部分通过配置 PE Profile实现

        1. Windows如何启动一个进程
           启动可执行文件的句柄 CreateFile();
           创建一个Section 将exe映射到这块内存中  NtCreateSection()
           使用该section创建一个进程  NtCreateProcessEx()
           分配参数和环境变量  CreateEnvironmentBlock / NtWriteVirtualMemory
           创建线程并执行 NtCreateThreadEx

        2. Windows删除文件方法
          FILE_SUPERSEDE   CREATE_ALWAYS标志  旧文件上创建新的文件
          创建或打开文件时设置 FILE_DELETE_ON_CLOSE FILE_FLAG_DELETE_ON_CLOSE 
          通过NtSetInformationFile 调用FileDispositionInformation  将FILE_DISPOSITION_INFORMATION结构中的DeleteFile字段设置为TRUE

        3.如何判断一个进程的启动
        驱动开发者通过在 PsSetCreateProcessNotifyRoutineEx PsSetCreateThreadNotifyRoutineEx等API监控上层程序创建进程或线程的状态
        PsSetCreateProcessNotifyRoutineEx回调函数并不是在进程创建时调用的，而是在创建这些进程的第一个线程调用的
        当把exe映射到section中 没有线程，这时不会认为是exe的启动状态
        */

        #endregion

        #region ProcessGhosting实现原理


        #endregion
    }

    partial class ProcessGhosting
    {
        // black 待执行的malicious payload
        // white retrieve process environment variables which will not be opened
        // temp  set flag = delete 
        public static IntPtr buffer_payload(string filename, out int size)
        {
            size = 10;
            SafeHFILE handle = CreateFile(
                                filename,
                                Kernel32.FileAccess.GENERIC_READ,
                                FileShare.Read,
                                null,
                                FileMode.Open,
                                FileFlagsAndAttributes.FILE_ATTRIBUTE_NORMAL,
                                HFILE.NULL);
            if (handle != HFILE.INVALID_HANDLE_VALUE)
            {
                SafeHSECTION section = CreateFileMapping(
                    handle,
                    null,
                    MEM_PROTECTION.PAGE_READONLY, // 创建读写Map导致出错
                    0,
                    0,
                    "Anonymous");
                if (section != HSECTION.NULL)
                {
                    IntPtr mapAddress = MapViewOfFile(
                        section,
                        FILE_MAP.FILE_MAP_READ,
                        0,
                        0,
                        0);
                    if (mapAddress != IntPtr.Zero)
                    {
                        uint lpSizeHigh;
                        uint fileSize = GetFileSize(handle, out lpSizeHigh);

                        IntPtr address = VirtualAlloc(
                            IntPtr.Zero,
                            fileSize,
                            MEM_ALLOCATION_TYPE.MEM_COMMIT | MEM_ALLOCATION_TYPE.MEM_RESERVE,
                            MEM_PROTECTION.PAGE_READWRITE);
                        // C# 如何将非托管数据从第一个地址拷贝到另一个地址
                        // Marshal.Write() 操作单个字节
                        RtlCopyMemory(address, mapAddress, fileSize);

                        if (UnmapViewOfFile(mapAddress))
                        {
                            CloseHandle(handle.DangerousGetHandle());
                            CloseHandle(section.DangerousGetHandle());
                        }
                        return address;
                    }
                }
            }

            return IntPtr.Zero;
        }


        /// <summary>
        /// 进行启动进程的操作
        /// </summary>
        /// <returns></returns>
        public static bool Process_Ghost(string targetPath,IntPtr payload,long payloadSize)
        {
            StringBuilder sb = new StringBuilder();
            uint status = GetTempPath(256, sb);
            status = GetTempFileName("", "", 0, sb);
            return true;
        }

        public static void make_section_from_delete_pending_file(string filename, IntPtr payload, long payloadSize)
        {
            OFSTRUCT of = new OFSTRUCT();


            SafeHFILE file = OpenFile(
                filename,
                ref of,
                OpenFileAction.OF_READ);
            if (file != IntPtr.Zero)
            {
                FILE_DISPOSITION_INFO info = new FILE_DISPOSITION_INFO();
                info.DeleteFile = true; // 设置文件删除状态为true

                // https://feed.nuget.org/packages/Vanara.PInvoke.NtDll/3.3.12
                // NtSetInformationFile
                
            }

        }
    }
}
