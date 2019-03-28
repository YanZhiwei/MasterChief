using System;
using System.Data;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     IDataReader 帮助类
    /// </summary>
    /// 日期：2015-09-23 16:05
    /// 备注：
    public static class DataReaderHelper
    {
        #region Methods

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <typeparam name="T">返回值泛型</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <param name="failValue">如数值等于DBNull.Value时候返回的值</param>
        /// <returns>数值</returns>
        /// 日期：2015-09-23 16:03
        /// 备注：
        public static T GetValue<T>(this IDataReader reader, string columnName, T failValue)
        {
            var result = reader[columnName] != DBNull.Value;
            return result ? (T) reader[columnName] : failValue;
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>数值</returns>
        /// 时间：2016-01-04 16:52
        /// 备注：
        public static T GetValueOrDefault<T>(this IDataReader reader, string columnName)
        {
            return GetValueOrDefault(reader, columnName, default(T));
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>数值</returns>
        /// 时间：2016-01-04 16:52
        /// 备注：
        public static T GetValueOrDefault<T>(this IDataReader reader, string columnName, T defaultValue)
        {
            var result = defaultValue;
            var dbColValue = reader[columnName];

            if (!(dbColValue is DBNull))
            {
                var dbColType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                result = (T) Convert.ChangeType(dbColValue, dbColType);
            }

            return result;
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:54
        /// 备注：
        public static bool? GetNullableBool(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (bool?) null : reader.GetBoolean(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:54
        /// 备注：
        public static byte? GetNullableByte(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (byte?) null : reader.GetByte(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:54
        /// 备注：
        public static DateTime? GetNullableDateTime(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (DateTime?) null : reader.GetDateTime(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:54
        /// 备注：
        public static double? GetNullableDouble(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (double?) null : reader.GetDouble(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:54
        /// 备注：
        public static float? GetNullableFloat(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (float?) null : reader.GetFloat(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:54
        /// 备注：
        public static short? GetNullableInt16(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (short?) null : reader.GetInt16(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:54
        /// 备注：
        public static int? GetNullableInt32(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (int?) null : reader.GetInt32(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>数值</returns>
        /// 时间：2016-01-05 16:51
        /// 备注：
        public static string GetNullableString(this IDataRecord reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        /// <summary>
        ///     从IDataReader获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>T?</returns>
        /// 时间：2016-01-05 16:31
        /// 备注：
        public static T? GetValueOrNull<T>(IDataReader reader, string columnName)
            where T : struct
        {
            T? value = null;
            var ordinal = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(ordinal))
                value = (T) reader[columnName];
            return value;
        }

        #endregion Methods
    }
}