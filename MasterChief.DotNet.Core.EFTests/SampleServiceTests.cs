using MasterChief.DotNet.Core.EFTests.Service;
using MasterChief.DotNet4.Utilities.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MasterChief.DotNet.Core.EFTests.Model;

namespace MasterChief.DotNet.Core.EFTests
{
    [TestClass()]
    public class SampleServiceTests
    {
        private IKernel _kernel;
        private ISampleService _sampleService;
        private readonly Guid _testId = "2F6D3C43-C2C7-4398-AD2B-ED5E82D79999".ToGuidOrDefault(Guid.Empty);
        private const string TestName = "EFSample";

        [TestInitialize]
        public void SetUp()
        {
            _kernel = new StandardKernel(new ServiceModule());
            Assert.IsNotNull(_kernel);

            _sampleService = _kernel.Get<ISampleService>();
            //if (!_sampleService.Exist(ent => ent.ID == _testID))
            //{
            //    _sampleService.Create(new EFSample() { UserName = _testName, ID = _testID });
            //}
        }

        /// <summary>
        /// 创建测试
        /// </summary>
        [TestMethod()]
        public void CreateTest()
        {
            bool actual = _sampleService.Create(new EfSample() { UserName = "ef" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);

            actual = _sampleService.Create(new EfSample() { UserName = "ef" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);

            actual = _sampleService.Create(new EfSample() { UserName = "ef" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);

            actual = _sampleService.Create(new EfSample() { UserName = "ef" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);

            actual = _sampleService.Create(new EfSample() { UserName = "ef" + DateTime.Now.ToString("MMddHHmmss") });
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void GetFirstOrDefaultTest()
        {
            EfSample actual = _sampleService.GetFirstOrDefault(ent => ent.Id == _testId);
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void GetByKeyIdTest()
        {
            EfSample actual = _sampleService.GetByKeyId(_testId);
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void GetListTest()
        {
            // ReSharper disable once RedundantBoolCompare
            List<EfSample> actual = _sampleService.GetList(ent => ent.Available == true);
            Assert.IsNotNull(actual);
            CollectionAssert.AllItemsAreNotNull(actual);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            EfSample sample = new EfSample
            {
                Id = _testId,
                ModifyTime = DateTime.Now,
                UserName = "modify"
            };
            bool actual = _sampleService.Update(sample);
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void TransactionSuccessTest()
        {
            EfSample sample = new EfSample
            {
                UserName = "TransactionSuccess1"
            };

            EfSample sample2 = new EfSample
            {
                UserName = "TransactionSuccess2"
            };
            bool actual = _sampleService.CreateWithTransaction(sample, sample2);
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void TransactionFailTest()
        {
            EfSample sample3 = new EfSample
            {
                UserName = "TransactionSuccess3"
            };

            EfSample sample4 = new EfSample
            {
                UserName = null
            };
            bool actual = _sampleService.CreateWithTransaction(sample3, sample4);
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void ExistTest()
        {
            bool actual = _sampleService.Exist(ent => ent.Id == _testId);
            Assert.IsTrue(actual);

            actual = _sampleService.Exist(ent => ent.UserName == TestName);
            Assert.IsTrue(actual);

            DateTime createTime = DateTime.Now.AddDays(-1);
            actual = _sampleService.Exist(ent => ent.CreateTime >= createTime);
            Assert.IsTrue(actual);

            actual = _sampleService.Exist(ent => ent.CreateTime <= DateTime.Now);
            Assert.IsTrue(actual);

            // ReSharper disable once RedundantBoolCompare
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
            List<EfSample> actual = _sampleService.SqlQuery(sql, parameter);
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