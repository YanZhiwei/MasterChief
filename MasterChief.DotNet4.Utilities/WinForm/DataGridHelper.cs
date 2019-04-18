using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MasterChief.DotNet4.Utilities.Common;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     DataGrid 帮助类
    /// </summary>
    public static class DataGridHelper
    {
        #region Methods

        /// <summary>
        ///     将DateTimePicker应用到列编辑时候
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <param name="datePicker">DateTimePicker</param>
        /// <param name="columnIndex">应用编辑列索引</param>
        public static void ApplyDateTimePicker(this DataGridView dataGrid, DateTimePicker datePicker, int columnIndex)
        {
            datePicker.Visible = false;
            datePicker.ValueChanged += (sender, e) =>
            {
                DateTimePicker cuDateTimePicker = sender as DateTimePicker;
                // ReSharper disable once PossibleNullReferenceException
                dataGrid.CurrentCell.Value = cuDateTimePicker.Value;
                cuDateTimePicker.Visible = false;
            };
            dataGrid.CellClick += (sender, e) =>
            {
                if (e.ColumnIndex == columnIndex)
                {
                    var curDataGridView = sender as DataGridView;
                    // ReSharper disable once PossibleNullReferenceException
                    Rectangle cellRectangle = curDataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    datePicker.Location = cellRectangle.Location;
                    datePicker.Width = cellRectangle.Width;

                    try
                    {
                        datePicker.Value = curDataGridView.CurrentCell.Value.ToDateOrDefault(DateTime.Now);
                    }
                    catch
                    {
                        datePicker.Value = DateTime.Now;
                    }

                    datePicker.Visible = true;
                }
            };
            dataGrid.Controls.Add(datePicker);
        }

        /// <summary>
        ///     清除绑定
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        public static void ClearDynamicBind(this DataGridView dataGrid)
        {
            var bindingSource = new BindingSource();
            bindingSource.DataSource = null;
            dataGrid.DataSource = bindingSource;
        }

        /// <summary>
        ///     获取行数
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <returns>行数</returns>
        public static int GetDynamicBindRowCount(this DataGridView dataGrid)
        {
            if (dataGrid.DataSource is BindingSource)
            {
                var source = (BindingSource) dataGrid.DataSource;
                return source.Count;
            }

            return 0;
        }

        /// <summary>
        ///     添加checkbox 列头
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="headerText">列名称</param>
        public static void ApplyHeaderCheckbox(this DataGridView dataGrid, int columnIndex, string headerText)
        {
            var checkedBox = new DatagridViewCheckBoxHeaderCell();
            dataGrid.Columns[columnIndex].HeaderCell = checkedBox;
            dataGrid.Columns[columnIndex].HeaderText = headerText;
            checkedBox.OnCheckBoxClicked += state =>
            {
                var count = dataGrid.Rows.Count;

                for (var i = 0; i < count; i++)
                {
                    var checkCell = (DataGridViewCheckBoxCell) dataGrid.Rows[i].Cells[columnIndex];
                    checkCell.Value = state;
                }
            };
        }

        /// <summary>
        ///     根据cell内容调整其宽度
        /// </summary>
        /// <param name="girdview">DataGridView</param>
        public static void AutoCellWidth(this DataGridView girdview)
        {
            var columnSumWidth = 0;

            for (var i = 0; i < girdview.Columns.Count; i++)
            {
                girdview.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                columnSumWidth += girdview.Columns[i].Width;
            }

            girdview.AutoSizeColumnsMode = columnSumWidth > girdview.Size.Width
                ? DataGridViewAutoSizeColumnsMode.DisplayedCells
                : DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        ///     绘制行号
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        public static void DrawSequenceNumber(this DataGridView dataGrid)
        {
            dataGrid.RowPostPaint += (sender, e) =>
            {
                var curDataGridView = sender as DataGridView;
                // ReSharper disable once PossibleNullReferenceException
                var rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y,
                    curDataGridView.RowHeadersWidth, e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                    curDataGridView.RowHeadersDefaultCellStyle.Font,
                    rectangle,
                    curDataGridView.RowHeadersDefaultCellStyle.ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            };
        }

        /// <summary>
        ///     DataGridView绑定
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <param name="source">数据源</param>
        public static void DynamicBind<T>(this DataGridView dataGrid, IList<T> source)
            where T : class
        {
            BindingSource bindingSource;
            if (dataGrid.DataSource is BindingSource)
            {
                bindingSource = (BindingSource) dataGrid.DataSource;
                bindingSource.AllowNew = true;

                foreach (var entity in source) bindingSource.Add(entity);
            }
            else
            {
                var bindinglist = new BindingList<T>(source);
                bindingSource = new BindingSource(bindinglist, null);
                dataGrid.DataSource = bindingSource;
            }
        }

        /// <summary>
        ///     DataGridView绑定
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <param name="item">数据源</param>
        public static void DynamicBind<T>(this DataGridView dataGrid, T item)
            where T : class
        {
            BindingSource bindingSource;

            if (dataGrid.DataSource is BindingSource)
            {
                bindingSource = (BindingSource) dataGrid.DataSource;
                bindingSource.AllowNew = true;
                bindingSource.Add(item);
            }
            else
            {
                var dataSource = new List<T>(1) {item};
                var bindinglist = new BindingList<T>(dataSource);
                bindingSource = new BindingSource(bindinglist, null);
                dataGrid.DataSource = bindingSource;
            }
        }

        /// <summary>
        ///     获取选中行
        /// </summary>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <returns>若未有选中行则返回NULL</returns>
        /// 时间：2015-12-09 17:07
        /// 备注：
        public static DataGridViewRow SelectedRow(this DataGridView dataGrid)
        {
            var selectedRows = dataGrid.SelectedRows;

            // ReSharper disable once ConstantConditionalAccessQualifier
            return selectedRows?.Count
                   > 0
                ? selectedRows[0]
                : null;
        }

        /// <summary>
        ///     选中最后一行
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        public static void SelectedLastRow(this DataGridView dataGrid)
        {
            var lastRowIndex = dataGrid.Rows.Count - 1;
            dataGrid.ClearSelection();
            dataGrid.Rows[lastRowIndex].Selected = true;
            dataGrid.FirstDisplayedScrollingRowIndex = lastRowIndex;
        }

        #endregion Methods
    }
}