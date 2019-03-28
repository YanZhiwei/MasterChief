using System;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    /// WebApi 授权接口
    /// </summary>
    public interface IApiAuthorization
    {
        /// <summary>
        ///     注册用户获取访问令牌接口
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passWord">用户密码</param>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appid">应用接入ID</param>
        /// <returns>OperatedResult</returns>
        OperatedResult<IdentityToken> GetAccessToken(string userId, string passWord,
            string signature, string timestamp,
            string nonce, Guid appid);
    }
}