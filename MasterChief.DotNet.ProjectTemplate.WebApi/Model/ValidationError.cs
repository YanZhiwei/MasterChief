using System;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Model
{
    /// <summary>
    ///     验证错误信息
    /// </summary>
    [Serializable]
    public class ValidationError
    {
        /// <summary>
        ///     对应字段
        /// </summary>
        public readonly string Field;

        /// <summary>
        ///     错误信息
        /// </summary>
        public readonly string Message;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="field">对应字段</param>
        /// <param name="message">错误信息</param>
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }
}