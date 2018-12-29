namespace MasterChief.DotNet4.Utilities.DbManager
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;
    using System.IO;
    using System.Text;

    /// <summary>
    /// EXCEL 操作帮助类
    /// </summary>
    public class ExcelIDbManager
    {
        #region Fields

        private static readonly string _xls = ".xls";
        private static readonly string _xlsx = ".xlsx";

        private readonly string _excelConnectString = string.Empty;
        private readonly string _excelExt = string.Empty; //后缀
        private readonly string _excelPath = string.Empty; //路径
        private readonly bool _x64Version = false;

        #endregion Fields

        #region Constructors

        //链接字符串
        //是否强制使用x64链接字符串，即xlsx形式
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="excelPath">EXCEL路径</param>
        /// <param name="x64Version">是否是64位操作系统</param>
        public ExcelIDbManager(string excelPath, bool x64Version)
        {
            string excelExtension = Path.GetExtension(excelPath);
            _excelExt = excelExtension.ToLower();
            _excelPath = excelPath;
            _x64Version = x64Version;
            _excelConnectString = BuilderConnectionString();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取excel所有sheet数据
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet()
        {
            DataSet excelDb = null;
            using (OleDbConnection sqlcon = new OleDbConnection(_excelConnectString))
            {
                try
                {
                    sqlcon.Open();
                    DataTable schemaTable = sqlcon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (schemaTable != null)
                    {
                        int i = 0;
                        excelDb = new DataSet();

                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string sheetName = row["TABLE_NAME"].ToString().Trim();
                            string sql = string.Format("select * from [{0}]", sheetName);
                            using (OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                            {
                                using (OleDbDataAdapter sqldap = new OleDbDataAdapter(sqlcmd))
                                {
                                    DataTable _dtResult = new DataTable
                                    {
                                        TableName = sheetName
                                    };
                                    sqldap.Fill(_dtResult);
                                    excelDb.Tables.Add(_dtResult);
                                }
                            }
                            i++;
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
        /// 读取sheet
        ///<para> eg:select * from [Sheet1$]</para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql)
        {
            using (OleDbConnection sqlcon = new OleDbConnection(_excelConnectString))
            {
                using (OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    using (OleDbDataAdapter sqldap = new OleDbDataAdapter(sqlcmd))
                    {
                        DataTable table = new DataTable();
                        sqldap.Fill(table);
                        return table;
                    }
                }
            }
        }

        /// <summary>
        /// Excel操作_添加，修改
        /// DELETE不支持
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            int affectedRows = -1;
            using (OleDbConnection sqlcon = new OleDbConnection(_excelConnectString))
            {
                sqlcon.Open();
                using (OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    affectedRows = sqlcmd.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// 获取EXCEL内sheet集合
        /// </summary>
        /// <returns>sheet集合</returns>
        public string[] GetExcelSheetNames()
        {
            DataTable schemaTable = null;
            using (OleDbConnection sqlcon = new OleDbConnection(_excelConnectString))
            {
                sqlcon.Open();
                schemaTable = sqlcon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string[] excelSheets = new string[schemaTable.Rows.Count];
                int i = 0;

                foreach (DataRow row in schemaTable.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString().Trim();
                    i++;
                }

                return excelSheets;
            }
        }

        /// <summary>
        /// 创建链接字符串
        /// </summary>
        /// <returns></returns>
        private string BuilderConnectionString()
        {
            Dictionary<string, string> connectionDict = new Dictionary<string, string>();

            if (!_excelExt.Equals(_xlsx) && !_excelExt.Equals(_xls))
            {
                throw new ArgumentException("excelPath");
            }

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
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<string, string> keyValue in connectionDict)
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