using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.Utilities.Common.Tests
{
    [TestClass]
    public class ServiceHelperTests
    {
        [TestMethod]
        public void IsExistedTest()
        {
            var actual = ServiceHelper.IsExisted("PeerDistSvc");
            Assert.IsTrue(actual);

            actual = ServiceHelper.IsExisted("PeerDistSvc2");
            Assert.IsFalse(actual);
        }
    }
}