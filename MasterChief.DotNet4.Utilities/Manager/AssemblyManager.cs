namespace MasterChief.DotNet4.Utilities.Manager
{
    using System;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Assembly 帮助类
    /// </summary>
    public class AssemblyManager
    {
        #region Fields

        /// <summary>
        /// Assembly对象
        /// </summary>
        private Assembly _assembly = null;

        /// <summary>
        /// 程序集路径
        /// </summary>
        private string _filePath = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AssemblyManager()
        {
            _assembly = Assembly.GetExecutingAssembly();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyManager"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public AssemblyManager(string path)
        {
            ValidateManager.Begin().NotNullOrEmpty(path, "Exe或DLL文件")
            .IsFilePath(path)
            .CheckFileExists(path);
            _filePath = path;
            _assembly = Assembly.LoadFile(path);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取程序集显示名称
        /// </summary>
        /// <returns>程序集显示名称</returns>
        public string GetAppFullName()
        {
            return _assembly.FullName.ToString();
        }

        /// <summary>
        /// 获取编译日期
        /// </summary>
        /// <returns>编译日期</returns>
        public DateTime GetBuildDateTime()
        {
            int peHeaderOffset = 60,
                linkerTimestampOffset = 8;
            using(Stream stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[2048];
                stream.Read(buffer, 0, 2048);
                int position = BitConverter.ToInt32(buffer, peHeaderOffset);
                int since1970 = BitConverter.ToInt32(buffer, position + linkerTimestampOffset);
                DateTime builderDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                builderDate = builderDate.AddSeconds(since1970);
                builderDate = builderDate.ToLocalTime();
                return builderDate;
            }
        }

        /// <summary>
        /// 获取根据实际编译版本信息
        /// </summary>
        /// <returns>实际编译版本信息</returns>
        public DateTime GetBuildDateTimeByVersion()
        {
            Version version = _assembly.GetName().Version;
            return new DateTime(2000, 01, 01).AddDays(version.Build).AddSeconds(version.Revision * 2);
        }

        /// <summary>
        /// 获取公司名称信息
        /// </summary>
        /// <returns>公司名称信息</returns>
        public string GetCompany()
        {
            string company = string.Empty;
            GetAssemblyCommon<AssemblyCompanyAttribute>(_ass => company = _ass.Company);
            return company;
        }

        /// <summary>
        /// 获取版权信息
        /// </summary>
        /// <returns>版权信息</returns>
        public string GetCopyright()
        {
            string copyright = string.Empty;
            GetAssemblyCommon<AssemblyCopyrightAttribute>(_ass => copyright = _ass.Copyright);
            return copyright;
        }

        /// <summary>
        /// 获取说明信息
        /// </summary>
        /// <returns>说明信息</returns>
        public string GetDescription()
        {
            string description = string.Empty;
            GetAssemblyCommon<AssemblyDescriptionAttribute>(_ass => description = _ass.Description);
            return description;
        }

        /// <summary>
        /// 获取产品名称信息
        /// </summary>
        /// <returns>产品名称信息</returns>
        public string GetProductName()
        {
            string product = string.Empty;
            GetAssemblyCommon<AssemblyProductAttribute>(_ass => product = _ass.Product);
            return product;
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <returns>文件名</returns>
        public string GetTitle()
        {
            string title = string.Empty;
            GetAssemblyCommon<AssemblyTitleAttribute>(_ass => title = _ass.Title);

            if(string.IsNullOrEmpty(title))
            {
                title = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }

            return title;
        }

        /// <summary>
        /// 获取主版本号，次版本号；
        /// </summary>
        /// <returns>主版本号，次版本号</returns>
        public string GetVersion()
        {
            return _assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// 获取程序集信息
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="assemblyFacotry">参数委托</param>
        /// 时间：2015-09-09 15:56
        /// 备注：
        private void GetAssemblyCommon<T>(Action<T> assemblyFacotry)
        where T : Attribute
        {
            if(_assembly != null)
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(T), false);

                if(attributes.Length > 0)
                {
                    T attr= (T)attributes[0];
                    assemblyFacotry(attr);
                }
            }
        }

        #endregion Methods
    }
}