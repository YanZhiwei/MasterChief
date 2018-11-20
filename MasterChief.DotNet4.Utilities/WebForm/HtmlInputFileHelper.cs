namespace MasterChief.DotNet4.Utilities.WebForm
{
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// HtmlInputFile 帮助类
    /// </summary>
    /// 时间：2016-01-04 16:57
    /// 备注：
    public static class HtmlInputFileHelper
    {
        #region Methods

        /// <summary>
        /// 将文件读取成byte数组
        /// </summary>
        /// <param name="htmlInputFile">HtmlInputFile</param>
        /// <returns>若未有文件，则返回NULL；</returns>
        /// 时间：2016-01-04 17:07
        /// 备注：
        public static byte[] GetBytes(this HtmlInputFile htmlInputFile)
        {
            int _fsize = htmlInputFile.PostedFile.ContentLength;
            if (_fsize > 0)
            {
                byte[] _imageBuffer = new byte[_fsize];
                htmlInputFile.PostedFile.InputStream.Read(_imageBuffer, 0, _fsize);
                return _imageBuffer;
            }
            return null;
        }

        #endregion Methods
    }
}