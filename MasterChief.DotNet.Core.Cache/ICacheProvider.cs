namespace MasterChief.DotNet.Core.Cache
{
    /// <summary>
    /// 缓存提供者接口
    /// </summary>
    public interface ICacheProvider
    {
        T Get<T>(string key);
        void Set(string key, object data, int cacheTime);
        void Set(string key, object data, string dependFile);
        bool IsSet(string key);
        void Remove(string key);
        void RemoveByPattern(string pattern);
    }
}
