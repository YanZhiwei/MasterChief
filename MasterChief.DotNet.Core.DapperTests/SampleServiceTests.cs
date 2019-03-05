using MasterChief.DotNet.Core.DapperTests;
using MasterChief.DotNet.Core.DapperTests.Model;
using MasterChief.DotNet.Core.DapperTests.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
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
        public void SqlQueryTest()
        {
            List<EFSample> actual = _sampleService.SqlQuery();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod()]
        public void GetTest()
        {
            EFSample actual = _sampleService.Get(new Guid("D164AEE5-CE41-46A7-ACCE-7C07A73BA1A2"));
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void CreateTest()
        {
            bool actual = _sampleService.Create(new EFSample() { UserName = "Dapper" + DateTime.Now.ToString("MMddHHmmss") });
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