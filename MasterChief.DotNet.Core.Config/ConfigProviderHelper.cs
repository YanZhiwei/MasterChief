namespace MasterChief.DotNet.Core.Config
{
    public static class ConfigProviderHelper
    {
        public static string CreateClusteredIndex<T>(string index) where T : class, new()
        {
            var fileName = typeof(T).Name;
            return string.IsNullOrEmpty(index) ? fileName : $"{fileName}_{index}";
        }
    }
}