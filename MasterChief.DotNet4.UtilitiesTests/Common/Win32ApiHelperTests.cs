using System.Linq;
using MasterChief.DotNet4.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.UtilitiesTests.Common
{
    [TestClass]
    public class Win32ApiHelperTests
    {
        [TestMethod]
        public void CreateProcessTest()
        {
            var path = @"C:\Windows\System32\notepad.exe";
            Win32ApiHelper.CreateProcess(path);
        }

        [TestMethod]
        public void GetWindowsTest()
        {
            var actual = Win32ApiHelper.GetWindows()?.Any() ?? false;
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void GetWindowStationNamesTest()
        {
            var actual = Win32ApiHelper.GetWindowStationNames()?.Any() ?? false;
            Assert.IsTrue(actual);
        }
    }
}