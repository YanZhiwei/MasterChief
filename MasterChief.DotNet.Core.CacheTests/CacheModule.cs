using MasterChief.DotNet.Core.Cache;
using Ninject.Modules;

namespace MasterChief.DotNet.Core.CacheTests
{
    public sealed class CacheModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICacheProvider>().To<LocalCacheProvider>().InSingletonScope();
        }
    }
}