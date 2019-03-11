using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.EFTests.Service;
using Ninject.Modules;

namespace MasterChief.DotNet.Core.EFTests
{
    public sealed class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseContextFactory>().To<SampleDbContextFactory>().InSingletonScope();

            //Bind(typeof(IRepository<>)).To(typeof(EfRepository<>));

            Bind<ISampleService>().To<SampleService>();

            #region 方式二

            //Bind<IDbContext>().To<SampleDbContext>();
            //Bind(typeof(IRepository<>)).To(typeof(EfRepository<>));
            //Bind<ISampleService>().To<Sample2Service>();

            #endregion 方式二
        }
    }
}