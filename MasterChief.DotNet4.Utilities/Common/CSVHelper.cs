namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Data;
    using System.IO;
    using System.Text;

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
                ValidateOperator.Begin().NotNull(table, "需要导出为CSV文件的DataTable").IsFilePath(filePath).NotNull(columname, "列名称");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        streamWriter.WriteLine(tableheader);
                        streamWriter.WriteLine(columname);

                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            for (int j = 0; j < table.Columns.Count; j++)
                            {
                                streamWriter.Write(table.Rows[i][j].ToStringOrDefault(string.Empty));
                                streamWriter.Write(",");
                            }

                            streamWriter.WriteLine();
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
            DataTable _table = new DataTable();
            using (StreamReader stream = new StreamReader(filePath, encoding))
            {
                int _rowIndex = 0;
                CsvRow _csvRow = new CsvRow
                {
                    RowText = stream.ReadLine()
                };
                while (CheckCSVRowText(_csvRow))
                {
                    if (_rowIndex == startRowIndex)
                    {
                        foreach (string item in _csvRow)
                        {
                            _table.Columns.Add(item.Replace("\"", ""));
                        }
                    }
                    else if (_rowIndex > startRowIndex)
                    {
                        int _index = 0;
                        DataRow _row = _table.NewRow();
                        foreach (string item in _csvRow)
                        {
                            _row[_index] = item.Replace("\"", "");
                            _index++;
                        }
                        _table.Rows.Add(_row);
                    }
                    _rowIndex++;
                    _csvRow = new CsvRow
                    {
                        RowText = stream.ReadLine()
                    };
                }
            }

            return _table;
        }

        private static bool CheckCSVRowText(CsvRow row)
        {
            if (string.IsNullOrEmpty(row.RowText))
            {
                return false;
            }

            int _offset = 0;
            int _rowCount = 0;

            while (_offset < row.RowText.Length)
            {
                string _tmpText;

                if (row.RowText[_offset] == '"')
                {
                    _offset++;

                    int _start = _offset;
                    while (_offset < row.RowText.Length)
                    {
                        if (row.RowText[_offset] == '"')
                        {
                            _offset++;

                            if (_offset >= row.RowText.Length || row.RowText[_offset] != '"')
                            {
                                _offset--;
                                break;
                            }
                        }
                        _offset++;
                    }
                    _tmpText = row.RowText.Substring(_start, _offset - _start);
                    _tmpText = _tmpText.Replace("\"\"", "\"");
                }
                else
                {
                    int start = _offset;
                    while (_offset < row.RowText.Length && row.RowText[_offset] != ',')
                    {
                        _offset++;
                    }

                    _tmpText = row.RowText.Substring(start, _offset - start);
                }
                if (_rowCount < row.Count)
                {
                    row[_rowCount] = _tmpText;
                }
                else
                {
                    row.Add(_tmpText);
                }
                _rowCount++;

                while (_offset < row.RowText.Length && row.RowText[_offset] != ',')
                {
                    _offset++;
                }
                if (_offset < row.RowText.Length)
                {
                    _offset++;
                }
            }

            while (row.Count > _rowCount)
            {
                row.RemoveAt(_rowCount);
            }
            return row.Count > 0;
        }

        #endregion Methods
    }
}