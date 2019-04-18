using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    #region Delegates

    /// <summary>
    ///     委托
    /// </summary>
    /// <param name="state"></param>
    public delegate void CheckBoxClickedHandler(bool state);

    #endregion Delegates

    /// <summary>
    ///     勾选列
    /// </summary>
    public class DatagridViewCheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        #region Constructors

        #endregion Constructors

        #region Events

        /// <summary>
        ///     勾选事件
        /// </summary>
        public event CheckBoxClickedHandler OnCheckBoxClicked;

        #endregion Events

        #region Fields

        private CheckBoxState _allCheckedState = CheckBoxState.UncheckedNormal;
        private Point _cellLocation;
        private Point _checkBoxLocation;
        private Size _checkBoxSize;
        private bool _ckstatus;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Raises the <see cref="E:MouseClick" /> event.
        /// </summary>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs" /> instance containing the event data.</param>
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            var point = new Point(e.X + _cellLocation.X, e.Y + _cellLocation.Y);

            if (point.X >= _checkBoxLocation.X && point.X <= _checkBoxLocation.X + _checkBoxSize.Width &&
                point.Y >= _checkBoxLocation.Y && point.Y <= _checkBoxLocation.Y + _checkBoxSize.Height)
            {
                _ckstatus = !_ckstatus;

                if (OnCheckBoxClicked != null)
                {
                    OnCheckBoxClicked(_ckstatus);
                    DataGridView.InvalidateCell(this);
                }
            }

            base.OnMouseClick(e);
        }

        /// <summary>
        ///     Paints the specified graphics.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="clipBounds">The clip bounds.</param>
        /// <param name="cellBounds">The cell bounds.</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="dataGridViewElementState">State of the data grid view element.</param>
        /// <param name="value">The value.</param>
        /// <param name="formattedValue">The formatted value.</param>
        /// <param name="errorText">The error text.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <param name="advancedBorderStyle">The advanced border style.</param>
        /// <param name="paintParts">The paint parts.</param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue,
                errorText, cellStyle, advancedBorderStyle, paintParts);
            var point = new Point();
            var size = CheckBoxRenderer.GetGlyphSize(graphics,
                CheckBoxState.UncheckedNormal);
            point.X = cellBounds.Location.X + cellBounds.Width / 2 - size.Width / 2;
            point.Y = cellBounds.Location.Y + cellBounds.Height / 2 - size.Height / 2;
            _cellLocation = cellBounds.Location;
            _checkBoxLocation = point;
            _checkBoxSize = size;

            if (_ckstatus)
                _allCheckedState = CheckBoxState.CheckedNormal;
            else
                _allCheckedState = CheckBoxState.UncheckedNormal;

            CheckBoxRenderer.DrawCheckBox(graphics, _checkBoxLocation, _allCheckedState);
        }

        #endregion Methods

        #region Other

        /*
         * 参考：
         * 1. http://www.codeproject.com/Articles/20165/CheckBox-Header-Column-For-DataGridView
         */

        #endregion Other
    }
}