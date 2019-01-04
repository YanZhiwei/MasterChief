using MasterChief.DotNet.ProjectTemplate.WebApi.Helper;
using MasterChief.DotNet4.Utilities.Common;
using System;
using System.Web.Http;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    /// WebApi 基类
    /// </summary>
    public class ApiBaseController : ApiController
    {
        #region Properties

        /// <summary>
        /// 当前通道ID
        /// </summary>
        public Guid CurrentAppId => Request.GetUriOrHeaderValue("Access_appId").ToGuidOrDefault(Guid.Empty);

        /// <summary>
        /// 当前令牌
        /// </summary>
        public string CurrentToken => Request.GetUriOrHeaderValue("Access_token").ToStringOrDefault(string.Empty);

        /// <summary>
        /// 当前用户
        /// </summary>
        public Guid CurrentUserId => Request.GetUriOrHeaderValue("Access_userId").ToGuidOrDefault(Guid.Empty);

        #endregion Properties
    }
}