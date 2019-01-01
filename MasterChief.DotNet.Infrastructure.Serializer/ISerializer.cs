namespace MasterChief.DotNet.Infrastructure.Serializer
{
    /// <summary>
    /// 序列化与反序列化接口
    /// </summary>
    public interface ISerializer
    {
        #region Methods

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="data">需要反序列化字符串</param>
        /// <returns>反序列化</returns>
        T Deserialize<T>(string data);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Jsonz字符串</returns>
        string Serialize(object serializeObject);

        #endregion Methods
    }
}