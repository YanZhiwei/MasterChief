namespace MasterChief.DotNet.Infrastructure.WindowsService
{
    /// <summary>
    ///     服务运行角色设定
    /// </summary>
    public enum ServiceRunAs
    {
        /// <summary>
        ///     The local system
        /// </summary>
        LocalSystem,

        /// <summary>
        ///     The local service
        /// </summary>
        LocalService,

        /// <summary>
        ///     The network service
        /// </summary>
        NetworkService,

        /// <summary>
        ///     The prompt
        /// </summary>
        Prompt,

    }
}