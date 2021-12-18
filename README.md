# ProcessInjecting
Process Injection Collection via C#

> 关于注入技术使用C#实现的集合，未来希望能更全面一点，然后就是Evasion等手段的使用 () => DInvoke,SYSCALL等
> 命令行参数Example: 
> -p ProcessHollowing -d Demo -y Profile.yaml

> 目前MEF解析引擎是在ProcessInjecting内部InitMEF nested status，后面会考虑重构。
> ProcessInjectionDemo更多算是一个临时区域，用以熟悉这些注入技术的实现以及原理；然后由插件生产者产生标准插件供主框架使用。
> 我简单画了个图，便于理清楚整个逻辑。(字太丑，勿怪)

<img src="./images/processinjecting.jpg" style="zoom:50%;" />


## Module Stomping

```
======解析profile======>
userwx:True
syscall:False
syscall:False
hellgates:False
patchAmsi:False
patchEtw:False
Blockdlls:False
XOR:15
sleep_time:5000
spwanto:c:\\windows\\system32\\rundll32.exe
NtReadVirtualMemory
NtAllocateVirtualMemory
NtWriteVirtualMemory

Name:           Module Stomping
Description:    Module Stomping (which also seems to by the names Module Overloading and Dll Hollowing)is a shellcode injection technique
References:
                https://github.com/countercept/ModuleStomping
                https://blog.f-secure.com/hiding-malicious-code-with-module-stomping/
                https://offensivedefence.co.uk/posts/module-stomping/
                https://www.ired.team/offensive-security/code-injection-process-injection/modulestomping-dll-hollowing-shellcode-injection
Principles:
                1. Create a process or open a handle to an existing process
                2. Fore that process to load a legitimate DLL from disk
                3. Write the shellcode somewhere into the DLL
                4. Kick off execution using CreateRemoteThread or other (eg.UserQueueAPC also works)
Invoke Module Stomping

```
