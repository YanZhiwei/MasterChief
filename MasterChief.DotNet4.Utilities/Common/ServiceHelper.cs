using System;
using System.Collections;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Windows Service辅助类
    /// </summary>
    public sealed class ServiceHelper
    {
        /// <summary>
        ///     判断服务是否存在
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>是否存在</returns>
        public static bool IsExisted(string serviceName)
        {
            ValidateOperator.Begin().NotNullOrEmpty(serviceName, "服务名称");
            var services = ServiceController.GetServices();
            return services.Count(c =>
                       string.Compare(c.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase) == 0) > 0;
        }

        /// <summary>
        ///     安装服务
        /// </summary>
        /// <param name="serviceFilePath">服务路径</param>
        public static void Install(string serviceFilePath)
        {
            ValidateOperator.Begin().CheckFileExists(serviceFilePath);
            using (var installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        /// <summary>
        ///     卸载服务
        /// </summary>
        /// <param name="serviceFilePath">服务路径</param>
        public static void Uninstall(string serviceFilePath)
        {
            ValidateOperator.Begin().CheckFileExists(serviceFilePath);
            using (var installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                installer.Uninstall(null);
            }
        }

        /// <summary>
        ///     启动服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Start(string serviceName)
        {
            ValidateOperator.Begin().NotNullOrEmpty(serviceName, "服务名称");
            using (var control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped) control.Start();
            }
        }

        /// <summary>
        ///     停止服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Stop(string serviceName)
        {
            ValidateOperator.Begin().NotNullOrEmpty(serviceName, "服务名称");
            using (var control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running) control.Stop();
            }
        }
    }
}