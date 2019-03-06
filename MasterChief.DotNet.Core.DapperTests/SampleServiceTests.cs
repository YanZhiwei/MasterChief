using MasterChief.DotNet.Core.DapperTests;
using MasterChief.DotNet.Core.DapperTests.Model;
using MasterChief.DotNet.Core.DapperTests.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MasterChief.DotNet4.Utilities.Common;
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

        /// <summary>
        /// 脚本查询测试
        /// </summary>
        [TestMethod()]
        public void SqlQueryTest()
        {
            List<EFSample> actual = _sampleService.SqlQuery();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

        /// <summary>
        /// 查询测试
        /// </summary>
        [TestMethod()]
        public void GetTest()
        {
            EFSample actual = _sampleService.Get(new Guid("BE492852-16C0-406D-B74B-17EE3D5F4C06"));
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// 创建测试
        /// </summary>
        [TestMethod()]
        public void CreateTest()
        {
            bool actual = _sampleService.Create(new EFSample() { UserName = "Dapper" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// 删除测试
        /// </summary>
        [TestMethod()]
        public void DeleteTest()
        {
            bool actual = _sampleService.Delete(new EFSample() { ID = new Guid("97EF5448-EB83-4730-B1A3-47B66634EF27") });
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// 删除测试
        /// </summary>
        [TestMethod()]
        public void ExistTest()
        {
            //bool actual = _sampleService.Exist<EFSample>(ent => ent.ID == new Guid("AFF0E545-8731-465F-8B0E-BFCAB44D6386"));
            //Assert.IsTrue(actual);

            //actual = _sampleService.Exist<EFSample>(ent => ent.ID == Guid.Empty);
            //Assert.IsFalse(actual);

            //var actual = _sampleService.Exist<EFSample>(ent => ent.CreateTime == "2019-03-06 22:44:33.373".ToDateOrDefault(DateTime.Now));
            //Assert.IsTrue(actual);

            var actual = _sampleService.Exist<EFSample>(ent => ent.UserName.Contains("dapper"));
            Assert.IsTrue(actual);

        }

        /// <summary>
        /// 多线程测试
        /// </summary>
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