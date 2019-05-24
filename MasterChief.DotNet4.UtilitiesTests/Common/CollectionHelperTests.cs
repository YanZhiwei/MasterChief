using System.Collections.Generic;
using MasterChief.DotNet4.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.UtilitiesTests.Common
{
    [TestClass()]
    public class CollectionHelperTests
    {
        [TestMethod()]
        public void IsNullOrEmptyTest()
        {
            List<int> data1 = new List<int> { };
            Assert.IsTrue(data1.IsNullOrEmpty());

            List<int> data2 = new List<int> { 1, 2, 3 };
            Assert.IsFalse(data2.IsNullOrEmpty());

            string[] data3 = { "123", "456" };
            Assert.IsFalse(data3.IsNullOrEmpty());
        }
    }
}