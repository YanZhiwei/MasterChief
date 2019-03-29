using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MasterChief.DotNet.Core.Cache;
using MasterChief.DotNet4.Utilities.Operator;
namespace MasterChief.DotNet.ProjectTemplate.WebApi.Filter
{
    /// <summary>
    ///     缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ControllerCacheAttribute : ActionFilterAttribute
    {
        #region Constructors

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dependsOnIdentity">缓存取决于访问令牌</param>
        /// <param name="cacheProvider">ICacheProvider</param>
        protected ControllerCacheAttribute(bool dependsOnIdentity, ICacheProvider cacheProvider)
        {
            ValidateOperator.Begin().NotNull(cacheProvider, "ICacheProvider");
            DependsOnIdentity = dependsOnIdentity;
            CacheProvider = cacheProvider;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     缓存时间【秒】
        /// </summary>
        protected int CacheSeconds { get; set; }

        /// <summary>
        ///     缓存取决于访问令牌
        /// </summary>
        protected readonly bool DependsOnIdentity;

        /// <summary>
        ///     ICacheProvider
        /// </summary>
        protected readonly ICacheProvider CacheProvider;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     创建用于缓存Key
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <returns>缓存Key</returns>
        protected virtual string CreateCacheKey(HttpActionContext actionContext)
        {
            var cachekey = string.Join(":", actionContext.Request.RequestUri.OriginalString,
                actionContext.Request.Headers.Contains("User-Agent")
                    ? actionContext.Request.Headers.UserAgent.ToString()
                    : string.Empty);

            if (DependsOnIdentity)
                cachekey = cachekey.Insert(0, GetIdentityToken(actionContext));

            return cachekey;
        }

        /// <summary>
        ///     Called when [action executed].
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!CheckedCacheEnable()) return;
            if (actionExecutedContext.Response?.Content != null)
            {
                var cachekey = CreateCacheKey(actionExecutedContext.ActionContext);
                ModifyCache(cachekey, actionExecutedContext);
            }
        }

        /// <summary>
        ///     Called when [action executing].
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!CheckedCacheEnable() || !CheckedCurRequestCacheEnable(actionContext)) return;
            var cachekey = CreateCacheKey(actionContext);

            if (string.IsNullOrEmpty(cachekey) || !CacheProvider.IsSet(cachekey)) return;
            var cacheResult = CacheProvider.Get<string>(cachekey);

            if (cacheResult == null) return;
            actionContext.Response = actionContext.Request.CreateResponse();
            actionContext.Response.Content = new StringContent(cacheResult);
            var contentment = CacheProvider.Get<MediaTypeHeaderValue>(cachekey + ":response-ct") ??
                              MediaTypeHeaderValue.Parse("application/json; charset=utf-8");

            actionContext.Response.Content.Headers.ContentType = contentment;
        }

        /// <summary>
        ///     检查Response字符串是否合法，用于判断Response字符串是否可以缓存
        ///     用于正确的响应才缓存结果
        /// </summary>
        /// <param name="context">HttpActionContext</param>
        /// <param name="responseText">请求响应内容</param>
        /// <returns>否可以缓存</returns>
        protected abstract bool CheckedResponseAvailable(HttpActionContext context, string responseText);

        /// <summary>
        ///     获取身份访问令牌
        /// </summary>
        /// <param name="actionContext">HttpActionContext</param>
        /// <returns>身份访问令牌</returns>
        protected abstract string GetIdentityToken(HttpActionContext actionContext);

        /// <summary>
        ///     判断是否可以缓存
        /// </summary>
        private bool CheckedCacheEnable()
        {
            return CacheProvider != null;
        }

        /// <summary>
        ///     当前请求是否可缓存
        /// </summary>
        private bool CheckedCurRequestCacheEnable(HttpActionContext context)
        {
            if (CacheSeconds <= 0) return false;
            return context.Request.Method == HttpMethod.Get || context.Request.Method == HttpMethod.Post;
        }

        private void ModifyCache(string cachekey, HttpActionExecutedContext actionExecutedContext)
        {
            if (string.IsNullOrEmpty(cachekey)) return;
            var responseText = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

            if (!CheckedResponseAvailable(actionExecutedContext.ActionContext, responseText)) return;
            var contentType = actionExecutedContext.Response.Content.Headers.Contains("Content-Type")
                ? actionExecutedContext.Response.Content.Headers.ContentType
                : MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
            var cacheExpire = TimeSpan.FromSeconds(CacheSeconds).Minutes;

            if (CacheProvider.IsSet(cachekey))
            {
                CacheProvider.Remove(cachekey);
                CacheProvider.Remove(cachekey + ":response-ct");
            }

            CacheProvider.Set(cachekey, responseText, cacheExpire);
            CacheProvider.Set(cachekey + ":response-ct", contentType, cacheExpire);
        }

        #endregion Methods
    }
}