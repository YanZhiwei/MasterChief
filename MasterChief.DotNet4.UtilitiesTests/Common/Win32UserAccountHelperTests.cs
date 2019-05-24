using System.Linq;
using MasterChief.DotNet4.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.UtilitiesTests.Common
{
    [TestClass]
    public class Win32UserAccountHelperTests
    {
        [TestMethod]
        public void GetUsersTest()
        {
            var actual = Win32UserAccountHelper.GetUsers()?.Any() ?? false;
            Assert.IsTrue(actual);
        }
    }
}