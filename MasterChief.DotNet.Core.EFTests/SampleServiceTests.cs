using MasterChief.DotNet.Core.EFTests.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;

namespace MasterChief.DotNet.Core.EFTests
{
    [TestClass()]
    public class SampleServiceTests
    {
        private IKernel _kernel = null;
        private ISampleService _sampleService = null;

        [TestInitialize]
        public void SetUp()
        {
            _kernel = new StandardKernel(new ServiceModule());
            Assert.IsNotNull(_kernel);

            _sampleService = _kernel.Get<ISampleService>();
        }

        [TestMethod()]
        public void CreateTest()
        {
            bool actual = _sampleService.Create(new EFSample() { UserName = DateTime.Now.ToString() });
            Assert.IsTrue(actual);
        }
    }
}