namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// Unix 时间戳 帮助类
    /// </summary>
    public class UnixEpochHelper
    {
        #region Fields

        /*
         *参考：
         *1. http://stackoverflow.com/questions/7983441/unix-time-conversions-in-c-sharp
         *2. http://www.cnblogs.com/yc-755909659/archive/2012/12/25/2832673.html
         *
         *知识：
         *1.整个地球分为二十四时区，每个时区都有自己的本地时间。在国际无线电通信场合，为了统一起见，使用一个统一的时间，称为通用协调时(UTC, Universal Time Coordinated)。UTC与格林尼治平均时(GMT, Greenwich Mean Time)一样，都与英国伦敦的本地时相同。
         *UTC与GMT含义完全相同。
         *北京时区是东八区，领先UTC八个小时，在电子邮件信头的Date域记为+0800
         *
         */

        /// <summary>
        /// Unix 时间戳
        /// </summary>
        public static readonly DateTime UnixEpochUtcValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="UnixEpochHelper"/> class.
        /// </summary>
        static UnixEpochHelper()
        {
            UnixEpochUtcValue = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 将Unix时间戳转换成DateTime
        /// </summary>
        /// <param name="millis">毫秒</param>
        /// <returns>DateTime</returns>
        public static DateTime DateTimeFromUnixTimestampMillis(double millis)
        {
            return UnixEpochUtcValue.AddMilliseconds(millis);
        }

        /// <summary>
        /// 获取当前时间Unix时间戳
        /// </summary>
        /// <returns>Unix时间戳</returns>
        public static TimeSpan GetCurrentUnixTimestamp()
        {
            return (DateTime.UtcNow - UnixEpochUtcValue);
        }

        #endregion Methods
    }
}