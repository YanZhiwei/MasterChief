using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MasterChief.DotNet4.WindowsAPI.Core
{
    /// <summary>
    ///     Windows Api自定义异常
    /// </summary>
    public class Win32ErrorCodeException : Win32Exception
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="context">异常信息</param>
        public Win32ErrorCodeException(string context)
        {
            var error = Marshal.GetLastWin32Error();
            var innerException = new Win32Exception(error);

            Message = $"{context}: (Error Code {error}) {innerException.Message}";
        }

        /// <summary>
        ///     相信异常信息
        /// </summary>
        public override string Message { get; }
    }
}