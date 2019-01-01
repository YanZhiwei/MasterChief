using System;
using System.IO;

namespace MasterChief.DotNet.Core.Config
{
    /// <summary>
    /// 本地文件配置
    /// </summary>
    public sealed class FileConfigService : IConfigProvider
    {
        private readonly string configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");

        /// <summary>
        /// 根据名称获取配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns>
        /// 配置内容
        /// </returns>
        public string GetConfig(string name)
        {
            string configPath = GetFilePath(name);

            return !File.Exists(configPath) ? null : File.ReadAllText(configPath);
        }

        private string GetFilePath(string name)
        {
            if (!Directory.Exists(configFolder))
            {
                Directory.CreateDirectory(configFolder);
            }

            return string.Format(@"{0}\{1}.xml", configFolder, name);
        }

        /// <summary>
        /// 获取配置索引
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">索引名称</param>
        /// <returns>
        /// 索引
        /// </returns>
        public string GetClusteredIndex<T>(string index = null) where T : class, new()
        {
            return $"{Path.Combine(configFolder, ConfigProviderHelper.CreateClusteredIndex<T>(index))}.xml";
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="content">配置内容</param>
        public void SaveConfig(string name, string content)
        {
            string configPath = GetFilePath(name);
            File.WriteAllText(configPath, content);
        }
    }
}