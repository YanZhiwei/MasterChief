using MasterChief.DotNet4._5.Utilities.Model;
using MasterChief.DotNet4._5.Utilities.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Filters;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Filter
{
    /// <summary>
    /// 异常记录过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ControllerExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 当WebApi Action发生异常
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext</param>
        /// <param name="actionName">WebApi Action名称</param>
        /// <param name="statusCode">HttpStatusCode</param>
        /// <param name="requestRaw">Request原始信息</param>
        public abstract void OnActionExceptioning(HttpActionExecutedContext actionExecutedContext, string actionName, HttpStatusCode statusCode, HttpRequestRaw requestRaw);

        /// <summary>
        /// 引发异常事件。
        /// </summary>
        /// <param name="actionExecutedContext">操作的上下文。</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception _exception = actionExecutedContext.Exception;
            HttpStatusCode _statusCode = HttpStatusCode.OK;
            if (_exception is KeyNotFoundException || _exception is ArgumentOutOfRangeException)
            {
                _statusCode = HttpStatusCode.NotFound;
            }
            else if (_exception is ArgumentException)
            {
                _statusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                _statusCode = HttpStatusCode.InternalServerError;
            }
            string _actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            HttpRequestRaw _requestRaw = actionExecutedContext.Request.ToRaw();
            OnActionExceptioning(actionExecutedContext, _actionName, _statusCode, _requestRaw);
        }
    }
}