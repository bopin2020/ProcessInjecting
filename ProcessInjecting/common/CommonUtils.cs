// Author: bopin (@bopin2020)
// Project: ProcessInjecting (https://github.com/bopin2020/ProcessInjecting)
// License: GNU GPLv3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessInjecting.common
{
    public sealed class CommonUtils
    {
        public sealed class ConsoleSide
        {
            /// <summary>
            /// 刷新当前行的值
            /// 使用Console SetCursorPosition 设置光标位置
            /// </summary>
            public void RefreshCurrentLineValues()
            {
                var l = Console.CursorLeft;
                var t = Console.CursorTop;
                for (int i = 0; i < 100; ++i)
                {
                    Console.SetCursorPosition(l, t);
                    // Thread.Sleep(50);
                    Console.Write($"{  i  }% ");
                }
            }

            #region Static Functions

            /// <summary>
            /// Output error message
            /// </summary>
            /// <param name="message"></param>
            public static void PrintError(params string[] message)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                foreach (var item in message)
                {
                    Console.WriteLine(item);
                }
                Console.ResetColor();
            }
            /// <summary>
            /// Output basical information
            /// </summary>
            /// <param name="message"></param>
            public static void PrintInfo(params string[] message)
            {
                Console.WriteLine(message);
            }
            /// <summary>
            /// Output Debug message
            /// </summary>
            /// <param name="message"></param>
            public static void PrintDebug(params string[] message)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            /// <summary>
            /// Output warning message
            /// </summary>
            /// <param name="message"></param>
            public static void PrintWarn(params string[] message)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                Console.ResetColor();
            }

            public static void print_good(String var0)
            {
                Console.WriteLine("\u001b[01;32m[+] \u001b[0m " + var0);
            }

            public static void print_opsec(String var0)
            {
                Console.WriteLine("\u001b[00;33m[%] \u001b[0m " + var0);
            }

            public static void print_info(String var0)
            {
                Console.WriteLine("\u001b[01;34m[*] \u001b[0m " + var0);
            }

            public static void print_warn(String var0)
            {
                Console.WriteLine("\u001b[01;33m[!] \u001b[0m " + var0);
            }


            /// <summary>
            /// How Write text with Underline or Overline
            /// Console class is limited
            /// https://docs.microsoft.com/en-us/windows/console/setconsolecursorinfo?redirectedfrom=MSDN
            /// 
            /// Windows 10 build 16257 and later
            /// https://stackoverflow.com/questions/5237666/adding-text-decorations-to-console-output
            /// </summary>
            /// <param name="message"></param>
            public static void Underline(string message)
            {
                uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;
                var handle = GetStdHandle(-11);
                uint mode;
                GetConsoleMode(handle, out mode);
                mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
                SetConsoleMode(handle, mode);

                const string UNDERLINE = "\x1B[4m";
                const string RESET = "\x1B[0m";

                Console.WriteLine(UNDERLINE + message + RESET);

            }

            public static void print_under(string under)
            {
                Console.Write("\x1B[4m" + under + "\x1B[0m");
            }

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll")]
            static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

            [DllImport("kernel32.dll")]
            static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
            /// <summary>
            /// 前景输出高亮
            /// </summary>
            /// <param name="message"></param>
            public static void ForeHighlight(string message)
            {
                Console.WriteLine("\x1B[1m" + message);
            }
            #endregion
        }
    }
}
