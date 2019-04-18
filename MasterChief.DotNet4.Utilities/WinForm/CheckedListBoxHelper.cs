using System.Collections.Generic;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     CheckedList 帮助类
    /// </summary>
    public static class CheckedListBoxHelper
    {
        #region Methods

        /// <summary>
        ///     获取选中项集合
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="checkedListBox">CheckedListBox</param>
        /// <returns>选中项集合</returns>
        public static List<T> GetCheckedItemList<T>(this CheckedListBox checkedListBox)
            where T : class
        {
            var checkedItemList = new List<T>();
            for (var i = 0; i < checkedListBox.CheckedItems.Count; i++)
            {
                var item = (T) checkedListBox.Items[i];
                checkedItemList.Add(item);
            }

            return checkedItemList;
        }

        /// <summary>
        ///     设置项勾选状态
        /// </summary>
        /// <param name="checkedListBox">CheckedListBox</param>
        /// <param name="state">是否勾选</param>
        public static void SetAllItemState(this CheckedListBox checkedListBox, bool state)
        {
            for (var i = 0; i < checkedListBox.Items.Count; i++) checkedListBox.SetItemChecked(i, state);
        }

        /// <summary>
        ///     CheckedListBox 数据绑定
        /// </summary>
        /// <param name="checkedListBox">CheckedListBox</param>
        /// <param name="dataSource">绑定数据源</param>
        /// <param name="valueMember">隐式字段</param>
        /// <param name="displayMember">显示字段</param>
        public static void SetDataSource(this CheckedListBox checkedListBox, object dataSource, string valueMember,
            string displayMember)
        {
            checkedListBox.DataSource = dataSource;
            checkedListBox.ValueMember = valueMember;
            checkedListBox.DisplayMember = displayMember;
        }

        #endregion Methods
    }
}