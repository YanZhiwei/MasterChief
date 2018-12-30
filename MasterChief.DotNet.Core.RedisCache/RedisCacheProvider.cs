using MasterChief.DotNet.Core.Cache;

namespace MasterChief.DotNet.Core.RedisCache
{
    public sealed class RedisCacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSet(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveByPattern(string pattern)
        {
            throw new System.NotImplementedException();
        }

        public void Set(string key, object data, int cacheTime)
        {
            throw new System.NotImplementedException();
        }

        public void Set(string key, object data, string dependFile)
        {
            throw new System.NotImplementedException();
        }
    }
}
