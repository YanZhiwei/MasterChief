using MasterChief.DotNet4.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.UtilitiesTests.Common
{
    [TestClass()]
    public class ArrayHelperTests
    {
        [TestMethod()]
        public void AddTest()
        {
            string[] data = { "a", "b", "c" };
            string[] actual = data.Add("d");
            string[] expect = { "a", "b", "c", "d" };
            CollectionAssert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void AddRangeTest()
        {
            string[] data = { "a", "b", "c" };
            string[] actual = data.AddRange(new string[] { "d", "e" });
            string[] expect = { "a", "b", "c", "d", "e" };
            CollectionAssert.AreEqual(expect, actual);
        }
    }
}