using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     ListBox 帮助类
    /// </summary>
    public static class ListBoxHelper
    {
        #region Methods

        /// <summary>
        ///     ITEM下移动，适合不是datasource绑定情况
        /// </summary>
        /// <param name="lsbox">ListBox</param>
        public static void ItemMoveDown(this ListBox lsbox)
        {
            var itemCnt = lsbox.Items.Count;
            var selectedIndex = lsbox.SelectedIndex;
            if (itemCnt > selectedIndex && selectedIndex < itemCnt - 1)
            {
                var selectedItem = lsbox.SelectedItem;
                lsbox.Items.RemoveAt(selectedIndex);
                lsbox.Items.Insert(selectedIndex + 1, selectedItem);
                lsbox.SelectedIndex = selectedIndex + 1;
            }
        }

        /// <summary>
        ///     添加并选中
        /// </summary>
        /// <param name="lsbox">ListBox</param>
        /// <param name="msg">内容</param>
        public static void AddItemSelected(this ListBox lsbox, string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;
            lsbox.Items.Add(msg);
            lsbox.SelectedIndex = lsbox.Items.Count - 1;
        }

        /// <summary>
        ///     ITEM上移动，适合不是datasource绑定情况
        /// </summary>
        /// <param name="lsbox">ListBox</param>
        public static void ItemMoveUp(this ListBox lsbox)
        {
            var itemCnt = lsbox.Items.Count;
            var selectedIndex = lsbox.SelectedIndex;
            if (itemCnt > selectedIndex && selectedIndex > 0)
            {
                var selectedItem = lsbox.SelectedItem;
                lsbox.Items.RemoveAt(selectedIndex);
                lsbox.Items.Insert(selectedIndex - 1, selectedItem);
                lsbox.SelectedIndex = selectedIndex - 1;
            }
        }

        #endregion Methods
    }
}