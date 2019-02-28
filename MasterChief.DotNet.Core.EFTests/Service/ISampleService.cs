namespace MasterChief.DotNet.Core.EFTests.Service
{
    /// <summary>
    /// 测试数据接口
    /// </summary>
    public interface ISampleService
    {
        /// <summary>
        /// Creates the specified samle.
        /// </summary>
        /// <param name="samle">The samle.</param>
        /// <returns></returns>
        bool Create(EFSample samle);
    }
}
