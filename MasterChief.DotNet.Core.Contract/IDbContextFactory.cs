namespace MasterChief.DotNet.Core.Contract
{
    public interface IDbContextFactory
    {
        IDbContext Create();
    }
}
