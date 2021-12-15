# ProcessInjecting
Process Injection Collection via C#

目前MEF解析引擎是在ProcessInjecting内部InitMEF nested status，后面会考虑重构。
ProcessInjectionDemo更多算是一个临时区域，用以熟悉这些注入技术的实现以及原理；然后由插件生产者产生标准插件供主框架使用。
我简单画了个图，便于理清楚整个逻辑。(字太丑，勿怪)

![](./images/processinjecting.jpg)
