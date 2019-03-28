using System;
using System.Data.Common;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     DbParameter帮助类
    /// </summary>
    /// 时间：2016-01-29 17:11
    /// 备注：
    public static class DbParameterHelper
    {
        /// <summary>
        ///     处理当参数值等于NULL的时候引发的bug，
        ///     赋值等于DBNull.Value
        /// </summary>
        /// <param name="paramter">DbParameter</param>
        /// <returns>DbParameter</returns>
        /// 时间：2016-01-29 17:11
        /// 备注：
        public static DbParameter SetDbNull(this DbParameter paramter)
        {
            if (paramter != null)
                if (paramter.Value == null)
                    paramter.Value = DBNull.Value;

            return paramter;
        }
    }
}