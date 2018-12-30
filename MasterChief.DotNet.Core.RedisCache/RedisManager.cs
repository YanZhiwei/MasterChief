using ServiceStack.Caching;
using ServiceStack.Redis;

namespace MasterChief.DotNet.Core.RedisCache
{
    /// <summary>
    /// RedisManager
    /// </summary>
    public sealed class RedisManager
    {
        #region Fields

        private static PooledRedisClientManager _prcm;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Redis配置文件信息
        /// </summary>
        private static RedisConfig redisConfigInfo => CachedConfigContext.Instance.RedisConfig;

        #endregion Properties

        #region Methods

        /// <summary>
        /// 获取缓存Client
        /// </summary>
        public static ICacheClient GetCacheClient()
        {
            if (_prcm == null)
            {
                CreateManager();
            }

            return _prcm.GetCacheClient();
        }

        /// <summary>
        /// 获取可写入的Client
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (_prcm == null)
            {
                CreateManager();
            }

            return _prcm.GetClient();
        }

        /// <summary>
        /// 获取只读的Client
        /// </summary>
        public static IRedisClient GetReadOnlyClient()
        {
            if (_prcm == null)
            {
                CreateManager();
            }

            return _prcm.GetReadOnlyClient();
        }

        /// <summary>
        /// 创建PooledRedisClientManager
        /// </summary>
        private static void CreateManager()
        {
            string[] writeServerList = redisConfigInfo.WriteServerList.Split(',');
            string[] readServerList = redisConfigInfo.ReadServerList.Split(',');
            _prcm = new PooledRedisClientManager(readServerList, writeServerList, new RedisClientManagerConfig
            {
                MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                AutoStart = redisConfigInfo.AutoStart,
            });
        }

        #endregion Methods
    }
}