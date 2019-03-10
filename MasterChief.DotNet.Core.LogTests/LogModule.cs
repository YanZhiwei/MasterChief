using MasterChief.DotNet.Core.Log;
using Ninject.Modules;

namespace MasterChief.DotNet.Core.LogTests
{
    public sealed class LogModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogService>().To<FileLogService>().InSingletonScope();
        }
    }
}