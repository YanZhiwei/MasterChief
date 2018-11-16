namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Text;

    /// <summary>
    /// Random的帮助类
    /// </summary>
    public static class RandomHelper
    {
        #region Fields

        /// <summary>
        /// 随机种子
        /// </summary>
        public static readonly Random RandomSeed = null;

        /// <summary>
        /// 0~9 A~Z字符串
        /// </summary>
        public static readonly string RandomString09AZ = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RandomHelper()
        {
            RandomSeed = new Random((int)DateTime.Now.Ticks);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 生成随机字符串
        /// <para>eg:RandomHelper.NetxtString(4, false);</para>
        /// </summary>
        /// <param name="size">字符串长度</param>
        /// <param name="lowerCase">字符串是小写</param>
        /// <returns>随机字符串</returns>
        public static string NextString(int size, bool lowerCase)
        {
            StringBuilder _builder = new StringBuilder(size);
            int _startChar = lowerCase ? 97 : 65;  //65 = A / 97 = a

            for (int i = 0; i < size; i++)
            {
                _builder.Append((char)(26 * RandomSeed.NextDouble() + _startChar));
            }

            return _builder.ToString();
        }

        /// <summary>
        /// 依据指定字符串来生成随机字符串
        /// <para>eg:RandomHelper.NetxtString(RandomHelper.RandomString_09AZ, 4, false);</para>
        /// </summary>
        /// <param name="randomString">指定字符串</param>
        /// <param name="size">字符串长度</param>
        /// <param name="lowerCase">字符串是小写</param>
        /// <returns>随机字符串</returns>
        public static string NextString(string randomString, int size, bool lowerCase)
        {
            string _nextString = string.Empty;

            if (!string.IsNullOrEmpty(randomString))
            {
                StringBuilder _builder = new StringBuilder(size);
                int _maxCount = randomString.Length - 1;

                for (int i = 0; i < size; i++)
                {
                    int _number = RandomSeed.Next(0, _maxCount);
                    _builder.Append(randomString[_number]);
                }

                _nextString = _builder.ToString();
            }

            return lowerCase ? _nextString.ToLower() : _nextString.ToUpper();
        }

        /// <summary>
        /// 随机布尔值
        /// </summary>
        /// <returns>随机布尔值</returns>
        public static bool NextBool()
        {
            return RandomSeed.NextDouble() >= 0.5;
        }

        /// <summary>
        /// 生成随机颜色
        /// </summary>
        /// <returns>随机颜色</returns>
        public static Color NextColor()
        {
            return Color.FromArgb((byte)RandomSeed.Next(255), (byte)RandomSeed.Next(255), (byte)RandomSeed.Next(255));
        }

        /// <summary>
        /// 获取随机时间
        /// </summary>
        /// <returns>随机时间</returns>
        public static DateTime NextDateTime()
        {
            int _year = RandomSeed.Next(1900, DateTime.Now.Year),
                _month = RandomSeed.Next(1, 12),
                _day = RandomSeed.Next(1, DateTime.DaysInMonth(_year, _month)),
                _hour = RandomSeed.Next(0, 23),
                _minute = RandomSeed.Next(0, 59),
                _second = RandomSeed.Next(0, 59);
            string _dateTimeString = string.Format("{0}-{1}-{2} {3}:{4}:{5}", _year, _month, _day, _hour, _minute, _second);
            DateTime _nextTime = Convert.ToDateTime(_dateTimeString);
            return _nextTime;
        }

        /// <summary>
        /// 生成设置范围内的Double的随机数
        /// <para>eg:RandomHelper.NextDouble(1.5, 1.7);</para>
        /// </summary>
        /// <param name="miniDouble">生成随机数的最大值</param>
        /// <param name="maxiDouble">生成随机数的最小值</param>
        /// <returns>Double的随机数</returns>
        public static double NextDouble(double miniDouble, double maxiDouble)
        {
            return RandomSeed.NextDouble() * (maxiDouble - miniDouble) + miniDouble;
        }

        /// <summary>
        /// 生成随机十六进制字符串
        /// </summary>
        /// <param name="byteLength">生成随机十六进制字节长度</param>
        /// <returns>十六进制字符串</returns>
        /// 时间：2016/11/23 16:45
        /// 备注：
        public static string NextHexString(ushort byteLength)
        {
            byte[] _buffer = new byte[byteLength];
            RandomSeed.NextBytes(_buffer);
            StringBuilder _builder = new StringBuilder();

            foreach (byte x in _buffer)
            {
                _builder.Append(x.ToString("X2"));
            }

            return _builder.ToString();
        }

        /// <summary>
        /// 生成随机MAC地址
        /// <para>eg:RandomHelper.NextMacAddress();</para>
        /// </summary>
        /// <returns>新的MAC地址</returns>
        public static string NextMacAddress()
        {
            int _minValue = 0, _maxValue = 16;
            string _newMacAddress = string.Format("{0}{1}:{2}{3}:{4}{5}:{6}{7}:{8}{9}:{10}{11}", RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x"), RandomSeed.Next(_minValue, _maxValue).ToString("x")).ToUpper();
            return _newMacAddress;
        }

        /// <summary>
        /// 生成随机数【包括上下限】
        /// </summary>
        /// <param name="low">下限</param>
        /// <param name="high">上线</param>
        /// <returns>随机数</returns>
        public static int NextNumber(int low, int high)
        {
            return RandomSeed.Next(low, high);
        }

        /// <summary>
        /// 生成随机时间
        /// <para>eg:RandomHelper.NextTime();</para>
        /// </summary>
        /// <returns>随机时间</returns>
        public static DateTime NextTime()
        {
            int _hour = RandomSeed.Next(0, 23);
            int _minute = RandomSeed.Next(0, 60);
            int _second = RandomSeed.Next(0, 60);
            string _dateTimeString = string.Format("{0} {1}:{2}:{3}", DateTime.Now.ToString("yyyy-MM-dd"), _hour, _minute, _second);
            DateTime _nextTime = Convert.ToDateTime(_dateTimeString);
            return _nextTime;
        }

        #endregion Methods
    }
}