namespace MasterChief.DotNet.Core.Config
{
    /// <summary>
    /// 配置服务接口
    /// </summary>
    public interface IConfigProvider
    {
        #region Methods

        /// <summary>
        /// 根据名称获取配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns>配置内容</returns>
        string GetConfig(string name);

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <param name="content">配置内容</param>
        void SaveConfig(string name, string content);

        /// <summary>
        /// 获取配置索引
        /// </summary>
        /// <param name="index">索引名称</param>
        /// <returns>索引</returns>
        string GetClusteredIndex<T>(string index = null) where T : class, new();

        #endregion Methods
    }
}