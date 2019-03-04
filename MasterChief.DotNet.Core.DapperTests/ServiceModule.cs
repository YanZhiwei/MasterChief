using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.DapperTests.Service;
using Ninject.Modules;

namespace MasterChief.DotNet.Core.DapperTests
{
    public sealed class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseContextFactory>().To<SampleDbContextFactory>();

            Bind<ISampleService>().To<SampleService>();
        }
    }
}