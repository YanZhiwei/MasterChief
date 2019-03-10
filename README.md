C# 开发辅助类库，和士官长一样身经百战且越战越勇的战争机器，能力无人能出其右

项目架构思维导图：

![设计](https://9o7amq.dm.files.1drv.com/y4mvdeFkWkSFrsbowcTYmKLg5_xMkv2M9_7S3HwyQp1lmvOWURZQnzQN18sMDcB-sGNRf4ZS-WqImDuOKY_1huI90ubUT8uf1oaFk0ojztU9xjId0pvhTnu0B6DcMD9JdDYVOHEgBoUq3U23QntnItai4eIqrTvtHr5bkwdrQjDqHZKp2FBs0Fuv25LT-z_iSONM8mdzBCeEXzGVl6xsiLLFQ?width=1140&height=510&cropmode=none)

1. Data Access 模块说明

   a. 支持Dapper和Entity Framework 两种ORM框架;

   b. 通过IOC可以很少代码在Dapper和Entity Framework切换；

   c. 实现Repository和UnitOfWork；

   d. CURD以及事务实现简单，很大程度关注业务实现即可；

代码使用说明：

1. Create 添加

```csharp
public bool Create(EFSample samle)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.Create<EFSample>(samle);
    }
}
```

2. Delete 删除

```c#
public bool Delete(EFSample sample)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.Delete(sample);
    }
}
```

3. Update 修改

```c#
public bool Update(EFSample sample)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.Update(sample);
    }
}
```

3. GetByKeyID 根据主键查询

```csharp
public EFSample GetByKeyID(Guid id)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.GetByKeyID<EFSample>(id);
    }
}
```

4. GetList 条件查询集合

```c#
public List<EFSample> GetList(Expression<Func<EFSample, bool>> predicate = null)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.GetList<EFSample>(predicate);
    }
}
```

5. Exist 条件查询是否存在

```c#
public bool Exist(Expression<Func<EFSample, bool>> predicate = null)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.Exist<EFSample>(predicate);
    }
}
```

6. SqlQuery 执行Sql脚本

```c#
public List<EFSample> SqlQuery(string sql, DbParameter[] parameter)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.SqlQuery<EFSample>(sql, parameter)?.ToList();
    }
}
```

7. CreateWithTransaction 事务处理

```c#
public bool CreateWithTransaction(EFSample sample, EFSample sample2)
{
    bool result = true;
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        try
        {
            dbcontext.BeginTransaction();//开启事务
            dbcontext.Create(sample);
            dbcontext.Create(sample2);
            dbcontext.Commit();
        }
        catch (Exception)
        {
            dbcontext.Rollback();
            result = false;
        }
    }
 
    return result;
}
```

8. GetFirstOrDefault 条件查询第一项或默认数据

   ```c#
   public EFSample GetFirstOrDefault(Expression<Func<EFSample, bool>> predicate = null)
   {
       using (IDbContext dbcontext = _contextFactory.Create())
       {
           return dbcontext.GetFirstOrDefault<EFSample>(predicate);
       }
   }
   ```

9. 单元测试以及Sql Server脚本

   ```csharp
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
           private readonly Guid _testID = "2F6D3C43-C2C7-4398-AD2B-ED5E82D78888".ToGuidOrDefault(Guid.Empty);
           private readonly string _testName = "DapperSample";
    
           [TestInitialize]
           public void SetUp()
           {
               _kernel = new StandardKernel(new ServiceModule());
               Assert.IsNotNull(_kernel);
    
               _sampleService = _kernel.Get<ISampleService>();
               if (!_sampleService.Exist(ent => ent.ID == _testID))
               {
                   _sampleService.Create(new EFSample() { UserName = _testName, ID = _testID });
               }
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
               EFSample actual = _sampleService.GetFirstOrDefault(ent => ent.ID == _testID);
               Assert.IsNotNull(actual);
           }
    
           [TestMethod()]
           public void GetByKeyIdTest()
           {
               EFSample actual = _sampleService.GetByKeyID(_testID);
               Assert.IsNotNull(actual);
           }
    
           [TestMethod()]
           public void DeleteTest()
           {
               bool actual = _sampleService.Delete(new EFSample() { ID = _testID });
               Assert.IsTrue(actual);
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
                   ID = _testID,
                   ModifyTime = DateTime.Now,
                   UserName = "modify"
               };
               bool actual = _sampleService.Update(sample);
               Assert.IsNotNull(actual);
           }
    
           [TestMethod()]
           public void TransactionSuccessTest()
           {
               EFSample sample = new EFSample
               {
                   UserName = "TransactionSuccess1"
               };
    
               EFSample sample2 = new EFSample
               {
                   UserName = "TransactionSuccess2"
               };
               bool actual = _sampleService.CreateWithTransaction(sample, sample2);
               Assert.IsTrue(actual);
           }
    
           [TestMethod()]
           public void TransactionFailTest()
           {
               EFSample sample3 = new EFSample
               {
                   UserName = "TransactionSuccess3"
               };
    
               EFSample sample4 = new EFSample
               {
                   UserName = null
               };
               bool actual = _sampleService.CreateWithTransaction(sample3, sample4);
               Assert.IsFalse(actual);
           }
    
           [TestMethod()]
           public void ExistTest()
           {
               bool actual = _sampleService.Exist(ent => ent.ID == _testID);
               Assert.IsTrue(actual);
    
               actual = _sampleService.Exist(ent => ent.UserName == _testName);
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
   ```

   ```mssql
   USE [Sample]
   GO
    
   /****** Object:  Table [dbo].[EFSample]    Script Date: 2019/3/9 22:04:45 ******/
   SET ANSI_NULLS ON
   GO
    
   SET QUOTED_IDENTIFIER ON
   GO
    
   CREATE TABLE [dbo].[EFSample](
   	[ID] [uniqueidentifier] NOT NULL,
   	[CreateTime] [datetime] NOT NULL,
   	[ModifyTime] [datetime] NOT NULL,
   	[Available] [bit] NOT NULL,
   	[UserName] [nvarchar](20) NOT NULL,
    CONSTRAINT [EFSamle_PK] PRIMARY KEY CLUSTERED 
   (
   	[ID] ASC
   )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
   ) ON [PRIMARY]
   GO
    
   EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EFSample', @level2type=N'COLUMN',@level2name=N'UserName'
   GO
   ```

