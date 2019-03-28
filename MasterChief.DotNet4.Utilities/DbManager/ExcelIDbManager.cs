using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace MasterChief.DotNet4.Utilities.DbManager
{
    /// <summary>
    ///     EXCEL 操作帮助类
    /// </summary>
    public class ExcelIDbManager
    {
        #region Constructors

        //链接字符串
        //是否强制使用x64链接字符串，即xlsx形式
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="excelPath">EXCEL路径</param>
        /// <param name="x64Version">是否是64位操作系统</param>
        public ExcelIDbManager(string excelPath, bool x64Version)
        {
            var excelExtension = Path.GetExtension(excelPath);
            if (excelExtension != null) _excelExt = excelExtension.ToLower();
            _excelPath = excelPath;
            _x64Version = x64Version;
            _excelConnectString = BuilderConnectionString();
        }

        #endregion Constructors

        #region Fields

        private static readonly string _xls = ".xls";
        private static readonly string _xlsx = ".xlsx";

        private readonly string _excelConnectString;
        private readonly string _excelExt; //后缀
        private readonly string _excelPath; //路径
        private readonly bool _x64Version;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     获取excel所有sheet数据
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet()
        {
            DataSet excelDb = null;
            using (var oleDbConnection = new OleDbConnection(_excelConnectString))
            {
                try
                {
                    oleDbConnection.Open();
                    var schemaTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (schemaTable != null)
                    {
                        excelDb = new DataSet();

                        foreach (DataRow row in schemaTable.Rows)
                        {
                            var sheetName = row["TABLE_NAME"].ToString().Trim();
                            var sql = $"select * from [{sheetName}]";
                            using (var oleDbCommand = new OleDbCommand(sql, oleDbConnection))
                            {
                                using (var oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
                                {
                                    var dtResult = new DataTable
                                    {
                                        TableName = sheetName
                                    };
                                    oleDbDataAdapter.Fill(dtResult);
                                    excelDb.Tables.Add(dtResult);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return excelDb;
        }

        /// <summary>
        ///     读取sheet
        ///     <para> eg:select * from [Sheet1$]</para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql)
        {
            using (var dbConnection = new OleDbConnection(_excelConnectString))
            {
                using (var dbCommand = new OleDbCommand(sql, dbConnection))
                {
                    using (var adapter = new OleDbDataAdapter(dbCommand))
                    {
                        var table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        ///     Excel操作_添加，修改
        ///     DELETE不支持
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            int affectedRows;
            using (var oleDbConnection = new OleDbConnection(_excelConnectString))
            {
                oleDbConnection.Open();
                using (var oleDbCommand = new OleDbCommand(sql, oleDbConnection))
                {
                    affectedRows = oleDbCommand.ExecuteNonQuery();
                }
            }

            return affectedRows;
        }

        /// <summary>
        ///     获取EXCEL内sheet集合
        /// </summary>
        /// <returns>sheet集合</returns>
        public string[] GetExcelSheetNames()
        {
            using (var oleDbConnection = new OleDbConnection(_excelConnectString))
            {
                oleDbConnection.Open();
                var schemaTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable != null)
                {
                    var excelSheets = new string[schemaTable.Rows.Count];
                    var i = 0;

                    foreach (DataRow row in schemaTable.Rows)
                    {
                        excelSheets[i] = row["TABLE_NAME"].ToString().Trim();
                        i++;
                    }

                    return excelSheets;
                }

                return null;
            }
        }

        /// <summary>
        ///     创建链接字符串
        /// </summary>
        /// <returns></returns>
        private string BuilderConnectionString()
        {
            var connectionDict = new Dictionary<string, string>();

            if (!_excelExt.Equals(_xlsx) && !_excelExt.Equals(_xls)) throw new ArgumentException("excelPath");

            if (!_x64Version)
            {
                if (_excelExt.Equals(_xlsx))
                {
                    // XLSX - Excel 2007, 2010, 2012, 2013
                    connectionDict["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                    connectionDict["Extended Properties"] = "'Excel 12.0 XML;IMEX=1'";
                }
                else if (_excelExt.Equals(_xls))
                {
                    // XLS - Excel 2003 and Older
                    connectionDict["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                    connectionDict["Extended Properties"] = "'Excel 8.0;IMEX=1'";
                }
            }
            else
            {
                connectionDict["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                connectionDict["Extended Properties"] = "'Excel 12.0 XML;IMEX=1'";
            }

            connectionDict["Data Source"] = _excelPath;
            var builder = new StringBuilder();

            foreach (var keyValue in connectionDict)
            {
                builder.Append(keyValue.Key);
                builder.Append('=');
                builder.Append(keyValue.Value);
                builder.Append(';');
            }

            return builder.ToString();
        }

        #endregion Methods
    }
}