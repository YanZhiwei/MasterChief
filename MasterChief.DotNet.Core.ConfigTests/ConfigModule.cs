using MasterChief.DotNet.Core.Config;
using Ninject.Modules;

namespace MasterChief.DotNet.Core.ConfigTests
{
    public sealed class ConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigProvider>().To<FileConfigService>().InSingletonScope();
            // Bind<ConfigContext>().ToSelf().InSingletonScope();
            Bind<ConfigContext>().To<CacheConfigContext>().InSingletonScope();
        }
    }
}