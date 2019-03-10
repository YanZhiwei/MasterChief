using MasterChief.DotNet.Core.LogTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace MasterChief.DotNet.Core.Log.Tests
{
    [TestClass()]
    public class FileLogServiceTests
    {
        private IKernel _kernel = null;
        private ILogService _logService = null;

        [TestInitialize]
        public void SetUp()
        {
            _kernel = new StandardKernel(new LogModule());
            Assert.IsNotNull(_kernel);

            _logService = _kernel.Get<ILogService>();
        }

        [TestMethod()]
        public void DebugTest()
        {
            _logService.Debug("DebugTest");
        }

        [TestMethod()]
        public void ErrorTest()
        {
            _logService.Error("ErrorTest");
        }

        [TestMethod()]
        public void FatalTest()
        {
            _logService.Fatal("FatalTest");
        }

        [TestMethod()]
        public void InfoTest()
        {
            _logService.Info("InfoTest");
        }

        [TestMethod()]
        public void WarnTest()
        {
            _logService.Warn("WarnTest");
        }
    }
}