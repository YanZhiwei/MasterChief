using System;
using System.Data.Entity.Validation;
using System.Text;

namespace MasterChief.DotNet.Core.EF.Helper
{
    internal static class DbContextHelper
    {
        /// <summary>
        ///     获取DbEntityValidationException详细异常信息
        /// </summary>
        /// <param name="exc">DbEntityValidationException</param>
        /// <returns>DbEntityValidationException详细异常信息</returns>
        public static string GetFullErrorText(this DbEntityValidationException exc)
        {
            var builder = new StringBuilder();
            foreach (var validationErrors in exc.EntityValidationErrors)
            foreach (var error in validationErrors.ValidationErrors)
                builder.AppendFormat("Property: {0} Error: {1}{2}", error.PropertyName, error.ErrorMessage,
                    Environment.NewLine);
            return builder.ToString();
        }
    }
}