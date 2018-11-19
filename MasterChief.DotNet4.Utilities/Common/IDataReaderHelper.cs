namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Data;

    /// <summary>
    /// IDataReader 帮助类
    /// </summary>
    /// 日期：2015-09-23 16:05
    /// 备注：
    public static class IDataReaderHelper
    {
        #region Methods

        /// <summary>
        /// 从IDataReader获取值
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
            bool result = reader[columnName] != DBNull.Value;
            return result ? (T)reader[columnName] : failValue;
        }

        /// <summary>
        /// 从IDataReader获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <param name="columnName">列名称</param>
        /// <returns>数值</returns>
        /// 时间：2016-01-04 16:52
        /// 备注：
        public static T GetValueOrDefault<T>(this IDataReader reader, string columnName)
        {
            return GetValueOrDefault<T>(reader, columnName, default(T));
        }

        /// <summary>
        /// 从IDataReader获取值
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
            T result = defaultValue;
            object dbColValue = reader[columnName];

            if (!(dbColValue is DBNull))
            {
                Type dbColType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                result = (T)Convert.ChangeType(dbColValue, dbColType);
            }

            return result;
        }

        #endregion Methods
    }
}