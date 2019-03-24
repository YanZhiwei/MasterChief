using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     FONT帮助类
    /// </summary>
    public static class FontHelper
    {
        #region Methods

        /// <summary>
        ///     Adds the font resource.
        /// </summary>
        /// <param name="lpFileName">Name of the lp file.</param>
        /// <returns>数值</returns>
        [DllImport("gdi32")]
        public static extern int AddFontResource(string lpFileName);

        /// <summary>
        ///     字体安装
        /// </summary>
        /// <param name="fontSourcePath">字体所在路径</param>
        /// <returns>是否安装成功</returns>
        public static bool InstallFont(string fontSourcePath)
        {
            var fontFile = FileHelper.GetFileName(fontSourcePath);
            var targetFontPath = $@"{Environment.GetEnvironmentVariable("WINDIR")}\fonts\{fontFile}";

            try
            {
                var fontName = FileHelper.GetFileNameOnly(targetFontPath);

                if (!File.Exists(targetFontPath) && File.Exists(fontSourcePath))
                {
                    File.Copy(fontSourcePath, targetFontPath);
                    AddFontResource(targetFontPath);
                    WriteProfileString("fonts", fontName + "(TrueType)", fontFile);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, uint msg, int wParam, int lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        #endregion Methods
    }
}