using MasterChief.DotNet.Core.Contract;
using MasterChief.DotNet.Core.EFTests.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            bool actual = _sampleService.Create(new EFSample() { UserName = "EF" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void GetTest()
        {
            EFSample actual = _sampleService.Get(new Guid("2485CFA1-F251-4C7F-BBC7-76A8525963B5"));
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void GetByPageTest()
        {
            PagedList<EFSample> actual = _sampleService.GetByPage(3, 10);
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void SqlQueryTest()
        {
            List<EFSample> actual = _sampleService.SqlQuery();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
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