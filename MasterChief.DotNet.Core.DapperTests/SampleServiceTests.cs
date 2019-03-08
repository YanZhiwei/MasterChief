using MasterChief.DotNet.Core.DapperTests;
using MasterChief.DotNet.Core.DapperTests.Model;
using MasterChief.DotNet.Core.DapperTests.Service;
using MasterChief.DotNet4.Utilities.Common;
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
            EFSample actual = _sampleService.Get(new Guid("438168E6-F5E0-4D69-9482-B12B58CFD7B4"));
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void GetByTest()
        {
            EFSample actual = _sampleService.Get("UpdateTest");
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
        /// 更新测试
        /// </summary>
        [TestMethod()]
        public void UpdateTest()
        {
            EFSample sample = new EFSample
            {
                ID = "AF6F423E-F820-422E-8EBD-915DD476558F".ToGuidOrDefault(Guid.Empty),
                ModifyTime = DateTime.Now,
                UserName = "UpdateTest"
            };
            bool actual = _sampleService.Update(sample);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// 删除测试
        /// </summary>
        [TestMethod()]
        public void ExistTest()
        {
            bool actual = _sampleService.Exist<EFSample>(ent => ent.ID == new Guid("486A03E5-A58D-465C-9423-1BFAD7E40247"));
            Assert.IsTrue(actual);

            actual = _sampleService.Exist<EFSample>(ent => ent.ID == Guid.Empty);
            Assert.IsFalse(actual);

            actual = _sampleService.Exist<EFSample>(ent => ent.CreateTime == "2019-03-06 22:44:33.373".ToDateOrDefault(DateTime.Now));
            Assert.IsTrue(actual);

            actual = _sampleService.Exist<EFSample>(ent => ent.CreateTime >= "2019-03-06 22:44:33.373".ToDateOrDefault(DateTime.Now));
            Assert.IsTrue(actual);

            actual = _sampleService.Exist<EFSample>(ent => ent.UserName == "UpdateTest");
            Assert.IsTrue(actual);

            actual = _sampleService.Exist("UpdateTest");
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