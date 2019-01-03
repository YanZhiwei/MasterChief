using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.Utilities.Common.Tests
{
    [TestClass()]
    public class CheckHelperTests
    {
        [TestMethod()]
        public void IsFilePathTest()
        {
            string path = @"D:\OneDrive\软件\工具\calibre-3.33.1.msi";
            bool actual = CheckHelper.IsFilePath(path);
            Assert.IsTrue(actual);
        }
    }
}