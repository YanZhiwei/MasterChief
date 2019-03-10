using MasterChief.DotNet.Core.CacheTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace MasterChief.DotNet.Core.Cache.Tests
{
    [TestClass()]
    public class LocalCacheProviderTests
    {
        private IKernel _kernel = null;
        private ICacheProvider _cacheProvider = null;
        private readonly string _testCacheKey = "sampleKey";
        private readonly string _testCache = "sample";
        private readonly string _testKeyFormat = "login_{0}";

        [TestInitialize]
        public void SetUp()
        {
            _kernel = new StandardKernel(new CacheModule());
            Assert.IsNotNull(_kernel);

            _cacheProvider = _kernel.Get<ICacheProvider>();
            _cacheProvider.Set(_testCacheKey, _testCache, 10);
        }

        [TestMethod()]
        public void GetTest()
        {
            string actual = _cacheProvider.Get<string>(_testCacheKey);
            Assert.AreEqual(_testCache, actual);
        }

        [TestMethod()]
        public void IsSetTest()
        {
            bool actual = _cacheProvider.IsSet(_testCacheKey);
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            _cacheProvider.Remove(_testCacheKey);
            bool actual = _cacheProvider.IsSet(_testCacheKey);
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void RemoveByPatternTest()
        {
            string _loginKey = string.Format(_testKeyFormat, "123");
            _cacheProvider.Set(_loginKey, _testCache, 10);
            bool actual = _cacheProvider.IsSet(_loginKey);
            Assert.IsTrue(actual);
            _cacheProvider.RemoveByPattern(_testKeyFormat);
            actual = _cacheProvider.IsSet(_loginKey);
            Assert.IsFalse(actual);
            actual = _cacheProvider.IsSet(_testCacheKey);
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void SetTest()
        {
            _cacheProvider.Set("sampleSetKey", "sampleSetCache", 10);
            bool actual = _cacheProvider.IsSet("sampleSetKey");
            Assert.IsTrue(actual);
        }
    }
}