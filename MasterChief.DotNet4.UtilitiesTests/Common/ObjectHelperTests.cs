using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace MasterChief.DotNet4.Utilities.Common.Tests
{
    [TestClass()]
    public class ObjectHelperTests
    {
        [TestMethod()]
        public void IsCollectionTest()
        {
            object data1 = null;
            Assert.IsFalse(data1.IsCollection());
            int data2 = 123;
            Assert.IsFalse(data2.IsCollection());
            string[] data3 = { "1", "2", "3" };
            Assert.IsTrue(data3.IsCollection());
            List<int> data4 = new List<int>() { 1, 3, 4 };
            Assert.IsTrue(data4.IsCollection());

            ArrayList data5 = new ArrayList();
            Assert.IsTrue(data5.IsCollection());

            HashSet<int> data6 = new HashSet<int>();
            Assert.IsTrue(data6.IsCollection());
        }
    }
}