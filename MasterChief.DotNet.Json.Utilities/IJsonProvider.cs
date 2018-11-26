namespace MasterChief.DotNet.Json.Utilities
{
    /// <summary>
    /// Json 序列化与反序列化接口
    /// </summary>
    public interface IJsonProvider
    {
        #region Methods

        /// <summary>
        /// 反序列化Json数据格式
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="jsonText">Json文本</param>
        /// <returns>反序列化</returns>
        T Deserialize<T>(string jsonText);

        /// <summary>
        /// 序列化数据为Json数据格式
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Jsonz字符串</returns>
        string Serialize(object serializeObject);

        #endregion Methods
    }
}