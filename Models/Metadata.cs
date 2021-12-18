namespace Models
{
    public struct Metadata
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] References { get; set; }
        public string[] Principles { get; set; }
        public string Opinion { get; set; }
    }

    /// <summary>
    /// https://www.elastic.co/blog/ten-process-injection-techniques-technical-survey-common-and-trending-process
    /// ten process injection techniques
    /// https://www.ired.team/offensive-security/code-injection-process-injection
    /// ired process injection collection
    /// </summary>
    public enum ProcessKind
    { 
        ProcessHollowing,
        ProcessDoppelganging,
        ProcessHerpaderping,
        ProcessGhosting,
        TransactedHollowing,
        Classic,
        ModuleStomping,
        PEInjection,
        ThreadHijack,
        HookInjection,
        APCAtombombing,
        APCQueue,
        APCEarlyBird,
        IATHooking,
        InlineHooking,
        ReflectiveDllInjection,
    }
}
