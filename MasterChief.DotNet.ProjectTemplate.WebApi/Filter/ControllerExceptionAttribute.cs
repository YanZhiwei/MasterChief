using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Filters;
using MasterChief.DotNet4._5.Utilities.Model;
using MasterChief.DotNet4._5.Utilities.Web;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Filter
{
    /// <summary>
    ///     异常记录过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ControllerExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        ///     当WebApi Action发生异常
        /// </summary>
        /// <param name="actionExecutedContext">HttpActionExecutedContext</param>
        /// <param name="actionName">WebApi Action名称</param>
        /// <param name="statusCode">HttpStatusCode</param>
        /// <param name="requestRaw">Request原始信息</param>
        public abstract void OnActionExceptioning(HttpActionExecutedContext actionExecutedContext, string actionName,
            HttpStatusCode statusCode, HttpRequestRaw requestRaw);

        /// <summary>
        ///     引发异常事件。
        /// </summary>
        /// <param name="actionExecutedContext">操作的上下文。</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            // ReSharper disable once RedundantAssignment
            var statusCode = HttpStatusCode.OK;
            if (exception is KeyNotFoundException || exception is ArgumentOutOfRangeException)
                statusCode = HttpStatusCode.NotFound;
            else if (exception is ArgumentException)
                statusCode = HttpStatusCode.BadRequest;
            else
                statusCode = HttpStatusCode.InternalServerError;
            var actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            var requestRaw = actionExecutedContext.Request.ToRaw();
            OnActionExceptioning(actionExecutedContext, actionName, statusCode, requestRaw);
        }
    }
}