namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// FONT帮助类
    /// </summary>
    public class FontHelper
    {
        #region Methods

        /// <summary>
        /// Adds the font resource.
        /// </summary>
        /// <param name="lpFileName">Name of the lp file.</param>
        /// <returns>数值</returns>
        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);

        /// <summary>
        /// 字体安装
        /// </summary>
        /// <param name="fontSourcePath">字体所在路径</param>
        /// <returns>是否安装成功</returns>
        public static bool InstallFont(string fontSourcePath)
        {
            string _fontFile = FileHelper.GetFileName(fontSourcePath);
            string _targetFontPath = string.Format(@"{0}\fonts\{1}", Environment.GetEnvironmentVariable("WINDIR"), _fontFile);

            try
            {
                string _fontName = FileHelper.GetFileNameOnly(_targetFontPath);

                if (!File.Exists(_targetFontPath) && File.Exists(fontSourcePath))
                {
                    int _ret;
                    File.Copy(fontSourcePath, _targetFontPath);
                    _ret = AddFontResource(_targetFontPath);
                    _ret = WriteProfileString("fonts", _fontName + "(TrueType)", _fontFile);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>数值</returns>
        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint msg, int wParam, int lParam);

        /// <summary>
        /// Writes the profile string.
        /// </summary>
        /// <param name="lpszSection">The LPSZ section.</param>
        /// <param name="lpszKeyName">Name of the LPSZ key.</param>
        /// <param name="lpszString">The LPSZ string.</param>
        /// <returns>数值</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        #endregion Methods
    }
}