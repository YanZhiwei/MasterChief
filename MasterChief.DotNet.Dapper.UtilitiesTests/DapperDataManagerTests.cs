using MasterChief.DotNet.Dapper.UtilitiesTests.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MasterChief.DotNet.Dapper.Utilities.Tests
{
    [TestClass()]
    public class DapperDataManagerTests
    {
        private DapperDataManager dataManager = null;

        [TestInitialize]
        public void SetUp()
        {
            string connectString = "server=localhost;database=Sample;uid=sa;pwd=sasa";
            dataManager = new DapperSqlServerManager(connectString);
        }

        [TestMethod()]
        public void ExecuteDataTableTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            string sql = @"INSERT INTO [dbo].[EFSample]
           ([ID]
           ,[CreateTime]
           ,[ModifyTime]
           ,[Available]
           ,[UserName])
     VALUES
           (@ID
           ,@CreateTime
           ,@ModifyTime
           ,@Available
           ,@UserName)";
            EFSample sample = new EFSample
            {
                Available = true,
                CreateTime = DateTime.Now,
                ID = Guid.NewGuid(),
                ModifyTime = DateTime.Now,
                UserName = "DapperTests"
            };
            int actual = dataManager.ExecuteNonQuery<EFSample>(sql, sample);
            Assert.IsTrue(actual > 0);
        }

        [TestMethod()]
        public void ExecuteReaderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ExecuteScalarTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryListTest()
        {
            Assert.Fail();
        }
    }
}