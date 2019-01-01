namespace MasterChief.DotNet4.Utilities.Result
{
    /// <summary>
    /// 文件下载结果
    /// </summary>
    public sealed class FileDownloadResult : BasicResult<string>
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="filePhysicsPath">文件下载的物理路径</param>
        /// <param name="state">下载状态</param>
        /// <param name="message">附加信息</param>
        public FileDownloadResult(string fileName, string filePhysicsPath, bool state, string message)
        : base(message, null)
        {
            FileName = fileName;
            FilePhysicsPath = filePhysicsPath;
            State = state;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件下载的物理路径
        /// </summary>
        public string FilePhysicsPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 检查状态
        /// </summary>
        public bool State
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 返回失败结果
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="filePhysicsPath">文件下载的物理路径</param>
        /// <param name="message">附加信息</param>
        /// <returns>FileDownloadResult</returns>
        public static FileDownloadResult Fail(string fileName, string filePhysicsPath, string message)
        {
            FileDownloadResult _result = new FileDownloadResult(fileName, filePhysicsPath, false, message);
            return _result;
        }

        /// <summary>
        /// 返回成功结果
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="filePhysicsPath">文件下载的物理路径</param>
        /// <param name="message">附加信息</param>
        /// <returns>FileDownloadResult</returns>
        public static FileDownloadResult Success(string fileName, string filePhysicsPath, string message)
        {
            FileDownloadResult _result = new FileDownloadResult(fileName, filePhysicsPath, true, message);
            return _result;
        }

        /// <summary>
        /// 返回成功结果
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="filePhysicsPath">文件下载的物理路径</param>
        /// <returns>FileDownloadResult</returns>
        public static FileDownloadResult Success(string fileName, string filePhysicsPath)
        {
            return Success(fileName, filePhysicsPath, null);
        }

        #endregion Methods
    }
}