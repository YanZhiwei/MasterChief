using MasterChief.DotNet.Core.Config;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.WindowsAPI;

namespace MasterChief.DotNet.Infrastructure.DaemonService
{
    /// <summary>
    ///     基于Windows Api 进程守护
    /// </summary>
    public abstract class WinApiDaemonProvider : IDaemonProvider
    {
        protected readonly ConfigContext ConfigContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinApiDaemonProvider" /> class.
        /// </summary>
        /// <param name="configProvider">IConfigProvider</param>
        protected WinApiDaemonProvider(IConfigProvider configProvider)
        {
            ValidateOperator.Begin().NotNull(configProvider, "IConfigProvider");
            ConfigProvider = configProvider;
            ConfigContext = new ConfigContext(ConfigProvider);
        }

        /// <summary>
        ///     文件配置
        /// </summary>
        public IConfigProvider ConfigProvider { get; }

        /// <summary>
        ///     运行需要守护的进程
        /// </summary>
        public virtual void Run()
        {
            var config = GetConfig();
            if (CheckConfig(config)) WindowsCore.CreateProcess(config.InstallPath);
        }

        /// <summary>
        ///     执行批处理
        /// </summary>
        public virtual void RunBatchfile()
        {
            var config = GetConfig();
            if (CheckConfig(config)) WindowsCore.CreateProcess(config.BatchfilePath);
        }

        /// <summary>
        ///     获取进程守护配置信息
        /// </summary>
        /// <returns>
        ///     DaemonConfig
        /// </returns>
        public virtual DaemonConfig GetConfig()
        {
            return ConfigContext.Get<DaemonConfig>();
        }

        /// <summary>
        ///检查参数是否正确
        /// </summary>
        /// <param name="config">DaemonConfig.</param>
        /// <returns>是否合法</returns>
        public abstract bool CheckConfig(DaemonConfig config);
    }
}