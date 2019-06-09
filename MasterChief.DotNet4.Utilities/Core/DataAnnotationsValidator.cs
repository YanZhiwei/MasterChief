using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet4.Utilities.Core
{
    /// <summary>
    ///     DataAnnotations  验证
    /// </summary>
    public sealed class DataAnnotationsValidator
    {
        /// <summary>
        ///     尝试验证实体类
        /// </summary>
        /// <param name="model">实体类数据</param>
        /// <param name="results">若验证失败，验证结果</param>
        /// <returns>验证是否成功</returns>
        public static bool TryValidate(object model, out ICollection<ValidationResult> results)
        {
            ValidateOperator.Begin().NotNull(model, "实体类对象");
            var context = new ValidationContext(model, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(model, context, results, true);
        }
    }
}