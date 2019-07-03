using System;
using Topshelf;
using Topshelf.HostConfigurators;

namespace MasterChief.DotNet.Infrastructure.WindowsService
{
    /// <summary>
    ///     服务配置
    /// </summary>
    public sealed class ServiceConfigure<T> where T : IService, new()
    {
        /// <summary>
        ///     运行服务
        /// </summary>
        /// <param name="args">参数</param>
        public static void Run(string[] args = null)
        {
            var rc = HostFactory.Run(host =>
            {
                var businessService = new T();
                host.Service<IService>(service =>
                {
                    service.ConstructUsing(() => businessService);
                    service.WhenStarted(s => s.Start(args));
                    service.WhenStopped(s => s.Stop());
                    service.WhenPaused(s => s.Paused());
                    service.WhenContinued(s => s.Continued());
                });
                SetRunAs(host, businessService);
                //   host.EnableServiceRecovery(service => { service.RestartService(); });
                if (!string.IsNullOrEmpty(businessService.Description))
                    host.SetDescription(businessService.Description);
                if (!string.IsNullOrEmpty(businessService.DisplayName))
                    host.SetDisplayName(businessService.DisplayName);
                host.SetServiceName(businessService.ServiceName);
                SetStartPattern(host, businessService);
            });

            var exitCode = (int) Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

        private static void SetStartPattern(HostConfigurator host, T businessService)
        {
            switch (businessService.StartPattern)
            {
                case ServiceStartPattern.Automatically:
                    host.StartAutomatically();
                    break;
                case ServiceStartPattern.AutomaticallyDelayed:
                    host.StartAutomaticallyDelayed();
                    break;
                case ServiceStartPattern.Manually:
                    host.StartManually();
                    break;
            }
        }

        private static void SetRunAs(HostConfigurator host, T businessService)
        {
            switch (businessService.RunAs)
            {
                case ServiceRunAs.LocalService:
                    host.RunAsLocalService();
                    break;
                case ServiceRunAs.LocalSystem:
                    host.RunAsLocalSystem();
                    break;
                case ServiceRunAs.NetworkService:
                    host.RunAsNetworkService();
                    break;
                case ServiceRunAs.Prompt:
                    host.RunAsPrompt();
                    break;
            }
        }
    }
}