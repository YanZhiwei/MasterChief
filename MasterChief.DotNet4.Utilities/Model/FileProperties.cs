namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileProperties
    {
        /// <summary>
        /// 根目录
        /// </summary>
        public string Root
        {
            get;
            set;
        }

        /// <summary>
        /// 文件夹
        /// </summary>
        public string Folder
        {
            get;
            set;
        }

        /// <summary>
        ///  上一级文件
        /// </summary>
        public string SubFolder
        {
            get;
            set;
        }

        /// <summary>
        ///  文件名称
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件后缀
        /// </summary>
        public string FileNameExt
        {
            get;
            set;
        }
    }
}