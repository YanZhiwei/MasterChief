using System.IO;
using System.Reflection;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     资源文件操作帮助类
    /// </summary>
    public static class ResourceHelper
    {
        #region Methods

        /// <summary>
        ///     将嵌入的资源写入到本地
        /// </summary>
        /// <param name="resourceName">嵌入的资源名称【名称空间.资源名称】</param>
        /// <param name="filename">写入本地的路径</param>
        /// <returns>是否成功</returns>
        public static bool WriteFile(string resourceName, string filename)
        {
            var result = false;
            var assembly = Assembly.GetCallingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        var data = new byte[stream.Length];
                        stream.Read(data, 0, data.Length);
                        fileStream.Write(data, 0, data.Length);
                        result = true;
                    }
            }

            return result;
        }

        #endregion Methods
    }
}