using System;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     彩色控制台输出
    /// </summary>
    /// 时间：2016/9/2 17:27
    /// 备注：
    public class ColorConsole
    {
        #region Methods

        /// <summary>
        ///     写入一行错误信息【红色】
        /// </summary>
        /// <param name="text">文本</param>
        /// 时间：2016/9/2 17:29
        /// 备注：
        public static void WriteError(string text)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        ///     写入一行信息【绿色】
        /// </summary>
        /// <param name="text">文本</param>
        /// 时间：2016/9/2 17:29
        /// 备注：
        public static void WriteInfo(string text)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        ///     写入一行
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="color">文本颜色</param>
        /// 时间：2016/9/2 17:28
        /// 备注：
        public static void WriteLine(string text, ConsoleColor color)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        ///     写入一行警告信息【红色】
        /// </summary>
        /// <param name="text">文本</param>
        /// 时间：2016/9/2 17:30
        /// 备注：
        public static void WriteWarn(string text)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        #endregion Methods
    }
}