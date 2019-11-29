using System.Data;
using System.IO;
using System.Text;
using MasterChief.DotNet4.Utilities.Models;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     CSV 帮助类
    /// </summary>
    public static class CsvHelper
    {
        #region Methods

        /// <summary>
        ///     导出到csv文件
        ///     eg:
        ///     CSVHelper.ToCSV(_personInfoView, @"C:\Users\YanZh_000\Downloads\person.csv", "用户信息表", "名称,年龄");
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="filePath">导出路径</param>
        /// <param name="tableheader">标题</param>
        /// <param name="columname">列名称，以','英文逗号分隔</param>
        /// <returns>是否导出成功</returns>
        public static bool ToCsv(this DataTable table, string filePath, string tableheader, string columname)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        writer.WriteLine(tableheader);
                        writer.WriteLine(columname);

                        for (var i = 0; i < table.Rows.Count; i++)
                        {
                            for (var j = 0; j < table.Columns.Count; j++)
                            {
                                writer.Write(table.Rows[i][j].ToStringOrDefault(string.Empty));
                                writer.Write(",");
                            }

                            writer.WriteLine();
                        }

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     将CSV文件导出为DataTable
        /// </summary>
        /// <param name="filePath">CSV文件</param>
        /// <param name="encoding">Encoding</param>
        /// <param name="startRowIndex">起始行索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ToTable(string filePath, Encoding encoding, ushort startRowIndex)
        {
            var table = new DataTable();
            using (var stream = new StreamReader(filePath, encoding))
            {
                var rowIndex = 0;
                var csvRow = new CsvRow
                {
                    RowText = stream.ReadLine()
                };
                while (CheckCsvRowText(csvRow))
                {
                    if (rowIndex == startRowIndex)
                    {
                        foreach (var item in csvRow) table.Columns.Add(item.Replace("\"", ""));
                    }
                    else if (rowIndex > startRowIndex)
                    {
                        var index = 0;
                        var row = table.NewRow();
                        foreach (var item in csvRow)
                        {
                            row[index] = item.Replace("\"", "");
                            index++;
                        }

                        table.Rows.Add(row);
                    }

                    rowIndex++;
                    csvRow = new CsvRow
                    {
                        RowText = stream.ReadLine()
                    };
                }
            }

            return table;
        }

        private static bool CheckCsvRowText(CsvRow row)
        {
            if (string.IsNullOrEmpty(row.RowText)) return false;

            var offset = 0;
            var rowCount = 0;

            while (offset < row.RowText.Length)
            {
                string tmpText;
                if (row.RowText[offset] == '"')
                {
                    offset++;

                    var start = offset;
                    while (offset < row.RowText.Length)
                    {
                        if (row.RowText[offset] == '"')
                        {
                            offset++;

                            if (offset >= row.RowText.Length || row.RowText[offset] != '"')
                            {
                                offset--;
                                break;
                            }
                        }

                        offset++;
                    }

                    tmpText = row.RowText.Substring(start, offset - start);
                    tmpText = tmpText.Replace("\"\"", "\"");
                }
                else
                {
                    var start = offset;
                    while (offset < row.RowText.Length && row.RowText[offset] != ',') offset++;

                    tmpText = row.RowText.Substring(start, offset - start);
                }

                if (rowCount < row.Count)
                    row[rowCount] = tmpText;
                else
                    row.Add(tmpText);
                rowCount++;

                while (offset < row.RowText.Length && row.RowText[offset] != ',') offset++;
                if (offset < row.RowText.Length) offset++;
            }

            while (row.Count > rowCount) row.RemoveAt(rowCount);
            return row.Count > 0;
        }

        #endregion Methods
    }
}