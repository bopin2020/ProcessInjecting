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
using static Vanara.PInvoke.User32;

namespace ProcessInjectionDemo
{
    /// <summary>
    /// https://www.ired.team/offensive-security/code-injection-process-injection/setwindowhookex-code-injection
    /// </summary>
    partial class _SetWindowHookEx
    {
        /*
        SetWindowsHookEx
        Installs an application-defined hook procedure into a hook chain.
        Install a hook procedure to monitor the system for certain types of events
        在和当前调用线程在相同的桌面环境下，这些事件要么是和指定线程或所有线程相关的

        第一个参数表示安装 Hook的类型:
        https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa

        WH_CALLWNDPROC    在系统将消息发送到目标windows程序前
        WH_KEYBOARD_LL    监控低级键盘输入时间


        Kernel mode diving into
        通常该API是键盘记录所使用的,因此就有杀软/EDR会hook usermode APIs in kernelmode.
        https://forums.codeguru.com/showthread.php?377365-SetWindowsHookEx-in-kernel-mode
        hook KeServiceDescriptorTable


        NtUserSetWindowsHookE
        */
    }

    partial class _SetWindowHookEx
    {
        public static void Run()
        {
            var p = LoadLibrary(@"c:\users\James\Desktop\StandardDll.dll");
            var h = GetProcAddress(p, "test");

            // https://github.com/Bert-Proesmans/SC-Buddy/blob/9a491d914bb187fd3dfc2622b16c9b0619aeaa94/SC-Buddy/MouseHook.cs
            // MouseHook
            // https://github.com/Bert-Proesmans/SC-Buddy/


            HookProc hp = Marshal.GetDelegateForFunctionPointer<HookProc>(h);

            SetWindowsHookEx(HookType.WH_KEYBOARD,hp,p,0);
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return IntPtr.Zero;
        }
    }
}
