using System;
using System.Web.Http.Filters;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Filter
{
    /// <summary>
    ///     WebApi 授权验证实现
    /// </summary>
    public abstract class AuthenticateAttribute : AuthorizationFilterAttribute
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="apiAuthenticate">IApiAuthenticate</param>
        /// <param name="appCfgService">appCfgService</param>
        protected AuthenticateAttribute(IApiAuthenticate apiAuthenticate, IAppConfigService appCfgService)
        {
            ValidateOperator.Begin()
                .NotNull(apiAuthenticate, "IApiAuthenticate")
                .NotNull(appCfgService, "IAppConfigService");
            ApiAuthenticate = apiAuthenticate;
            AppCfgService = appCfgService;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     授权验证接口
        /// </summary>
        protected readonly IApiAuthenticate ApiAuthenticate;

        /// <summary>
        ///     请求通道配置信息，可以从文件或者数据库获取
        /// </summary>
        protected readonly IAppConfigService AppCfgService;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     验证Token令牌是否合法
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="appid">应用ID</param>
        /// <returns>CheckResult</returns>
        protected virtual OperatedResult<string> CheckIdentityToken(string token, Guid appid)
        {
            #region 请求参数检查

            var checkResult = CheckRequest(token, appid);

            if (!checkResult.State)
                return OperatedResult<string>.Fail(checkResult.Message);

            #endregion

            #region 请求通道检查

            var getAppConfig = AppCfgService.Get(appid);

            if (!getAppConfig.State) return OperatedResult<string>.Fail(getAppConfig.Message);
            var appConfig = getAppConfig.Data;

            #endregion

            return ApiAuthenticate.CheckIdentityToken(token, appConfig);
        }

        private CheckResult CheckRequest(string token, Guid appid)
        {
            if (string.IsNullOrEmpty(token))
                return CheckResult.Fail("用户令牌为空");
            return Guid.Empty == appid ? CheckResult.Fail("应用ID非法") : CheckResult.Success();
        }

        #endregion Methods
    }
}