using System;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    ///     API请求通道接口
    /// </summary>
    public interface IAppConfigService
    {
        /// <summary>
        ///     根据appId获取请求通道配置信息
        /// </summary>
        /// <param name="appid">appId</param>
        /// <returns>AppConfig</returns>
        CheckResult<AppConfig> Get(Guid appid);

        /// <summary>
        ///     保存配置
        /// </summary>
        /// <param name="appConfig">AppConfig</param>
        void Save(AppConfig appConfig);
    }
}