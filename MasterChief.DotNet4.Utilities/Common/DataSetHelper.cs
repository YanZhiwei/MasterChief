using System.Data;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     DataSet 帮助类
    /// </summary>
    /// 时间：2016-01-05 13:18
    /// 备注：
    public static class DataSetHelper
    {
        #region Methods

        /// <summary>
        ///     判断DataSet是否是NULL或者没有DataTable
        /// </summary>
        /// <param name="dataset">DataSet</param>
        /// <returns>是否是NULL或者没有DataTable</returns>
        /// 时间：2016-01-05 13:19
        /// 备注：
        public static bool IsNullOrEmpty(this DataSet dataset)
        {
            return dataset == null || dataset.Tables.Count == 0;
        }

        #endregion Methods
    }
}