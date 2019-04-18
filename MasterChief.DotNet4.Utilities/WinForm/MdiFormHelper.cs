using System;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     MDI 帮助类
    /// </summary>
    public static class MdiFormHelper
    {
        #region Methods

        /// <summary>
        ///     MDI子窗口是否已经弹出存在
        ///     <para>eg:MdiFormHelper.CheckMDIChildrenFormExist(this, "Form2")</para>
        /// </summary>
        /// <param name="parentForm">MDI容器窗口</param>
        /// <param name="formName">>MDI子窗口名称</param>
        /// <returns>是否已经弹出存在</returns>
        public static bool CheckMdiChildrenFormExist(this Form parentForm, string formName)
        {
            for (var i = 0; i < parentForm.MdiChildren.Length; i++)
                if (String.Compare(parentForm.MdiChildren[i].Name, formName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    parentForm.MdiChildren[i].Activate();
                    return true;
                }

            return false;
        }

        /// <summary>
        ///     MDI窗口弹出
        ///     <para>eg:MdiFormHelper.ShowMDIChildrenForm(this, "Form2");</para>
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="parentForm">MDI容器窗口</param>
        /// <param name="formName">MDI子窗口名称</param>
        public static void ShowMdiChildrenForm<T>(this Form parentForm, string formName)
            where T : Form
        {
            var form = Activator.CreateInstance<T>();
            form.Name = formName;
            form.MdiParent = parentForm;
            form.Show();
        }

        #endregion Methods
    }
}