namespace MasterChief.DotNet.Core.RedisCache
{
    using ServiceStack.Caching;
    using ServiceStack.Redis;

    /// <summary>
    /// RedisManager
    /// </summary>
    public sealed class RedisManager
    {
        #region Fields

        /// <summary>
        /// Redis配置文件信息
        /// </summary>
        private readonly RedisConfig _redisConfig;

        private PooledRedisClientManager _redisClientManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 静态构造函数
        /// </summary>
        public RedisManager(RedisConfig config)
        {
            _redisConfig = config;
            CreateManager();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取缓存Client
        /// </summary>
        public ICacheClient GetCacheClient()
        {
            if (_redisClientManager == null)
            {
                CreateManager();
            }

            return _redisClientManager.GetCacheClient();
        }

        /// <summary>
        /// 获取可写入的Client
        /// </summary>
        public IRedisClient GetClient()
        {
            if (_redisClientManager == null)
            {
                CreateManager();
            }

            return _redisClientManager.GetClient();
        }

        /// <summary>
        /// 获取只读的Client
        /// </summary>
        public IRedisClient GetReadOnlyClient()
        {
            if (_redisClientManager == null)
            {
                CreateManager();
            }

            return _redisClientManager.GetReadOnlyClient();
        }

        /// <summary>
        /// 创建PooledRedisClientManager
        /// </summary>
        private void CreateManager()
        {
            string[] writeServerList = _redisConfig.WriteServerList.Split(',');
            string[] readServerList = _redisConfig.ReadServerList.Split(',');
            _redisClientManager = new PooledRedisClientManager(readServerList, writeServerList, new RedisClientManagerConfig
            {
                MaxWritePoolSize = _redisConfig.MaxWritePoolSize,
                MaxReadPoolSize = _redisConfig.MaxReadPoolSize,
                AutoStart = _redisConfig.AutoStart,
            });
        }

        #endregion Methods
    }
}