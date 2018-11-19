namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    /// <summary>
    /// JSON转换类
    /// </summary>
    public static class JsonHelper
    {
        #region Methods

        /// <summary>
        /// 序列化成JSON字符串
        /// </summary>
        /// <param name="dataSet">DataSet对象</param>
        /// <returns>Json字符串</returns>
        public static string Serialize(DataSet dataSet)
        {
            string jsonText = "{";

            foreach (DataTable table in dataSet.Tables)
            {
                jsonText += "\"" + table.TableName + "\":" + Serialize(table) + ",";
            }

            jsonText = jsonText.TrimEnd(',');
            return jsonText + "}";
        }

        /// <summary>
        /// 序列化成JSON字符串
        /// </summary>
        /// <param name="table">Datatable对象</param>
        /// <returns>Json字符串</returns>
        public static string Serialize(DataTable table)
        {
            StringBuilder jsonText = new StringBuilder();
            jsonText.Append("[");
            DataRowCollection dataRows = table.Rows;

            for (int i = 0; i < dataRows.Count; i++)
            {
                jsonText.Append("{");

                for (int j = 0; j < table.Columns.Count; j++)
                {
                    string _key = table.Columns[j].ColumnName;
                    string _value = dataRows[i][j].ToString();
                    Type _type = table.Columns[j].DataType;
                    jsonText.Append("\"" + _key + "\":");
                    _value = StringFormat(_value, _type);

                    if (j < table.Columns.Count - 1)
                    {
                        jsonText.Append(_value + ",");
                    }
                    else
                    {
                        jsonText.Append(_value);
                    }
                }

                jsonText.Append("},");
            }

            jsonText.Remove(jsonText.Length - 1, 1);
            jsonText.Append("]");
            return jsonText.ToString();
        }

        /// <summary>
        /// 序列化成JSON字符串
        /// </summary>
        public static string Serialize(DataTable dataTable, string jsonName)
        {
            StringBuilder jsonText = new StringBuilder();

            if (string.IsNullOrEmpty(jsonName))
            {
                jsonName = dataTable.TableName;
            }

            jsonText.Append("{\"" + jsonName + "\":[");

            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    jsonText.Append("{");

                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        Type type = dataTable.Rows[i][j].GetType();
                        jsonText.Append("\"" + dataTable.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dataTable.Rows[i][j].ToString(), type));

                        if (j < dataTable.Columns.Count - 1)
                        {
                            jsonText.Append(",");
                        }
                    }

                    jsonText.Append("}");

                    if (i < dataTable.Rows.Count - 1)
                    {
                        jsonText.Append(",");
                    }
                }
            }

            jsonText.Append("]}");
            return jsonText.ToString();
        }

        /// <summary>
        /// 序列化成JSON字符串
        /// </summary>
        /// <param name="dataReader">DataReader对象</param>
        /// <returns>Json字符串</returns>
        public static string Serialize(DbDataReader dataReader)
        {
            StringBuilder jsonText = new StringBuilder();
            jsonText.Append("[");

            while (dataReader.Read())
            {
                jsonText.Append("{");

                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type _type = dataReader.GetFieldType(i);
                    string _key = dataReader.GetName(i);
                    string _value = dataReader[i].ToString();
                    jsonText.Append("\"" + _key + "\":");
                    _value = StringFormat(_value, _type);

                    if (i < dataReader.FieldCount - 1)
                    {
                        jsonText.Append(_value + ",");
                    }
                    else
                    {
                        jsonText.Append(_value);
                    }
                }

                jsonText.Append("},");
            }

            dataReader.Close();
            jsonText.Remove(jsonText.Length - 1, 1);
            jsonText.Append("]");
            return jsonText.ToString();
        }

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        private static string HanlderJsonString(string data)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                char tmp = data.ToCharArray()[i];

                switch (tmp)
                {
                    case '\"':
                        builder.Append("\\\"");
                        break;

                    case '\\':
                        builder.Append("\\\\");
                        break;

                    case '/':
                        builder.Append("\\/");
                        break;

                    case '\b':
                        builder.Append("\\b");
                        break;

                    case '\f':
                        builder.Append("\\f");
                        break;

                    case '\n':
                        builder.Append("\\n");
                        break;

                    case '\r':
                        builder.Append("\\r");
                        break;

                    case '\t':
                        builder.Append("\\t");
                        break;

                    default:
                        builder.Append(tmp);
                        break;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        private static string StringFormat(string data, Type type)
        {
            if (type == typeof(string))
            {
                data = HanlderJsonString(data);
                data = "\"" + data + "\"";
            }
            else if (type == typeof(DateTime))
            {
                data = "\"" + data + "\"";
            }
            else if (type == typeof(bool))
            {
                data = data.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(data))
            {
                data = "\"" + data + "\"";
            }

            return data;
        }

        #endregion Methods
    }
}