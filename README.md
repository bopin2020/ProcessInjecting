# ProcessInjecting
Process Injection Collection via C#

> 关于注入技术使用C#实现的集合，未来希望能更全面一点，然后就是Evasion等手段的使用 () => DInvoke,SYSCALL等
> 命令行参数Example: 
> -p ProcessHollowing -d Demo -y Profile.yaml

> 目前MEF解析引擎是在ProcessInjecting内部InitMEF nested status，后面会考虑重构。
> ProcessInjectionDemo更多算是一个临时区域，用以熟悉这些注入技术的实现以及原理；然后由插件生产者产生标准插件供主框架使用。
> 我简单画了个图，便于理清楚整个逻辑。(字太丑，勿怪)

![](./images/processinjecting.jpg)
