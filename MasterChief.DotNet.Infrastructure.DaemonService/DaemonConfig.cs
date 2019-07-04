using System;

namespace MasterChief.DotNet.Infrastructure.DaemonService
{
    /// <summary>
    ///     守护进程 配置信息
    /// </summary>
    [Serializable]
    public sealed class DaemonConfig
    {
        /// <summary>
        ///     进程名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        ///     安装路径
        /// </summary>
        public string InstallPath { get; set; }


        /// <summary>
        ///     批处理文件路径
        /// </summary>
        public string BatchfilePath { get; set; }

        /// <summary>
        ///     应用程序的名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///     参数
        /// </summary>
        public string Args { get; set; }
    }
}