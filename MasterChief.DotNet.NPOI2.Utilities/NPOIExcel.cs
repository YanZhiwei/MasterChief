namespace MasterChief.DotNet.NPOI2.Utilities
{
    using MasterChief.DotNet4.Utilities.Operator;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using System.Data;
    using System.IO;

    /// <summary>
    /// NPOIExcel 操作辅助类
    /// </summary>
    /// 时间：2016/9/9 11:49
    /// 备注：
    public class NPOIExcel
    {
        #region Methods

        /// <summary>
        /// 将EXCEL文件导入到DataTable
        /// </summary>
        /// <param name="filePath">EXCEL路径</param>
        /// <param name="sheetIndex">Sheet索引</param>
        /// <param name="headIndex">列索引</param>
        /// <param name="rowIndex">行起始索引</param>
        /// <returns>DataTable</returns>
        /// 时间：2016/10/11 17:07
        /// 备注：
        public static DataTable ToDataTable(string filePath, ushort sheetIndex, ushort headIndex, ushort rowIndex)
        {
            DataTable excelTable = new DataTable();
            IWorkbook workbook = NOPIHelper.GetExcelWorkbook(filePath);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            AddDataColumn(sheet, headIndex, excelTable);
            bool supportFormula = string.Compare(Path.GetExtension(filePath), ".xlsx", true) == 0;

            for (int i = (sheet.FirstRowNum + rowIndex); i <= sheet.LastRowNum; i++)
            {
                IRow excelRow = sheet.GetRow(i);

                if (excelRow == null)
                {
                    continue;
                }

                object[] itemArray = AddDataRow(workbook, excelRow, supportFormula);

                excelTable.Rows.Add(itemArray);
            }

            return excelTable;
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="table">需要导出的DataTable</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="title">标题</param>
        /// <param name="filePath">Excel路径</param>
        /// 时间:2016/10/15 21:35
        /// 备注:
        public static void ToExcel(DataTable table, string sheetName, string title, string filePath)
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
                IRow excelRow = sheet.CreateRow(0);
                excelRow.CreateCell(0).SetCellValue(title);
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
                excelRow.Height = 500;
                ICellStyle cellStyle = workBook.CreateCellStyle();
                IFont cellfont = workBook.CreateFont();
                cellfont.FontName = "微软雅黑";
                cellfont.FontHeightInPoints = 17;
                cellStyle.SetFont(cellfont);
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.Alignment = HorizontalAlignment.Center;
                excelRow.Cells[0].CellStyle = cellStyle;
                //处理表格列头
                excelRow = sheet.CreateRow(1);

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    excelRow.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
                    excelRow.Height = 350;
                    sheet.AutoSizeColumn(i);
                }

                //处理数据内容
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    excelRow = sheet.CreateRow(2 + i);
                    excelRow.Height = 250;

                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        excelRow.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                        sheet.SetColumnWidth(j, 256 * 15);
                    }
                }

                //写入数据流
                workBook.Write(fileStream);
            }
        }

        private static void AddDataColumn(ISheet sheet, ushort headIndex, DataTable table)
        {
            IRow excelHeader = sheet.GetRow(headIndex);

            if (excelHeader != null)
            {
                int colCount = excelHeader.LastCellNum;

                for (int i = 0; i < colCount; i++)
                {
                    table.Columns.Add(excelHeader.GetCell(i).StringCellValue);
                }
            }
        }

        private static object[] AddDataRow(IWorkbook workbook, IRow excelRow, bool supportFormula)
        {
            object[] itemArray = new object[excelRow.LastCellNum];
            for (int j = excelRow.FirstCellNum; j < excelRow.LastCellNum; j++)
            {
                if (excelRow.GetCell(j) == null)
                {
                    continue;
                }

                switch (excelRow.GetCell(j).CellType)
                {
                    case CellType.Numeric:
                        itemArray[j] = GetNumericCell(excelRow, j);

                        break;

                    case CellType.Blank:
                        itemArray[j] = string.Empty;
                        break;

                    case CellType.Formula:
                        itemArray[j] = GetFormulaCell(supportFormula, workbook, excelRow, j);

                        break;

                    default:
                        itemArray[j] = excelRow.GetCell(j).StringCellValue;
                        break;
                }
            }
            return itemArray;
        }

        private static bool CheckEmptyRow(object v)
        {
            if (v != null && !string.IsNullOrEmpty(v.ToString().Trim()))
            {
                return false;
            }
            return true;
        }

        private static object GetFormulaCell(bool supportFormula, IWorkbook workbook, IRow excelRow, int j)
        {
            IFormulaEvaluator evaluator = null;

            if (supportFormula)
            {
                evaluator = new XSSFFormulaEvaluator(workbook);
            }
            else
            {
                evaluator = new HSSFFormulaEvaluator(workbook);
            }

            if (evaluator.Evaluate(excelRow.GetCell(j)).CellType == CellType.Numeric)
            {
                return evaluator.Evaluate(excelRow.GetCell(j)).NumberValue;
            }
            else
            {
                return evaluator.Evaluate(excelRow.GetCell(j)).StringValue;
            }
        }

        private static object GetNumericCell(IRow excelRow, int j)
        {
            if (DateUtil.IsCellDateFormatted(excelRow.GetCell(j)))                  //日期类型
            {
                return excelRow.GetCell(j).DateCellValue;
            }
            else//其他数字类型
            {
                return excelRow.GetCell(j).NumericCellValue;
            }
        }

        #endregion Methods
    }
}