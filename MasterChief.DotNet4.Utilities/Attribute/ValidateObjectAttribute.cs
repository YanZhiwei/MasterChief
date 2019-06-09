using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet4.Utilities.Attribute
{
    /// <summary>
    ///     ValidateObjectAttribute
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    public class ValidateObjectAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Returns true if ... is valid.
        /// </summary>
        /// <param name="value">要验证的值。</param>
        /// <param name="validationContext">有关验证操作的上下文信息。</param>
        /// <returns>
        ///     <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> 类的实例。
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Any())
            {
                var validatorResults =
                    new DataAnnotationResult($"验证 {validationContext.DisplayName} 失败.");
                results.ForEach(validatorResults.Add);

                return validatorResults;
            }

            return ValidationResult.Success;
        }
    }
}