using MasterChief.DotNet.Core.Config;

namespace MasterChief.DotNet.Infrastructure.DaemonService
{
    /// <summary>
    ///     进程守护提供者接口
    /// </summary>
    public interface IDaemonProvider
    {
        /// <summary>
        ///     文件配置
        /// </summary>
        IConfigProvider ConfigProvider { get; }

        /// <summary>
        ///     运行需要守护的进程
        /// </summary>
        void Run();

        /// <summary>
        ///     获取进程守护配置信息
        /// </summary>
        /// <returns>DaemonConfig</returns>
        DaemonConfig GetConfig();
    }
}