using MasterChief.DotNet4.Utilities.DesignPattern;

namespace MasterChief.DotNet.Json.Utilities
{
    /// <summary>
    /// Json 序列化与反序列化辅助类
    /// </summary>
    public class JsonHelper
    {
        #region Fields

        /// <summary>
        /// Json 序列化与反序列化提供者
        /// </summary>
        public readonly IJsonProvider JsonProvider = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jsonProvider">IJsonProvider</param>
        public JsonHelper(IJsonProvider jsonProvider)
        {
            JsonProvider = jsonProvider;
        }

        /// <summary>
        /// 默认构造函数，使用Newtonsoft处理Json序列化与反序列化
        /// </summary>
        public JsonHelper()
            : this(new NewtonsoftJsonProvider())
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 单例
        /// </summary>
        public static JsonHelper Instance => Singleton<JsonHelper>.CreateInstance();

        #endregion Properties

        #region Methods

        /// <summary>
        /// 反序列化Json数据格式
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="jsonText">Json文本</param>
        /// <returns>反序列化</returns>
        public virtual T Deserialize<T>(string jsonText)
        {
            return JsonProvider.Deserialize<T>(jsonText);
        }

        /// <summary>
        /// 序列化数据为Json数据格式
        /// </summary>
        /// <param name="serializeObject">需要序列化对象</param>
        /// <returns>Jsonz字符串</returns>
        public virtual string Serialize(object serializeObject)
        {
            return JsonProvider.Serialize(serializeObject);
        }

        #endregion Methods
    }
}