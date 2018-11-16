namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Reflection;

    /// <summary>
    /// 项目帮助文件
    /// </summary>
    public class ProjectHelper
    {
        #region Methods

        /// <summary>
        /// 获取执行文件夹路径
        /// <para>eg:c:\\users\\yanzhiwei\\documents\\visual studio 2015\\Projects\\WebApplication2\\WebApplication2\\</para>
        /// </summary>
        /// <returns>执行文件夹路径</returns>
        public static string GetExecuteDirectory()
        {
            string _path = string.Empty;
            ProgramMode _mode = GetExecutionContext();

            switch (_mode)
            {
                case ProgramMode.WebForm:
                    _path = AppDomain.CurrentDomain.BaseDirectory.ToString();
                    break;

                case ProgramMode.WinForm:
                    _path = ApplicationHelper.GetExecuteDirectory();
                    break;
            }

            return _path;
        }

        /// <summary>
        /// 获取程序执行上下文
        /// </summary>
        /// <returns>程序执行上下文</returns>
        public static ProgramMode GetExecutionContext()
        {
            if (Assembly.GetEntryAssembly() != null)
            {
                return ProgramMode.WinForm;
            }

            return ProgramMode.WebForm;
        }

        #endregion Methods
    }
}