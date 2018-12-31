using System.IO;
using MasterChief.DotNet4.Utilities;
using MasterChief.DotNet4.Utilities.Operator;
using NPOI.SS.UserModel;

namespace MasterChief.DotNet.Infrastructure.Excel
{
    /// <summary>
    /// NOPI 操作辅助类
    /// </summary>
    /// 时间：2016/10/17 10:47
    /// 备注：
    public static class NOPIHelper
    {
        #region Methods

        /// <summary>
        /// 获取Excel IWorkbook对象
        /// </summary>
        /// <param name="filePath">Excel路径</param>
        /// <returns>IWorkbook</returns>
        public static IWorkbook GetExcelWorkbook(string filePath)
        {
            ValidateOperator.Begin()
            .NotNull(filePath, "需要导入到EXCEL文件路径")
            .IsFilePath(filePath).CheckFileExists(filePath)
            .CheckedFileExt(Path.GetExtension(filePath), FileExt.Excel);
            IWorkbook hssfworkbook;

            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = WorkbookFactory.Create(file);
            }

            return hssfworkbook;
        }

        /// <summary>
        /// 设置单元格背景色
        /// </summary>
        /// <param name="cellStyle">ICellStyle</param>
        /// <param name="colorRBGValue">颜色RBG数值</param>
        public static void SetCellBackColor(this ICellStyle cellStyle, short colorRBGValue)
        {
            if (cellStyle != null)
            {
                cellStyle.FillForegroundColor = colorRBGValue;
                cellStyle.FillPattern = FillPattern.SolidForeground;
            }
        }

        /// <summary>
        /// 设置单元格包含边框
        /// </summary>
        /// <param name="cellStyle">ICellStyle</param>
        public static void SetCellHasBorder(this ICellStyle cellStyle)
        {
            if (cellStyle != null)
            {
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;
            }
        }

        /// <summary>
        /// 设置单元格文本居中
        /// </summary>
        /// <param name="cellStyle">ICellStyle</param>
        public static void SetCellTextCenter(this ICellStyle cellStyle)
        {
            if (cellStyle != null)
            {
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.Alignment = HorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// 当value大于零的时候才插入值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public static void SetCellValueOnlyThanZero(this ICell cell, int value)
        {
            if (cell == null) return;

            if (value > 0)
            {
                cell.SetCellValue(value);
            }
        }

        /// <summary>
        /// 当value大于零的时候才插入值
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="value"></param>
        public static void SetCellValueOnlyThanZero(this ICell cell, double value)
        {
            if (cell == null) return;

            if (value > 0)
            {
                cell.SetCellValue(value);
            }
        }

        /// <summary>
        /// 样式创建
        /// eg:
        ///private ICellStyle CreateCellStly(HSSFWorkbook _excel)
        ///{
        ///    IFont _font = _excel.CreateFont();
        ///    _font.FontHeightInPoints = 11;
        ///    _font.FontName = "宋体";
        ///    _font.Boldweight = (short)FontBoldWeight.Bold;
        ///    ICellStyle _cellStyle = _excel.CreateCellStyle();
        ///    //_cellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
        ///    //_cellStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
        ///    _cellStyle.SetFont(_font);
        ///    return _cellStyle;
        ///}
        /// 为行设置样式
        /// </summary>
        /// <param name="row">IRow</param>
        /// <param name="cellStyle">ICellStyle</param>
        public static void SetRowStyle(this IRow row, ICellStyle cellStyle)
        {
            if (row != null && cellStyle != null)
            {
                for (int u = row.FirstCellNum; u < row.LastCellNum; u++)
                {
                    ICell cell = row.GetCell(u);

                    if (cell != null)
                        cell.CellStyle = cellStyle;
                }
            }
        }

        #endregion Methods
    }
}
