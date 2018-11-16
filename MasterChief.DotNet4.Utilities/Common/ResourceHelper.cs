namespace MasterChief.DotNet4.Utilities.Common
{
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// 资源文件操作帮助类
    /// </summary>
    public class ResourceHelper
    {
        #region Methods

        /// <summary>
        /// 将嵌入的资源写入到本地
        /// </summary>
        /// <param name="resourceName">嵌入的资源名称【名称空间.资源名称】</param>
        /// <param name="filename">写入本地的路径</param>
        /// <returns>是否成功</returns>
        public static bool WriteFile(string resourceName, string filename)
        {
            bool _result = false;
            Assembly _curCall = Assembly.GetCallingAssembly();
            using (Stream stream = _curCall.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Create))
                    {
                        byte[] _byte = new byte[stream.Length];
                        stream.Read(_byte, 0, _byte.Length);
                        fs.Write(_byte, 0, _byte.Length);
                        _result = true;
                    }
                }
            }
            return _result;
        }

        #endregion Methods
    }
}