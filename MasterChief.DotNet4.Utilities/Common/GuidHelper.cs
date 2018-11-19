namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// GUID 帮助类
    /// </summary>
    public static class GuidHelper
    {
        #region Methods

        /// <summary>
        /// 返回Guid用于数据库操作，特定的时间代码可以提高检索效率
        /// combined guid/timestamp，意思是：组合GUID/时间截
        /// </summary>
        /// <returns>COMB类型 Guid 数据</returns>
        public static Guid CreateCOMB()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime initDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            long nowTicks = (new DateTime(now.Year, now.Month, now.Day)).Ticks;
            TimeSpan days = new TimeSpan(now.Ticks - initDate.Ticks),
            msecs = new TimeSpan(now.Ticks - nowTicks);
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }

        /// <summary>
        /// 格式化Guid
        /// <para>0==>xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx</para>
        /// <para>1==>xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx</para>
        /// <para>2==>{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}</para>
        /// <para>3==>(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) </para>
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <param name="guidMode">格式类型</param>
        /// <returns></returns>
        /// 时间：2016-01-08 14:40
        /// 备注：
        public static string FormatGuid(this Guid guid, int guidMode)
        {
            string formatString = string.Empty;

            switch (guidMode)
            {
                case 0:
                    formatString = guid.ToString("N");//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                    break;

                case 1:
                    formatString = guid.ToString("D");//xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
                    break;

                case 2:
                    formatString = guid.ToString("B");//{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
                    break;

                case 3:
                    formatString = guid.ToString("P");//(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
                    break;

                default:
                    formatString = guid.ToString();
                    break;
            }

            return formatString;
        }

        /// <summary>
        /// 从SQL Server 返回的Guid中生成时间信息
        /// </summary>
        /// <param name="combGuid">The SQL server unique identifier.</param>
        /// <returns>DateTime</returns>
        /// 时间：2015-09-15 13:28
        /// 备注：
        public static DateTime ParseCOMBGuid(Guid combGuid)
        {
            DateTime guidTime = new DateTime(1900, 1, 1);
            byte[] daysArray = new byte[4];
            byte[] msecsArray = new byte[4];
            byte[] guidArray = combGuid.ToByteArray();
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);
            int days = BitConverter.ToInt32(daysArray, 0);
            int msecs = BitConverter.ToInt32(msecsArray, 0);
            DateTime date = guidTime.AddDays(days);
            date = date.AddMilliseconds(msecs * 3.333333);
            return date;
        }

        /// <summary>
        /// 将Guid转换为Base64String
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>Base64字符串</returns>
        /// 时间：2016/11/8 14:54
        /// 备注：
        public static string ToBase64String(Guid guid)
        {
            byte[] guidArray = guid.ToByteArray();
            return Convert.ToBase64String(guidArray);
        }

        /// <summary>
        /// 将GUID转换成符合SQL Server的GUID
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns>符合SQL Server的GUID</returns>
        public static Guid ToCOMBGuid(this Guid guid)
        {
            byte[] guidArray = guid.ToByteArray();
            Array.Reverse(guidArray, 0, 4);
            Array.Reverse(guidArray, 4, 2);
            Array.Reverse(guidArray, 6, 2);
            return new Guid(guidArray);
        }

        /// <summary>
        /// 将Guid转换为Long类型
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// 时间：2016/11/8 14:36
        /// 备注：
        public static long ToLongId(Guid value)
        {
            byte[] guidArray = value.ToByteArray();
            return BitConverter.ToInt64(guidArray, 0);
        }

        #endregion Methods
    }
}