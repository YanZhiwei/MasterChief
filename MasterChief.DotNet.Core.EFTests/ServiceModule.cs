using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.EF;
using MasterChief.DotNet.Core.EFTests.Service;
using Ninject.Modules;

namespace MasterChief.DotNet.Core.EFTests
{
    public sealed class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseContextFactory>().To<SampleDbContextFactory>();
       
            Bind(typeof(IRepository<>)).To(typeof(EfRepository<>));

            Bind<ISampleService>().To<SampleService>();
        }
    }
}