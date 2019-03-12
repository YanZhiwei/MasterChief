using MasterChief.DotNet4.Utilities.Common;
using System;
using System.Linq.Expressions;

namespace MasterChief.DotNet4.Utilities.ComponentModel
{
    /// <summary>
    /// 通知客户端属性值已更改 辅助类
    /// </summary>
    public static class NotificationHelper
    {
        #region Methods

        /// <summary>
        /// 通知客户端属性值已更改
        /// </summary>
        /// <param name="model">实体类</param>
        /// <param name="keySelector">表达式</param>
        public static void NotifyPropertyChanged<T, TProperty>(this T model, Expression<Func<T, TProperty>> keySelector)
            where T : NotificationObject
        {
            var propertyName = keySelector.Body.GetMemberName();

            if (!string.IsNullOrEmpty(propertyName))
            {
                model.NotifyChanges(propertyName);
            }
        }

        #endregion Methods
    }
}