namespace MasterChief.DotNet4.Utilities.DataOperator
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
    public class ExcelDataOperator
    {
        #region Fields

        private static readonly string xls = ".xls";
        private static readonly string xlsx = ".xlsx";

        private static bool x64Version = false;

        private string excelConnectString = string.Empty;
        private string excelExt = string.Empty; //后缀
        private string excelPath = string.Empty; //路径

        #endregion Fields

        #region Constructors

        //链接字符串
        //是否强制使用x64链接字符串，即xlsx形式
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="excelPath">EXCEL路径</param>
        /// <param name="x64Version">是否是64位操作系统</param>
        public ExcelDataOperator(string excelPath, bool x64Version)
        {
            string _excelExtension = Path.GetExtension(excelPath);
            excelExt = _excelExtension.ToLower();
            this.excelPath = excelPath;
            ExcelDataOperator.x64Version = x64Version;
            excelConnectString = BuilderConnectionString();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取excel所有sheet数据
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet()
        {
            DataSet _excelDb = null;
            using(OleDbConnection sqlcon = new OleDbConnection(excelConnectString))
            {
                try
                {
                    sqlcon.Open();
                    DataTable _schemaTable = sqlcon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if(_schemaTable != null)
                    {
                        int i = 0;
                        _excelDb = new DataSet();

                        foreach(DataRow row in _schemaTable.Rows)
                        {
                            string _sheetName = row["TABLE_NAME"].ToString().Trim();
                            string _sql = string.Format("select * from [{0}]", _sheetName);
                            using(OleDbCommand sqlcmd = new OleDbCommand(_sql, sqlcon))
                            {
                                using(OleDbDataAdapter sqldap = new OleDbDataAdapter(sqlcmd))
                                {
                                    DataTable _dtResult = new DataTable();
                                    _dtResult.TableName = _sheetName;
                                    sqldap.Fill(_dtResult);
                                    _excelDb.Tables.Add(_dtResult);
                                }
                            }
                            i++;
                        }
                    }
                }
                catch(Exception)
                {
                    return null;
                }
            }
            return _excelDb;
        }

        /// <summary>
        /// 读取sheet
        ///<para> eg:select * from [Sheet1$]</para>
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sql)
        {
            using(OleDbConnection sqlcon = new OleDbConnection(excelConnectString))
            {
                using(OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    using(OleDbDataAdapter sqldap = new OleDbDataAdapter(sqlcmd))
                    {
                        DataTable _dtResult = new DataTable();
                        sqldap.Fill(_dtResult);
                        return _dtResult;
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
            int _affectedRows = -1;
            using(OleDbConnection sqlcon = new OleDbConnection(excelConnectString))
            {
                sqlcon.Open();
                using(OleDbCommand sqlcmd = new OleDbCommand(sql, sqlcon))
                {
                    _affectedRows = sqlcmd.ExecuteNonQuery();
                }
            }
            return _affectedRows;
        }

        /// <summary>
        /// 获取EXCEL内sheet集合
        /// </summary>
        /// <returns>sheet集合</returns>
        public string[] GetExcelSheetNames()
        {
            DataTable _schemaTable = null;
            using(OleDbConnection sqlcon = new OleDbConnection(excelConnectString))
            {
                sqlcon.Open();
                _schemaTable = sqlcon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                String[] _excelSheets = new String[_schemaTable.Rows.Count];
                int i = 0;

                foreach(DataRow row in _schemaTable.Rows)
                {
                    _excelSheets[i] = row["TABLE_NAME"].ToString().Trim();
                    i++;
                }

                return _excelSheets;
            }
        }

        /// <summary>
        /// 创建链接字符串
        /// </summary>
        /// <returns></returns>
        private string BuilderConnectionString()
        {
            Dictionary<string, string> _connectionParameter = new Dictionary<string, string>();

            if(!excelExt.Equals(xlsx) && !excelExt.Equals(xls))
            {
                throw new ArgumentException("excelPath");
            }

            if(!x64Version)
            {
                if(excelExt.Equals(xlsx))
                {
                    // XLSX - Excel 2007, 2010, 2012, 2013
                    _connectionParameter["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                    _connectionParameter["Extended Properties"] = "'Excel 12.0 XML;IMEX=1'";
                }
                else if(excelExt.Equals(xls))
                {
                    // XLS - Excel 2003 and Older
                    _connectionParameter["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                    _connectionParameter["Extended Properties"] = "'Excel 8.0;IMEX=1'";
                }
            }
            else
            {
                _connectionParameter["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                _connectionParameter["Extended Properties"] = "'Excel 12.0 XML;IMEX=1'";
            }

            _connectionParameter["Data Source"] = excelPath;
            StringBuilder _connectionString = new StringBuilder();

            foreach(KeyValuePair<string, string> parameter in _connectionParameter)
            {
                _connectionString.Append(parameter.Key);
                _connectionString.Append('=');
                _connectionString.Append(parameter.Value);
                _connectionString.Append(';');
            }

            return _connectionString.ToString();
        }

        #endregion Methods
    }
}