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
            byte[] _guidArray = Guid.NewGuid().ToByteArray();
            DateTime _dtBase = new DateTime(1900, 1, 1);
            DateTime _dtNow = DateTime.Now;
            long _nowTicks = (new DateTime(_dtNow.Year, _dtNow.Month, _dtNow.Day)).Ticks;
            TimeSpan _days = new TimeSpan(_dtNow.Ticks - _dtBase.Ticks),
            _msecs = new TimeSpan(_dtNow.Ticks - _nowTicks);
            byte[] _daysArray = BitConverter.GetBytes(_days.Days);
            byte[] _msecsArray = BitConverter.GetBytes((long)(_msecs.TotalMilliseconds / 3.333333));
            Array.Reverse(_daysArray);
            Array.Reverse(_msecsArray);
            Array.Copy(_daysArray, _daysArray.Length - 2, _guidArray, _guidArray.Length - 6, 2);
            Array.Copy(_msecsArray, _msecsArray.Length - 4, _guidArray, _guidArray.Length - 4, 4);
            return new Guid(_guidArray);
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
            string _formatString = string.Empty;

            switch (guidMode)
            {
                case 0:
                    _formatString = guid.ToString("N");//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                    break;

                case 1:
                    _formatString = guid.ToString("D");//xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
                    break;

                case 2:
                    _formatString = guid.ToString("B");//{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
                    break;

                case 3:
                    _formatString = guid.ToString("P");//(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
                    break;

                default:
                    _formatString = guid.ToString();
                    break;
            }

            return _formatString;
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
            DateTime _baseDate = new DateTime(1900, 1, 1);
            byte[] _daysArray = new byte[4];
            byte[] _msecsArray = new byte[4];
            byte[] _guidArray = combGuid.ToByteArray();
            Array.Copy(_guidArray, _guidArray.Length - 6, _daysArray, 2, 2);
            Array.Copy(_guidArray, _guidArray.Length - 4, _msecsArray, 0, 4);
            Array.Reverse(_daysArray);
            Array.Reverse(_msecsArray);
            int _days = BitConverter.ToInt32(_daysArray, 0);
            int _msecs = BitConverter.ToInt32(_msecsArray, 0);
            DateTime _date = _baseDate.AddDays(_days);
            _date = _date.AddMilliseconds(_msecs * 3.333333);
            return _date;
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
            byte[] _buffer = guid.ToByteArray();
            return Convert.ToBase64String(_buffer);
        }

        /// <summary>
        /// 将GUID转换成符合SQL Server的GUID
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns>符合SQL Server的GUID</returns>
        public static Guid ToCOMBGuid(this Guid guid)
        {
            byte[] _guid = guid.ToByteArray();
            Array.Reverse(_guid, 0, 4);
            Array.Reverse(_guid, 4, 2);
            Array.Reverse(_guid, 6, 2);
            return new Guid(_guid);
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
            byte[] _guid = value.ToByteArray();
            return BitConverter.ToInt64(_guid, 0);
        }

        #endregion Methods
    }
}