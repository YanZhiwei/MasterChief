using Newtonsoft.Json.Converters;

namespace MasterChief.DotNet.Infrastructure.JsonSerializer
{
    /// <summary>
    ///     序列化时候自定义时间格式
    ///     <para>字段属性增加： [JsonConverter(typeof(CustomDateFormat), "yyy-MM-dd HH:mm:ss")]</para>
    /// </summary>
    /// 时间：2016/7/20 10:49
    /// 备注：
    public class CustomDateFormat : IsoDateTimeConverter
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="format">格式化字符串</param>
        /// 时间：2016/7/20 10:50
        /// 备注：
        public CustomDateFormat(string format)
        {
            DateTimeFormat = format;
        }
    }
}