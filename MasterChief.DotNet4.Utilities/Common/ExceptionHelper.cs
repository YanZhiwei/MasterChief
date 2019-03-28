using System;
using System.Text;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Exception帮助类
    /// </summary>
    public static class ExceptionHelper
    {
        #region Methods

        /// <summary>
        ///     格式化异常消息
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="isHideStackTrace">是否显示堆栈信息</param>
        /// <param name="appString">堆栈信息描述前缀；默认空格</param>
        /// <returns>格式化后异常信息</returns>
        public static string FormatMessage(this Exception ex, bool isHideStackTrace, string appString = "  ")
        {
            var builder = new StringBuilder();

            while (ex != null)
            {
                builder.AppendLine($"{appString}异常消息：{ex.Message}");
                builder.AppendLine($"{appString}异常类型：{ex.GetType().FullName}");
                builder.AppendLine($"{appString}异常方法：{(ex.TargetSite == null ? null : ex.TargetSite.Name)}");
                builder.AppendLine($"{appString}异常来源：{ex.Source}");

                if (!isHideStackTrace && ex.StackTrace != null) builder.AppendLine($"{appString}异常堆栈：{ex.StackTrace}");

                if (ex.InnerException != null) builder.AppendLine($"{appString}内部异常：");

                ex = ex.InnerException;
            }

            return builder.ToString();
        }

        /// <summary>
        ///     获取innerException
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>Exception</returns>
        /// 日期：2015-10-20 16:22
        /// 备注：
        public static Exception GetOriginalException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetOriginalException();
        }

        /// <summary>
        ///     判断异常是哪个异常类型
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">Exception</param>
        /// <returns>判断异常类型</returns>
        public static bool Is<T>(this Exception source)
            where T : Exception
        {
            if (source is T)
                return true;
            if (source.InnerException != null)
                return source.InnerException.Is<T>();
            return false;
        }

        #endregion Methods
    }
}