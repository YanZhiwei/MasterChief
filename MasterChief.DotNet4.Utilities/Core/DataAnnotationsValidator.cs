using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet4.Utilities.Core
{
    /// <summary>
    ///     DataAnnotations  验证
    /// </summary>
    public sealed class DataAnnotationsValidator
    {
        #region Methods

        /// <summary>
        ///     尝试验证实体类
        /// </summary>
        /// <param name="model">实体类数据</param>
        /// <param name="validationResults">若验证失败，验证结果</param>
        /// <returns>验证是否成功</returns>
        public static bool TryValidate(object model, out List<ValidationResult> validationResults)
        {
            ValidateOperator.Begin().NotNull(model, "实体类对象");
            var context = new ValidationContext(model, null, null);
            validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject(model, context, validationResults, true);
            if (!result)
            {
                GetDataAnnotationResult(validationResults, out var validations);
                validationResults = validations;
            }

            return result;
        }

        private static void GetDataAnnotationResult(IEnumerable<ValidationResult> results, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            foreach (var validationResult in results)
                if (validationResult is DataAnnotationResult result)
                    GetDataAnnotationResult(result.Results, out validationResults);
                else
                    validationResults.Add(validationResult);
        }

        #endregion Methods
    }
}