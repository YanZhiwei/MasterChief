namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// Db context factory.
    /// </summary>
    public interface IDbContextFactory
    {
        /// <summary>
        /// Create this instance.
        /// </summary>
        /// <returns>The create.</returns>
        IDbContext Create();
    }
}
