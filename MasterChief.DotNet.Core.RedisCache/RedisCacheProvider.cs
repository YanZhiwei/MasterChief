namespace MasterChief.DotNet.Core.RedisCache
{
    using MasterChief.DotNet.Core.Cache;
    using ServiceStack.Redis;
    using System;

    /// <summary>
    /// Redis缓存实现
    /// </summary>
    /// <seealso cref="MasterChief.DotNet.Core.Cache.ICacheProvider" />
    public sealed class RedisCacheProvider : ICacheProvider
    {
        #region Fields

        private readonly object syncRoot = new object();
        private readonly RedisManager _redisManager = null;
        private readonly IRedisClient _redisReadClient = null;
        private readonly IRedisClient _redisWriteClient = null;

        #endregion Fields

        #region Constructors

        private RedisCacheProvider(RedisConfig config)
        {
            _redisManager = new RedisManager(config);
            _redisReadClient = _redisManager.GetReadOnlyClient();
            _redisWriteClient = _redisManager.GetClient();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 从缓存中获取强类型数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns>
        /// 获取的强类型数据
        /// </returns>
        public T Get<T>(string key)
        {
            if (_redisReadClient.ContainsKey(key))
            {
                return _redisReadClient.Get<T>(key);
            }

            return default(T);
        }

        /// <summary>
        /// 该key是否设置过缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数值</returns>
        public bool IsSet(string key)
        {
            return _redisReadClient.ContainsKey(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            _redisWriteClient.Remove(key);
        }

        /// <summary>
        /// 根据正则表达式移除缓存
        /// </summary>
        /// <param name="pattern">移除缓存</param>
        public void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, _redisReadClient.GetAllKeys());
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="cacheTime">过期时间，单位分钟</param>
        public void Set(string key, object data, int cacheTime)
        {
            lock (syncRoot)
            {
                if (data != null)
                {
                    if (!_redisWriteClient.ContainsKey(key))
                    {
                        _redisWriteClient.Set(key, data, TimeSpan.FromMinutes(cacheTime));
                    }
                }
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">值</param>
        /// <param name="dependFile">文件依赖</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set(string key, object data, string dependFile)
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods
    }
}