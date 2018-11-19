namespace MasterChief.DotNet4.Utilities.Common
{
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// 资源文件操作帮助类
    /// </summary>
    public static class ResourceHelper
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
            bool result = false;
            Assembly assembly = Assembly.GetCallingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (FileStream fileStream = new FileStream(filename, FileMode.Create))
                    {
                        byte[] data = new byte[stream.Length];
                        stream.Read(data, 0, data.Length);
                        fileStream.Write(data, 0, data.Length);
                        result = true;
                    }
                }
            }
            return result;
        }

        #endregion Methods
    }
}