using System;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    ///     Api授权
    /// </summary>
    public abstract class AuthorizeController : ApiBaseController
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="apiAuthorize">IApiAuthorize</param>
        /// <param name="appCfgService">IAppConfigService</param>
        protected AuthorizeController(IApiAuthorize apiAuthorize, IAppConfigService appCfgService)
        {
            ValidateOperator.Begin()
                .NotNull(apiAuthorize, "IApiAuthorize")
                .NotNull(appCfgService, "IAppConfigService");
            ApiAuthorize = apiAuthorize;
            AppCfgService = appCfgService;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     授权接口
        /// </summary>
        protected readonly IApiAuthorize ApiAuthorize;

        /// <summary>
        ///     请求通道配置信息，可以从文件或者数据库获取
        /// </summary>
        protected readonly IAppConfigService AppCfgService;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     创建合法用户的Token
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passWord">用户密码</param>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appid">应用接入ID</param>
        /// <returns>OperatedResult</returns>
        protected virtual OperatedResult<IdentityToken> CreateIdentityToken(string userId, string passWord,
            string signature, string timestamp,
            string nonce, Guid appid)
        {
            #region  参数检查

            var checkResult = CheckRequest(userId, passWord, signature, timestamp, nonce, appid);

            if (!checkResult.State)
                return OperatedResult<IdentityToken>.Fail(checkResult.Message);

            #endregion

            #region 用户鉴权

            var getIdentityUser = GetIdentityUser(userId, passWord);

            if (!getIdentityUser.State) return OperatedResult<IdentityToken>.Fail(getIdentityUser.Message);

            #endregion

            #region 请求通道检查

            var getAppConfig = AppCfgService.Get(appid);

            if (!getAppConfig.State) return OperatedResult<IdentityToken>.Fail(getAppConfig.Message);
            var appConfig = getAppConfig.Data;

            #endregion

            #region 检查请求签名检查

            var checkSignatureResult = ApiAuthorize.CheckRequestSignature(signature, timestamp, nonce, appConfig);
            if (!checkSignatureResult.State) return OperatedResult<IdentityToken>.Fail(checkSignatureResult.Message);

            #endregion

            #region 生成基于Jwt Token

            var getTokenResult = ApiAuthorize.CreateIdentityToken(getIdentityUser.Data, getAppConfig.Data);
            if (!getTokenResult.State) return OperatedResult<IdentityToken>.Fail(getTokenResult.Message);

            return OperatedResult<IdentityToken>.Success(getTokenResult.Data);

            #endregion
        }


        /// <summary>
        ///     检查用户的合法性
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passWord">用户密码</param>
        /// <returns>UserInfo</returns>
        protected abstract CheckResult<IdentityUser> GetIdentityUser(string userId, string passWord);

        private CheckResult CheckRequest(string userId, string passWord, string signature, string timestamp,
            string nonce, Guid appid)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(passWord))
                return CheckResult.Fail("用户名或密码为空");

            if (string.IsNullOrEmpty(signature))
                return CheckResult.Fail("请求签名为空");

            if (string.IsNullOrEmpty(timestamp))
                return CheckResult.Fail("时间戳为空");

            if (string.IsNullOrEmpty(nonce))
                return CheckResult.Fail("随机数为空");

            if (appid == Guid.Empty)
                return CheckResult.Fail("应用接入ID非法");

            return CheckResult.Success();
        }

        #endregion Methods
    }
}