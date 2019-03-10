using MasterChief.DotNet.Core.ConfigTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System.Collections.Generic;

namespace MasterChief.DotNet.Core.Config.Tests
{
    [TestClass()]
    public class FileConfigServiceTests
    {
        private IKernel _kernel = null;
        private IConfigProvider _configProvider = null;
        public ConfigContext _configContext = null;

        [TestInitialize]
        public void SetUp()
        {
            _kernel = new StandardKernel(new ConfigModule());
            Assert.IsNotNull(_kernel);

            _configProvider = _kernel.Get<IConfigProvider>();
            _configContext = _kernel.Get<ConfigContext>();
        }

        [TestMethod()]
        public void SaveConfigTest()
        {
            RedisConfig redisConfig = new RedisConfig
            {
                AutoStart = true,
                LocalCacheTime = 10,
                MaxReadPoolSize = 1024,
                MaxWritePoolSize = 1024,
                ReadServerList = "10",
                RecordeLog = true,
                WriteServerList = "10"
            };
            redisConfig.RedisItems = new List<RedisItemConfig>
            {
                new RedisItemConfig() { Text = "MasterChief" },
                new RedisItemConfig() { Text = "Config." }
            };

            _configContext.Save(redisConfig, "prod");
            _configContext.Save(redisConfig, "alpha");

            RedisConfig prodRedisConfig = _configContext.Get<RedisConfig>("prod");
            Assert.IsNotNull(prodRedisConfig);

            prodRedisConfig = _configContext.Get<RedisConfig>("prod");//文件缓存测试
            Assert.IsNotNull(prodRedisConfig);

            RedisConfig alphaRedisConfig = _configContext.Get<RedisConfig>("alpha");
            Assert.IsNotNull(alphaRedisConfig);

            DaoConfig daoConfig = new DaoConfig
            {
                Log = "server=localhost;database=Sample;uid=sa;pwd=sasa"
            };
            _configContext.Save(daoConfig, "prod");
            _configContext.Save(daoConfig, "alpha");
            DaoConfig prodDaoConfig = _configContext.Get<DaoConfig>("prod");
            Assert.IsNotNull(prodDaoConfig);

            DaoConfig alphaDaoConfig = _configContext.Get<DaoConfig>("alpha");
            Assert.IsNotNull(alphaDaoConfig);
        }
    }
}