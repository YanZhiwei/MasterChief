using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet4.Utilities.Attribute
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    public class ValidateCollectionAttribute : ValidationAttribute
    {
        /// <summary>
        ///     Gets or sets the type of the validation.
        /// </summary>
        /// <value>
        ///     The type of the validation.
        /// </value>
        public Type ValidationType { get; set; }

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
            var collectionResults = new DataAnnotationResult(
                $"验证 {validationContext.DisplayName} 失败.");
            var enumerable = value as IEnumerable;

            var validators = GetValidators().ToList();
            if (enumerable == null) return ValidationResult.Success;

            var i = 0;

            foreach (var item in enumerable)
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(item, validationContext.ServiceContainer, null);

                if (ValidationType != null)
                    Validator.TryValidateValue(item, context, results, validators);
                else
                    Validator.TryValidateObject(item, context, results, true);

                if (results.Count != 0)
                {
                    var validatorResults =
                        new DataAnnotationResult(
                            $"验证 {validationContext.DisplayName}[{i}] 失败.");

                    results.ForEach(validatorResults.Add);

                    collectionResults.Add(validatorResults);
                }

                i++;
            }


            if (collectionResults.Results.Any()) return collectionResults;

            return ValidationResult.Success;
        }

        /// <summary>
        ///     Gets the validators.
        /// </summary>
        /// <returns>ValidationAttributes</returns>
        private IEnumerable<ValidationAttribute> GetValidators()
        {
            if (ValidationType == null) yield break;

            yield return (ValidationAttribute) Activator.CreateInstance(ValidationType);
        }
    }
}