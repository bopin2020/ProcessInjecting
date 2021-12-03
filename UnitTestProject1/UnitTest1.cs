using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessInjecting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            InitProfiles.Run(@"D:\NativeSword\PEProfiles\Profile.yaml");
        }
    }
}
