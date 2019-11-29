using System.Reflection;
using MasterChief.DotNet4._5.Utilities.Core;
using MasterChief.DotNet4._5.UtilitiesTests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4._5.UtilitiesTests.Core
{
    [TestClass]
    public class LocalizedResourceTests
    {
        private LocalizedResource _localizedResource;

        [TestInitialize]
        public void Init()
        {
            _localizedResource = new XfLocalizedResource();
        }

        [TestMethod]
        public void GetStringTest()
        {
            var actual = _localizedResource.GetString(XfResource.Name);

            Assert.AreEqual("姓名", actual);
        }
    }

    public sealed class XfLocalizedResource : LocalizedResource
    {
        public XfLocalizedResource() : base(XfResource.ResourceManager.BaseName, Assembly.GetCallingAssembly())
        {
        }
    }
}