using MasterChief.DotNet.Core.DapperTests;
using MasterChief.DotNet.Core.DapperTests.Model;
using MasterChief.DotNet.Core.DapperTests.Service;
using MasterChief.DotNet4.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
        /// 创建测试
        /// </summary>
        [TestMethod()]
        public void CreateTest()
        {
            bool actual = _sampleService.Create(new EFSample() { UserName = "Dapper" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void GetFirstOrDefaultTest()
        {
            EFSample actual = _sampleService.GetFirstOrDefault(ent => ent.ID == "2F6D3C43-C2C7-4398-AD2B-ED5E82D7514E".ToGuidOrDefault(Guid.Empty));
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void GetByKeyIdTest()
        {
            EFSample actual = _sampleService.GetByKeyID("2F6D3C43-C2C7-4398-AD2B-ED5E82D7514E".ToGuidOrDefault(Guid.Empty));
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void GetListTest()
        {
            List<EFSample> actual = _sampleService.GetList(ent => ent.Available == true);
            Assert.IsNotNull(actual);
            CollectionAssert.AllItemsAreNotNull(actual);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            EFSample sample = new EFSample
            {
                ID = "2F6D3C43-C2C7-4398-AD2B-ED5E82D7514E".ToGuidOrDefault(Guid.Empty),
                ModifyTime = DateTime.Now,
                UserName = "modify"
            };
            bool actual = _sampleService.Update(sample);
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void ExistTest()
        {
            bool actual = _sampleService.Exist(ent => ent.ID == "2F6D3C43-C2C7-4398-AD2B-ED5E82D7514E".ToGuidOrDefault(Guid.Empty));
            Assert.IsTrue(actual);

            actual = _sampleService.Exist(ent => ent.UserName == "Dapper0309002757");
            Assert.IsTrue(actual);

            actual = _sampleService.Exist(ent => ent.CreateTime >= DateTime.Now.AddDays(-1));
            Assert.IsTrue(actual);

            actual = _sampleService.Exist(ent => ent.CreateTime <= DateTime.Now);
            Assert.IsTrue(actual);

            actual = _sampleService.Exist(ent => ent.Available == true);
            Assert.IsTrue(actual);

            actual = _sampleService.Exist(ent => ent.Available != true);
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void SqlQueryTest()
        {
            string sql = @"select * from [dbo].[EFSample]
where CreateTime>=@CreateTime
and Available=@Available
order by CreateTime desc";
            DbParameter[] parameter = {
                    new SqlParameter(){ ParameterName="@CreateTime", Value=DateTime.Now.AddDays(-1) },
                    new SqlParameter(){ ParameterName="@Available", Value=true }
                };
            List<EFSample> actual = _sampleService.SqlQuery(sql, parameter);
            Assert.IsNotNull(actual);
            CollectionAssert.AllItemsAreNotNull(actual);
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