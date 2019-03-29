using System;
using MasterChief.DotNet.Core.Config;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    ///     默认请求通道配置信息
    /// </summary>
    public sealed class AppConfigService : IAppConfigService
    {
        private readonly ConfigContext _configContext;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="configContext">ConfigContext</param>
        public AppConfigService(ConfigContext configContext)
        {
            _configContext = configContext;
        }

        /// <summary>
        ///     根据appId获取请求通道配置信息
        /// </summary>
        /// <param name="appid">appId</param>
        /// <returns>AppConfig</returns>
        public CheckResult<AppConfig> Get(Guid appid)
        {
            var appConfig = _configContext.Get<AppConfig>(appid.ToString());
            return appConfig != null
                ? CheckResult<AppConfig>.Success(appConfig)
                : CheckResult<AppConfig>.Fail($"{appid}配置参数缺失.");
        }

        /// <summary>
        ///     保存配置
        /// </summary>
        /// <param name="appConfig">AppConfig</param>
        public void Save(AppConfig appConfig)
        {
            ValidateOperator.Begin().NotNull(appConfig, "AppConfig");
            _configContext.Save(appConfig);
        }
    }
}