using System;
using System.Data;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     DataTable 帮助类
    /// </summary>
    /// 时间：2016-01-05 13:13
    /// 备注：
    public static class DataTableHelper
    {
        #region Methods

        /// <summary>
        ///     判断DataTable是否是NULL或者Row行数等于零
        /// </summary>
        /// <param name="datatable">DataTable</param>
        /// <returns>否是NULL或者Row行数等于零</returns>
        /// 时间：2016-01-05 13:15
        /// 备注：
        public static bool IsNullOrEmpty(this DataTable datatable)
        {
            return datatable == null || datatable.Rows.Count == 0;
        }

        /// <summary>
        ///     创建Datatable，规范：列名|列类型,列名|列类型,列名|列类型
        ///     <para>举例：CustomeName|string,Gender|bool,Address</para>
        /// </summary>
        /// <param name="columnString">创建表的字符串规则信息</param>
        /// <returns>DataTable</returns>
        public static DataTable CreateTable(string columnString)
        {
            var table = new DataTable();
            var columnsArray = columnString.Split(',');
            string colName;
            string colType;
            string[] colKeyValue;

            foreach (var item in columnsArray)
            {
                colKeyValue = item.Split('|');
                if (colKeyValue.Length == 0) continue;

                colName = colKeyValue[0];
                table.Columns.Add(new DataColumn(colName));
                if (colKeyValue.Length == 2)
                {
                    colType = colKeyValue[1];
                    table.Columns.Add(new DataColumn(colName, Type.GetType(ConvertColumnType(colType)) ?? throw new InvalidOperationException()));
                }
            }

            return table;
        }

        /// <summary>
        ///     检查特定列名称是否存在
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="columnsName">需要存在的列名称</param>
        /// <returns>是否全部包含</returns>
        public static bool CheckedColumnsName(this DataTable table, string[] columnsName)
        {
            var result = table != null;

            if (table != null)
                foreach (var item in columnsName)
                    if (!table.Columns.Contains(item))
                    {
                        result = false;
                        break;
                    }

            return result;
        }

        /// <summary>
        ///     转义数据类型
        /// </summary>
        /// <param name="columnType">列类型</param>
        /// <returns>转义后实际数据类型</returns>
        private static string ConvertColumnType(string columnType)
        {
            string colType;

            switch (columnType.ToLower())
            {
                case "int":
                    colType = "System.Int32";
                    break;

                case "string":
                    colType = "System.String";
                    break;

                case "decimal":
                    colType = "System.Decimal";
                    break;

                case "double":
                    colType = "System.Double";
                    break;

                case "dateTime":
                    colType = "System.DateTime";
                    break;

                case "bool":
                    colType = "System.Boolean";
                    break;

                case "image":
                    colType = "System.Byte[]";
                    break;

                case "object":
                    colType = "System.Object";
                    break;

                default:
                    colType = "System.String";
                    break;
            }

            return colType;
        }

        #endregion Methods
    }
}