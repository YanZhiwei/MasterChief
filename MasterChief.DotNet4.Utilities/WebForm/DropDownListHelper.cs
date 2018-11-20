namespace MasterChief.DotNet4.Utilities.WebForm
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// DropDownList 帮助类
    /// </summary>
    /// 时间：2015-10-30 8:58
    /// 备注：
    public static class DropDownListHelper
    {
        #region Methods

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="dropdownList">DropDownList</param>
        /// <param name="dataSource">绑定数据集合对象</param>
        /// <param name="textField">显示绑定字段</param>
        /// <param name="valueField">隐示绑定字段</param>
        /// 时间：2015-10-30 9:01
        /// 备注：
        public static void SetDataSource(this DropDownList dropdownList, object dataSource, string textField, string valueField)
        {
            dropdownList.DataSource = dataSource;
            dropdownList.DataTextField = textField;
            dropdownList.DataValueField = valueField;
            dropdownList.DataBind();
        }

        #endregion Methods
    }
}