namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// TimeSpan帮助类
    /// </summary>
    /// 创建时间:2015-06-30 15:39
    /// 备注说明:<c>null</c>
    public static class TimeSpanHelper
    {
        #region Methods

        /// <summary>
        /// 添加小时
        /// </summary>
        /// <param name="ts">TimeSpan</param>
        /// <param name="hours">小时</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan AddHours(this TimeSpan ts, int hours)
        {
            return ts.Add(new TimeSpan(hours, 0, 0));
        }

        /// <summary>
        /// 添加分钟
        /// </summary>
        /// <param name="ts">TimeSpan</param>
        /// <param name="minutes">分钟</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan AddMinutes(this TimeSpan ts, int minutes)
        {
            return ts.Add(new TimeSpan(0, minutes, 0));
        }

        /// <summary>
        /// 添加秒
        /// </summary>
        /// <param name="ts">TimeSpan</param>
        /// <param name="seconds">秒</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan AddSeconds(this TimeSpan ts, int seconds)
        {
            return ts.Add(new TimeSpan(0, 0, seconds));
        }

        /// <summary>
        /// 格式化时间
        /// <para>eg: Assert.AreEqual("14:01", TimeSpanHelper.FormatTime(new TimeSpan(14, 1, 2)));</para>
        /// </summary>
        /// <param name="date">The timefield.</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatTime(this TimeSpan date)
        {
            if (date.Hours == 0 && date.Minutes == 0)
            {
                return "24:00";
            }
            else
            {
                return date.Hours.ToString().PadLeft(2, '0') + ":" + date.Minutes.ToString().PadLeft(2, '0');
            }
        }

        /// <summary>
        /// 将时间字符串转换成TimeSpan
        /// <para>eg: Assert.AreEqual(new TimeSpan(12, 12, 0), TimeSpanHelper.ParseTimeString("12:12:00"));</para>
        /// </summary>
        /// <param name="timeString">时间字符串</param>
        /// <returns>TimeSpan</returns>
        /// 创建时间:2015-06-30 15:39
        /// 备注说明:<c>null</c>
        public static TimeSpan ParseTimeString(this string timeString)
        {
            TimeSpan.TryParse(timeString, out TimeSpan _timeSpan);
            return _timeSpan;
        }

        /// <summary>
        /// 将日期设置为今天
        /// </summary>
        /// <param name="ts">TimeSpan</param>
        /// <returns>TimeSpan</returns>
        /// 创建时间:2015-06-30 15:43
        /// 备注说明:<c>null</c>
        public static TimeSpan SetToday(this TimeSpan ts)
        {
            DateTime now = DateTime.Now;
            return new TimeSpan(now.Day, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        }

        #endregion Methods
    }
}