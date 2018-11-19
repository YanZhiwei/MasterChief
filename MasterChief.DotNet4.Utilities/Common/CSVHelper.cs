using MasterChief.DotNet4.Utilities.Model;
using System.Data;
using System.IO;
using System.Text;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// CSV 帮助类
    /// </summary>
    public static class CSVHelper
    {
        #region Methods

        /// <summary>
        /// 导出到csv文件
        /// eg:
        /// CSVHelper.ToCSV(_personInfoView, @"C:\Users\YanZh_000\Downloads\person.csv", "用户信息表", "名称,年龄");
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="filePath">导出路径</param>
        /// <param name="tableheader">标题</param>
        /// <param name="columname">列名称，以','英文逗号分隔</param>
        /// <returns>是否导出成功</returns>
        public static bool ToCSV(this DataTable table, string filePath, string tableheader, string columname)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        writer.WriteLine(tableheader);
                        writer.WriteLine(columname);

                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
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
        /// 将CSV文件导出为DataTable
        /// </summary>
        /// <param name="filePath">CSV文件</param>
        /// <param name="encoding">Encoding</param>
        /// <param name="startRowIndex">起始行索引</param>
        /// <returns>DataTable</returns>
        public static DataTable ToTable(string filePath, Encoding encoding, ushort startRowIndex)
        {
            DataTable table = new DataTable();
            using (StreamReader stream = new StreamReader(filePath, encoding))
            {
                int rowIndex = 0;
                CsvRow csvRow = new CsvRow
                {
                    RowText = stream.ReadLine()
                };
                while (CheckCSVRowText(csvRow))
                {
                    if (rowIndex == startRowIndex)
                    {
                        foreach (string item in csvRow)
                        {
                            table.Columns.Add(item.Replace("\"", ""));
                        }
                    }
                    else if (rowIndex > startRowIndex)
                    {
                        int index = 0;
                        DataRow row = table.NewRow();
                        foreach (string item in csvRow)
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

        private static bool CheckCSVRowText(CsvRow row)
        {
            if (string.IsNullOrEmpty(row.RowText))
            {
                return false;
            }

            int offset = 0;
            int rowCount = 0;

            while (offset < row.RowText.Length)
            {
                string tmpText = string.Empty;
                if (row.RowText[offset] == '"')
                {
                    offset++;

                    int start = offset;
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
                    int start = offset;
                    while (offset < row.RowText.Length && row.RowText[offset] != ',')
                    {
                        offset++;
                    }

                    tmpText = row.RowText.Substring(start, offset - start);
                }
                if (rowCount < row.Count)
                {
                    row[rowCount] = tmpText;
                }
                else
                {
                    row.Add(tmpText);
                }
                rowCount++;

                while (offset < row.RowText.Length && row.RowText[offset] != ',')
                {
                    offset++;
                }
                if (offset < row.RowText.Length)
                {
                    offset++;
                }
            }

            while (row.Count > rowCount)
            {
                row.RemoveAt(rowCount);
            }
            return row.Count > 0;
        }

        #endregion Methods
    }
}