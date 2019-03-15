using System.Text;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// ASCII 辅助类
    /// </summary>
    public static class AsciiHelper
    {
        #region Methods

        /*
         * 知识：
         * ASCII是基于拉丁字母的一套电脑编码系统。它主要用于显示现代英语和其他西欧语言。它是现今最通用的单字节编码系统，并等同于国际标准ISO/IEC 646。
         * 在计算机中，所有的数据在存储和运算时都要使用二进制数表示（因为计算机用高电平和低电平分别表示1和0），例如，像
         * a、b、c、d这样的52个字母（包括大写）、以及0、1等数字还有一些常用的符号（例如*、#、@等）在计算机中存储时也要使用
         * 二进制数来表示，而具体用哪些二进制数字表示哪个符号，当然每个人都可以约定自己的一套（这就叫编码），而大家如果要想
         * 互相通信而不造成混乱，那么大家就必须使用相同的编码规则，于是美国有关的标准化组织就出台了ASCII编码，统一规定了上
         * 述常用符号用哪些二进制数来表示。
         *
         * ASCII 码使用指定的7 位或8 位二进制数组合来表示128 或256 种可能的字符。标准ASCII 码也叫基础ASCII码，使用7 位二
         * 进制数来表示所有的大写和小写字母，数字0 到9、标点符号， 以及在美式英语中使用的特殊控制字符。
         */

        /// <summary>
        /// 将byte转化为ASCII
        /// </summary>
        /// <param name="data">数值</param>
        /// <returns>ASCII</returns>
        public static char ParseASCII(this byte data)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] array = { data };
            return asciiEncoding.GetString(array)[0];
        }

        /// <summary>
        /// 将字符转化为ASCII数值
        /// </summary>
        /// <param name="data">字符</param>
        /// <returns>ASCII数值</returns>
        public static int ToASCII(this char data)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            char[] array = { data };
            return asciiEncoding.GetBytes(array)[0];
        }

        #endregion Methods
    }
}