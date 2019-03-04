using MasterChief.DotNet.Core.DapperTests;
using MasterChief.DotNet.Core.DapperTests.Model;
using MasterChief.DotNet.Core.DapperTests.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Threading.Tasks;

namespace MasterChief.DotNet.Core.Dapper.Tests
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
        public void GetTest()
        {
            EFSample actual = _sampleService.Get(new Guid("A2E4CA6D-1A11-4D3A-9DB3-006997B05E64"));
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void CreateTest()
        {
            bool actual = _sampleService.Create(new EFSample() { UserName = DateTime.Now.ToString() });
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void CreateTestThreadTest()
        {
            Task[] tasks = {
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                                Task.Factory.StartNew(() => CreateTest()),
                            };
            Task.WaitAll(tasks);
        }
    }
}