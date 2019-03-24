using MasterChief.DotNet4.Utilities.Common;

namespace MasterChief.DotNet.Core.Config
{
    public class ConfigContext
    {
        /// <summary>
        ///     配置服务
        /// </summary>
        public readonly IConfigProvider ConfigService;

        public ConfigContext(IConfigProvider configService)
        {
            ConfigService = configService;
        }

        public ConfigContext() : this(new FileConfigService())
        {
        }

        public virtual void Save<T>(T configGroup, string index = null) where T : class, new()
        {
            var clusteredIndex = ConfigProviderHelper.CreateClusteredIndex<T>(index);
            ConfigService.SaveConfig(clusteredIndex, SerializeHelper.XmlSerialize(configGroup));
        }

        public virtual T Get<T>(string index = null) where T : class, new()
        {
            var clusteredIndex = ConfigProviderHelper.CreateClusteredIndex<T>(index);
            var context = ConfigService.GetConfig(clusteredIndex);

            return string.IsNullOrEmpty(context) ? null : SerializeHelper.XmlDeserialize<T>(context.Trim());
        }

        public virtual string GetClusteredIndex<T>(string index = null) where T : class, new()
        {
            return ConfigService.GetClusteredIndex<T>(index);
        }
    }
}