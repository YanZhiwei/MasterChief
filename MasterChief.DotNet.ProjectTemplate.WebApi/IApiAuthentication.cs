using System;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    ///     webApi 验证系统基本接口
    /// </summary>
    public interface IApiAuthentication
    {
        #region Methods

        /// <summary>
        ///     验证Token令牌是否合法
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="appid">应用ID</param>
        /// <returns>CheckResult</returns>
        OperatedResult<string> ValidateToken(string token, Guid appid);

        #endregion Methods
    }
}