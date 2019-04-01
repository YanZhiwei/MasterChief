using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Filter
{
    /// <summary>
    ///     请求参数验证
    /// </summary>
    /// <seealso cref="System.Web.Http.Filters.ActionFilterAttribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     参数为NULL的时候
        /// </summary>
        /// <param name="actionContext">操作上下文</param>
        public abstract void OnParameterIsNulling(HttpActionContext actionContext);

        /// <summary>
        ///     参数验证不通过
        /// </summary>
        /// <param name="actionContext">操作上下文</param>
        /// <param name="result">ValidationFailedResult</param>
        public abstract void OnParameterInvaliding(HttpActionContext actionContext, ValidationFailedResult result);

        /// <summary>
        ///     在调用操作方法之前发生。
        /// </summary>
        /// <param name="actionContext">操作上下文。</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                if (actionContext.ActionArguments.Any(kv => kv.Value == null))
                {
                    OnParameterIsNulling(actionContext);
                }
                else
                {
                    var validateFailedResult =
                        new ValidationFailedResult(422, "请求格式正确，但是由于含有语义错误，无法响应。", actionContext.ModelState);
                    OnParameterInvaliding(actionContext, validateFailedResult);
                }

                //actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, OnParameterInvalid(_validateFailedResult));
            }
        }
    }
}