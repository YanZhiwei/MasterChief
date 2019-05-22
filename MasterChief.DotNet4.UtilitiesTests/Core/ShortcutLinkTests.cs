using MasterChief.DotNet4.Utilities.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.UtilitiesTests.Core
{
    [TestClass()]
    public class ShortcutLinkTests
    {
        [TestMethod()]
        public void CreatDesktopTest()
        {
            string path = "D:\\ceshi.bat";
            ShortcutLink.CreatCurUserDesktop("测试", path, "单元测试");
            Assert.IsTrue(true);
        }
    }
}