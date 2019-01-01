namespace MasterChief.DotNet.Infrastructure.Excel
{
    using MasterChief.DotNet4.Utilities.Operator;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using System.Data;
    using System.IO;

    /// <summary>
    /// NPOIM anager.
    /// </summary>
    public class NPOIManager : IExcelManger
    {
        #region Methods

        /// <summary>
        /// Tos the data table.
        /// </summary>
        /// <returns>The data table.</returns>
        /// <param name="filePath">File path.</param>
        /// <param name="sheetIndex">Sheet index.</param>
        /// <param name="headIndex">Head index.</param>
        /// <param name="rowIndex">Row index.</param>
        public DataTable ToDataTable(string filePath, ushort sheetIndex = 0, ushort headIndex = 0, ushort rowIndex = 0)
        {
            DataTable table = new DataTable();
            IWorkbook hssfworkbook = NOPIHelper.GetExcelWorkbook(filePath);
            ISheet sheet = hssfworkbook.GetSheetAt(sheetIndex);
            AddDataColumns(sheet, headIndex, table);
            bool supportFormula = string.Compare(Path.GetExtension(filePath), ".xlsx", true) == 0;

            for (int i = (sheet.FirstRowNum + rowIndex); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                bool emptyRow = true;

                if (row == null)
                {
                    continue;
                }

                object[] itemArray = new object[row.LastCellNum];

                for (int j = row.FirstCellNum; j < row.LastCellNum; j++)
                {
                    if (row.GetCell(j) == null)
                    {
                        continue;
                    }

                    switch (row.GetCell(j).CellType)
                    {
                        case CellType.Numeric:
                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))                  //日期类型
                            {
                                itemArray[j] = row.GetCell(j).DateCellValue;
                            }
                            else//其他数字类型
                            {
                                itemArray[j] = row.GetCell(j).NumericCellValue;
                            }

                            break;

                        case CellType.Blank:
                            itemArray[j] = string.Empty;
                            break;

                        case CellType.Formula:
                            IFormulaEvaluator eva = null;

                            if (supportFormula)
                            {
                                eva = new XSSFFormulaEvaluator(hssfworkbook);
                            }
                            else
                            {
                                eva = new HSSFFormulaEvaluator(hssfworkbook);
                            }

                            if (eva.Evaluate(row.GetCell(j)).CellType == CellType.Numeric)
                            {
                                itemArray[j] = eva.Evaluate(row.GetCell(j)).NumberValue;
                            }
                            else
                            {
                                itemArray[j] = eva.Evaluate(row.GetCell(j)).StringValue;
                            }

                            break;

                        default:
                            itemArray[j] = row.GetCell(j).StringCellValue;
                            break;
                    }

                    if (itemArray[j] != null && !string.IsNullOrEmpty(itemArray[j].ToString().Trim()))
                    {
                        emptyRow = false;
                    }
                }

                if (!emptyRow)
                {
                    table.Rows.Add(itemArray);
                }
            }

            return table;
        }

        /// <summary>
        /// Tos the excel.
        /// </summary>
        /// <param name="table">Table.</param>
        /// <param name="sheetName">Sheet name.</param>
        /// <param name="title">Title.</param>
        /// <param name="filePath">File path.</param>
        public void ToExcel(DataTable table, string sheetName, string title, string filePath)
        {
            ValidateOperator.Begin().NotNull(table, "需要导出到EXCEL数据源")
            .NotNullOrEmpty(title, "EXCEL标题")
            .NotNullOrEmpty(filePath, "EXCEL导出路径")
            .IsFilePath(filePath);

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                IWorkbook workBook = new HSSFWorkbook();
                sheetName = string.IsNullOrEmpty(sheetName) == true ? "sheet1" : sheetName;
                ISheet sheet = workBook.CreateSheet(sheetName);
                //处理表格标题
                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue(title);
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
                row.Height = 500;
                ICellStyle cellStyle = workBook.CreateCellStyle();
                IFont font = workBook.CreateFont();
                font.FontName = "微软雅黑";
                font.FontHeightInPoints = 17;
                cellStyle.SetFont(font);
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.Alignment = HorizontalAlignment.Center;
                row.Cells[0].CellStyle = cellStyle;
                //处理表格列头
                row = sheet.CreateRow(1);

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
                    row.Height = 350;
                    sheet.AutoSizeColumn(i);
                }

                //处理数据内容
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    row = sheet.CreateRow(2 + i);
                    row.Height = 250;

                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        row.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                        sheet.SetColumnWidth(j, 256 * 15);
                    }
                }

                //写入数据流
                workBook.Write(fileStream);
            }
        }

        private static void AddDataColumns(ISheet sheet, ushort headIndex, DataTable table)
        {
            IRow headRow = sheet.GetRow(headIndex);

            if (headRow != null)
            {
                int colCount = headRow.LastCellNum;

                for (int i = 0; i < colCount; i++)
                {
                    table.Columns.Add(headRow.GetCell(i).StringCellValue);
                }
            }
        }

        #endregion Methods
    }
}