namespace MasterChief.DotNet.Infrastructure.WindowsService
{
    /// <summary>
    ///     Windows 服务接口
    /// </summary>
    public interface IService
    {
        /// <summary>
        ///     服务说明
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     服务显示名称
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        ///     服务名称
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        ///     服务启动类型
        /// </summary>
        ServiceStartPattern StartPattern { get; }

        /// <summary>
        ///     服务运行角色类型
        /// </summary>
        ServiceRunAs RunAs { get; }

        /// <summary>
        ///     启动服务
        /// </summary>
        /// <param name="args">参数</param>
        void Start(string[] args);

        /// <summary>
        ///     停止服务
        /// </summary>
        void Stop();

        /// <summary>
        ///  服务暂停
        /// </summary>
        void Paused();

    }
}