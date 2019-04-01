using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http.ModelBinding;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Model
{
    /// <summary>
    ///     验证失败结果
    /// </summary>
    [Serializable]
    public class ValidationFailedResult : BasicResult<List<ValidationError>>
    {
        /// <summary>
        ///     响应状态码
        /// </summary>
        public readonly int StatusCode;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="statusCode">响应状态码</param>
        /// <param name="message">附加错误信息</param>
        /// <param name="modelState">ModelStateDictionary</param>
        public ValidationFailedResult(int statusCode, string message, ModelStateDictionary modelState)
        {
            base.Data = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
            base.Message = message;
            StatusCode = statusCode;
        }
    }
}