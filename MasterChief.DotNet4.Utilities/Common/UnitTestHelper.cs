namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Reflection;

    /// <summary>
    /// 单元测试辅助类
    /// </summary>
    public sealed class UnitTestHelper
    {
        #region Methods

        /// <summary>
        ///获取exe执行文件夹路径
        /// </summary>
        /// <returns>exe执行文件夹路径</returns>
        public static string GetExecuteDirectory()
        {
            string _codeBase = Assembly.GetExecutingAssembly().CodeBase;
            _codeBase = _codeBase.Substring(8, _codeBase.Length - 8);
            string _filePath = string.Empty;
            string[] _codeSplit = _codeBase.Split('/');

            for (int i = 0; i < _codeSplit.Length - 1; i++)
            {
                _filePath += _codeSplit[i] + '\\';
            }

            return _filePath;
        }

        #endregion Methods
    }
}