using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MasterChief.DotNet4.Utilities.Result
{
    /// <summary>
    ///     DataAnnotationResult
    /// </summary>
    public sealed class DataAnnotationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        public DataAnnotationResult(string errorMessage) : base(errorMessage)
        {
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="memberNames">memberNames</param>
        public DataAnnotationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage,
            memberNames)
        {
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="validationResult">ValidationResult</param>
        public DataAnnotationResult(ValidationResult validationResult) : base(validationResult)
        {
        }

        /// <summary>
        ///     ValidationResult 集合
        /// </summary>
        public IEnumerable<ValidationResult> Results => _results;

        /// <summary>
        ///     添加ValidationResult
        /// </summary>
        /// <param name="validationResult">ValidationResult</param>
        public void Add(ValidationResult validationResult)
        {
            if (validationResult != null)
                _results.Add(validationResult);
        }
    }
}