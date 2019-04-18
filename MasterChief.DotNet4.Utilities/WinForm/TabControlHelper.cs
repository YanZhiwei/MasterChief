using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     TabControl 帮助类
    /// </summary>
    public class TabControlHelper
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TabControlHelper" /> class.
        /// </summary>
        /// <param name="tabControl">The tab control.</param>
        public TabControlHelper(TabControl tabControl)
        {
            TabPageList = new List<TabPage>();
            _tabControl = tabControl;
            foreach (TabPage tp in tabControl.TabPages) TabPageList.Add(tp);
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     TabPge集合
        /// </summary>
        public readonly List<TabPage> TabPageList;

        /// <summary>
        ///     TabControl对象
        /// </summary>
        private readonly TabControl _tabControl;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     显示TabPage
        /// </summary>
        /// <param name="tabpage">需要显示的TabPage</param>
        public void Show(TabPage tabpage)
        {
            if (tabpage != null)
            {
                _tabControl.TabPages.Clear();
                _tabControl.TabPages.Add(tabpage);
            }
        }

        /// <summary>
        ///     显示TabPage
        /// </summary>
        /// <param name="showTabpageHanlder">委托</param>
        public void Show(Predicate<TabPage> showTabpageHanlder)
        {
            TabPage finded = null;
            foreach (var tp in TabPageList)
                if (showTabpageHanlder(tp))
                {
                    finded = tp;
                    break;
                }

            Show(finded);
        }

        /// <summary>
        ///     显示TabPage
        /// </summary>
        /// <param name="tabpage">需要显示的TabPage集合</param>
        public void Show(params TabPage[] tabpage)
        {
            _tabControl.TabPages.Clear();
            _tabControl.TabPages.AddRange(tabpage);
        }

        #endregion Methods
    }
}