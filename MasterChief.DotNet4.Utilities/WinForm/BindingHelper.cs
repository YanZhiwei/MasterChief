using System;
using System.Linq.Expressions;
using System.Windows.Forms;
using MasterChief.DotNet4.Utilities.Common;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     Winform 控件以及用户控件绑定辅助类
    /// </summary>
    public static class BindingHelper
    {
        /// <summary>
        ///     创建用户控件/控件绑定
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="propertyPredicate">控件属性筛选委托</param>
        /// <param name="dataSource">绑定数据源</param>
        /// <param name="dataMemberPredicate">数据源筛选委托</param>
        /// <param name="formattingEnabled">是否格式化显示的数据</param>
        /// <param name="updateMode">DataSourceUpdateMode</param>
        /// <returns>Binding</returns>
        public static Binding SetBinding<C, T, P, D>(this C control, Expression<Func<C, P>> propertyPredicate,
            T dataSource, Expression<Func<T, D>> dataMemberPredicate, bool formattingEnabled = false,
            DataSourceUpdateMode updateMode = DataSourceUpdateMode.OnPropertyChanged)
            where C : Control

        {
            if (!(control is UserControl) && !control.CausesValidation)
                control.CausesValidation = false;
            return control.DataBindings.Add(propertyPredicate.Body.GetMemberName(), dataSource,
                dataMemberPredicate.Body.GetMemberName(), formattingEnabled, updateMode);
        }
    }
}