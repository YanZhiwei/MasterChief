using MasterChief.DotNet.Core.Config;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.WindowsAPI;

namespace MasterChief.DotNet.Infrastructure.DaemonService
{
    /// <summary>
    ///     基于Windows Api 进程守护
    /// </summary>
    public sealed class WinApiDaemonProvider : IDaemonProvider
    {
        private readonly ConfigContext _configContext;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinApiDaemonProvider" /> class.
        /// </summary>
        /// <param name="configProvider">IConfigProvider</param>
        public WinApiDaemonProvider(IConfigProvider configProvider)
        {
            ValidateOperator.Begin().NotNull(configProvider, "IConfigProvider");
            ConfigProvider = configProvider;
            _configContext = new ConfigContext(ConfigProvider);
        }

        /// <summary>
        ///     文件配置
        /// </summary>
        public IConfigProvider ConfigProvider { get; }

        /// <summary>
        ///     运行需要守护的进程
        /// </summary>
        public void RunProcess()
        {
            var config = GetConfig();
            ValidateOperator.Begin()
                .NotNull(config, "进程守护配置信息")
                .CheckFileExists(config.InstallPath);
            WindowsCore.CreateProcess(config.InstallPath);
        }

        /// <summary>
        ///     获取进程守护配置信息
        /// </summary>
        /// <returns>
        ///     DaemonConfig
        /// </returns>
        public DaemonConfig GetConfig()
        {
            return _configContext.Get<DaemonConfig>();
        }
    }
}