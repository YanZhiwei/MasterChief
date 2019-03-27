C# 开发辅助类库，和士官长一样身经百战且越战越勇的战争机器，能力无人能出其右

项目架构思维导图：

![设计](https://9o7amq.dm.files.1drv.com/y4mvdeFkWkSFrsbowcTYmKLg5_xMkv2M9_7S3HwyQp1lmvOWURZQnzQN18sMDcB-sGNRf4ZS-WqImDuOKY_1huI90ubUT8uf1oaFk0ojztU9xjId0pvhTnu0B6DcMD9JdDYVOHEgBoUq3U23QntnItai4eIqrTvtHr5bkwdrQjDqHZKp2FBs0Fuv25LT-z_iSONM8mdzBCeEXzGVl6xsiLLFQ?width=1140&height=510&cropmode=none)

目录
=================

* [1\. 数据库访问](#1-%E6%95%B0%E6%8D%AE%E5%BA%93%E8%AE%BF%E9%97%AE)
* [2\. 日志](#2-%E6%97%A5%E5%BF%97)
* [3\. 缓存](#3-%E7%BC%93%E5%AD%98)
* [4\. 配置](#4-%E9%85%8D%E7%BD%AE)
* [5\. 快速构建适用于Mvc和WebForm 验证码](#5-%E5%BF%AB%E9%80%9F%E6%9E%84%E5%BB%BA%E9%80%82%E7%94%A8%E4%BA%8Emvc%E5%
  92%8Cwebform-%E9%AA%8C%E8%AF%81%E7%A0%81)
* [6\. 快速构建序列化与反序列化](#6-%E5%BF%AB%E9%80%9F%E6%9E%84%E5%BB%BA%E5%BA%8F%E5%88%97%E5%8C%96%E4%B8%8E%E5%8F%
  8D%E5%BA%8F%E5%88%97%E5%8C%96)
* [7\. 快速构建EXCEL导入导出](#7-%E5%BF%AB%E9%80%9F%E6%9E%84%E5%BB%BAexcel%E5%AF%BC%E5%85%A5%E5%AF%BC%E5%87%BA)

Created by [gh-md-toc](https://github.com/ekalinin/github-markdown-toc.go)

#### 1. 数据库访问


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

4. GetByKeyID 根据主键查询

```csharp
public EFSample GetByKeyID(Guid id)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.GetByKeyID<EFSample>(id);
    }
}
```

5. GetList 条件查询集合

```c#
public List<EFSample> GetList(Expression<Func<EFSample, bool>> predicate = null)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.GetList<EFSample>(predicate);
    }
}
```

6. Exist 条件查询是否存在

```c#
public bool Exist(Expression<Func<EFSample, bool>> predicate = null)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.Exist<EFSample>(predicate);
    }
}
```

7. SqlQuery 执行Sql脚本

```c#
public List<EFSample> SqlQuery(string sql, DbParameter[] parameter)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.SqlQuery<EFSample>(sql, parameter)?.ToList();
    }
}
```

8. CreateWithTransaction 事务处理

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

9. GetFirstOrDefault 条件查询第一项或默认数据

```c#
public EFSample GetFirstOrDefault(Expression<Func<EFSample, bool>> predicate = null)
{
    using (IDbContext dbcontext = _contextFactory.Create())
    {
        return dbcontext.GetFirstOrDefault<EFSample>(predicate);
    }
}
```

10. 单元测试以及Sql Server脚本

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

```sql
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



#### 2. 日志

a. 目前实现基于Log4Net的本地文件日志以及Kafka ELK的日志；

b. 基于接口ILogService可以很容易扩展其他日志显示； 

代码使用说明

1. 配置依赖注入，日志实现方式，这里采用文件日志形式

```c#
using MasterChief.DotNet.Core.Log;
using Ninject.Modules;
 
namespace MasterChief.DotNet.Core.LogTests
{
    public sealed class LogModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogService>().To<FileLogService>().InSingletonScope();
        }
    }
}
```

2. 拷贝日志config文件到项目内，并设置属性“始终复制”到输出目录，您可以根据项目需求调整config内容

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler" />
  </configSections>
  <log4net>
    <!-- FileLogger -->
    <logger name="FATAL_FileLogger">
      <level value="ALL" />
      <appender-ref ref="FATAL_FileAppender" />
    </logger>
    <logger name="ERROR_FileLogger">
      <level value="ALL" />
      <appender-ref ref="ERROR_FileAppender" />
    </logger>
    <logger name="WARN_FileLogger">
      <level value="ALL" />
      <appender-ref ref="WARN_FileAppender" />
    </logger>
    <logger name="INFO_FileLogger">
      <level value="ALL" />
      <appender-ref ref="INFO_FileAppender" />
    </logger>
    <logger name="DEBUG_FileLogger">
      <level value="ALL" />
      <appender-ref ref="DEBUG_FileAppender" />
    </logger>
    <!-- AdoNetLogger -->
    <!--<logger name="AdoNetLogger">
      <level value="ALL" />
      <appender-ref ref="AdoNetAppender" />
    </logger>-->
    <!-- ConsoleLogger -->
    <logger name="ConsoleLogger">
      <level value="ALL" />
      <appender-ref ref="ColoredConsoleAppender" />
    </logger>
 
    <!--使用Rolling方式记录日志按照日来记录日志-->
    <appender name="FATAL_FileAppender" type="log4net.Appender.RollingFileAppender">
      <!--文件名,可以相对路径,也可以绝对路径,这里只给定了文件夹-->
      <file value=".\log\\FATAL\\" />
      <!--是否增加文件-->
      <appendToFile value="true" />
      <maxSizeRollBackups value="5" />
      <!--日志追加类型,Date为按日期增加文件,Size为按大小-->
      <rollingStyle value="Date" />
      <!--最小锁定模型以允许多个进程可以写入同一个文件,解决文件独占问题-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <!--最大文件大小-->
      <maximumFileSize value="10MB" />
      <!--文件命名格式,非日期参数化要进行转义,如自定义文件后缀-->
      <datePattern value="yyyyMM\\yyyy-MM-dd&quot;.log&quot;" />
      <!--是否固定文件名-->
      <staticLogFileName value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="---------------------------------------------------%newline发生时间：%date %newline事件级别：%-5level %newline事件来源：%logger%newline日志内容：%message%newline" />
      </layout>
    </appender>
    <appender name="ERROR_FileAppender" type="log4net.Appender.RollingFileAppender">
      <!--文件名,可以相对路径,也可以绝对路径,这里只给定了文件夹-->
      <file value=".\log\\ERROR\\" />
      <!--是否增加文件-->
      <appendToFile value="true" />
      <maxSizeRollBackups value="5" />
      <!--日志追加类型,Date为按日期增加文件,Size为按大小-->
      <rollingStyle value="Date" />
      <!--最小锁定模型以允许多个进程可以写入同一个文件,解决文件独占问题-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <!--最大文件大小-->
      <maximumFileSize value="10MB" />
      <!--文件命名格式,非日期参数化要进行转义,如自定义文件后缀-->
      <datePattern value="yyyyMM\\yyyy-MM-dd&quot;.log&quot;" />
      <!--是否固定文件名-->
      <staticLogFileName value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="---------------------------------------------------%newline发生时间：%date %newline事件级别：%-5level %newline事件来源：%logger%newline日志内容：%message%newline" />
      </layout>
    </appender>
    <appender name="WARN_FileAppender" type="log4net.Appender.RollingFileAppender">
      <!--文件名,可以相对路径,也可以绝对路径,这里只给定了文件夹-->
      <file value=".\log\\WARN\\" />
      <!--是否增加文件-->
      <appendToFile value="true" />
      <maxSizeRollBackups value="5" />
      <!--日志追加类型,Date为按日期增加文件,Size为按大小-->
      <rollingStyle value="Date" />
      <!--最小锁定模型以允许多个进程可以写入同一个文件,解决文件独占问题-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <!--最大文件大小-->
      <maximumFileSize value="10MB" />
      <!--文件命名格式,非日期参数化要进行转义,如自定义文件后缀-->
      <datePattern value="yyyyMM\\yyyy-MM-dd&quot;.log&quot;" />
      <!--是否固定文件名-->
      <staticLogFileName value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="---------------------------------------------------%newline发生时间：%date %newline事件级别：%-5level %newline事件来源：%logger%newline日志内容：%message%newline" />
      </layout>
    </appender>
    <appender name="INFO_FileAppender" type="log4net.Appender.RollingFileAppender">
      <!--文件名,可以相对路径,也可以绝对路径,这里只给定了文件夹-->
      <file value=".\log\\INFO\\" />
      <!--是否增加文件-->
      <appendToFile value="true" />
      <maxSizeRollBackups value="5" />
      <!--日志追加类型,Date为按日期增加文件,Size为按大小-->
      <rollingStyle value="Date" />
      <!--最小锁定模型以允许多个进程可以写入同一个文件,解决文件独占问题-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <!--最大文件大小-->
      <maximumFileSize value="10MB" />
      <!--文件命名格式,非日期参数化要进行转义,如自定义文件后缀-->
      <datePattern value="yyyyMM\\yyyy-MM-dd&quot;.log&quot;" />
      <!--是否固定文件名-->
      <staticLogFileName value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="---------------------------------------------------%newline发生时间：%date %newline事件级别：%-5level %newline事件来源：%logger%newline日志内容：%message%newline" />
      </layout>
    </appender>
    <appender name="DEBUG_FileAppender" type="log4net.Appender.RollingFileAppender">
      <!--文件名,可以相对路径,也可以绝对路径,这里只给定了文件夹-->
      <file value=".\log\\DEBUG\\" />
      <!--是否增加文件-->
      <appendToFile value="true" />
      <maxSizeRollBackups value="5" />
      <!--日志追加类型,Date为按日期增加文件,Size为按大小-->
      <rollingStyle value="Date" />
      <!--最小锁定模型以允许多个进程可以写入同一个文件,解决文件独占问题-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <!--最大文件大小-->
      <maximumFileSize value="10MB" />
      <!--文件命名格式,非日期参数化要进行转义,如自定义文件后缀-->
      <datePattern value="yyyyMM\\yyyy-MM-dd&quot;.log&quot;" />
      <!--是否固定文件名-->
      <staticLogFileName value="false" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="---------------------------------------------------%newline发生时间：%date %newline事件级别：%-5level %newline事件来源：%logger%newline日志内容：%message%newline" />
      </layout>
    </appender>
    <!--使用AdoNetAppender方式记录日志按照日来记录日志-->
    <!--<appender name="AdoNetAppender" type="log4net.Appender.AdoNetAppender">
      <bufferSize value="1" />
      <connectionType value="System.Data.SqlClient.SqlConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
      <connectionString value="DATABASE=Sample;SERVER=.\SQLEXPRESS;UID=sa;PWD=sasa;Connect Timeout=15;" />
      <commandText value="INSERT INTO [Log4Net] ([Date],[Host],[Thread],[Level],[Logger],[Message],[Exception]) VALUES (@log_date, @host, @thread, @log_level, @logger, @message, @exception)" />
      <parameter>
        <parameterName value="@log_date" />
        <dbType value="DateTime" />
        <layout type="log4net.Layout.RawTimeStampLayout" />
      </parameter>
      <parameter>
        <parameterName value="@thread" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%thread" />
        </layout>
      </parameter>
 
      <parameter>
        <parameterName value="@host" />
        <dbType value="String" />
        <size value="50" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%property{log4net:HostName}" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@log_level" />
        <dbType value="String" />
        <size value="50" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%level" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@logger" />
        <dbType value="String" />
        <size value="255" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%logger" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@message" />
        <dbType value="String" />
        <size value="4000" />
        <layout type="log4net.Layout.PatternLayout">
          <conversionPattern value="%message" />
        </layout>
      </parameter>
      <parameter>
        <parameterName value="@exception" />
        <dbType value="String" />
        <size value="4000" />
        <layout type="log4net.Layout.ExceptionLayout" />
      </parameter>
    </appender>-->
    <!--使用ConsoleAppender方式记录日志按照日来记录日志-->
    <appender name="ColoredConsoleAppender" type="log4net.Appender.ColoredConsoleAppender">
      <mapping>
        <level value="INFO" />
        <foreColor value="White, HighIntensity" />
        <backColor value="Green" />
      </mapping>
      <mapping>
        <level value="DEBUG" />
        <foreColor value="White, HighIntensity" />
        <backColor value="Blue" />
      </mapping>
      <mapping>
        <level value="WARN" />
        <foreColor value="Yellow, HighIntensity" />
        <backColor value="Purple" />
      </mapping>
      <mapping>
        <level value="ERROR" />
        <foreColor value="Yellow, HighIntensity" />
        <backColor value="Red" />
      </mapping>
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="---------------------------------------------------%newline发生时间：%date %newline事件级别：%-5level%newline事件来源：%logger%newline事件行号：%line%newline日志内容：%message%newline" />
      </layout>
    </appender>
    <appender name="UdpAppender" type="log4net.Appender.UdpAppender">
      <remoteAddress value="127.0.0.1" />
      <remotePort value="7071" />
      <layout type="log4net.Layout.XmlLayoutSchemaLog4j" />
    </appender>
    <root>
      <appender-ref ref="UdpAppender" />
    </root>
  </log4net>
</configuration>
```

3. 单元测试

```c#
using MasterChief.DotNet.Core.LogTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
 
namespace MasterChief.DotNet.Core.Log.Tests
{
    [TestClass()]
    public class FileLogServiceTests
    {
        private IKernel _kernel = null;
        private ILogService _logService = null;
 
        [TestInitialize]
        public void SetUp()
        {
            _kernel = new StandardKernel(new LogModule());
            Assert.IsNotNull(_kernel);
 
            _logService = _kernel.Get<ILogService>();
        }
 
        [TestMethod()]
        public void DebugTest()
        {
            _logService.Debug("DebugTest");
        }
 
        [TestMethod()]
        public void ErrorTest()
        {
            _logService.Error("ErrorTest");
        }
 
        [TestMethod()]
        public void FatalTest()
        {
            _logService.Fatal("FatalTest");
        }
 
        [TestMethod()]
        public void InfoTest()
        {
            _logService.Info("InfoTest");
        }
 
        [TestMethod()]
        public void WarnTest()
        {
            _logService.Warn("WarnTest");
        }
    }
}
```



#### 3. 缓存

a. 支持本地内存缓存，HttpRequest请求缓存，Redis缓存；

b. 基于ICacheProvider接口，可以很容易扩展其他缓存实现；

代码使用说明：

1. 配置依赖注入，缓存实现方式，这里采用LocalCacheProvider缓存实现；

   ```c#
   using MasterChief.DotNet.Core.Cache;
   using Ninject.Modules;
    
   namespace MasterChief.DotNet.Core.CacheTests
   {
       public sealed class CacheModule : NinjectModule
       {
           public override void Load()
           {
               Bind<ICacheProvider>().To<LocalCacheProvider>().InSingletonScope();
           }
       }
   }
   ```

2. 单元测试

   ```c#
   using MasterChief.DotNet.Core.CacheTests;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Ninject;
    
   namespace MasterChief.DotNet.Core.Cache.Tests
   {
       [TestClass()]
       public class LocalCacheProviderTests
       {
           private IKernel _kernel = null;
           private ICacheProvider _cacheProvider = null;
           private readonly string _testCacheKey = "sampleKey";
           private readonly string _testCache = "sample";
           private readonly string _testKeyFormat = "login_{0}";
    
           [TestInitialize]
           public void SetUp()
           {
               _kernel = new StandardKernel(new CacheModule());
               Assert.IsNotNull(_kernel);
    
               _cacheProvider = _kernel.Get<ICacheProvider>();
               _cacheProvider.Set(_testCacheKey, _testCache, 10);
           }
    
           [TestMethod()]
           public void GetTest()
           {
               string actual = _cacheProvider.Get<string>(_testCacheKey);
               Assert.AreEqual(_testCache, actual);
           }
    
           [TestMethod()]
           public void IsSetTest()
           {
               bool actual = _cacheProvider.IsSet(_testCacheKey);
               Assert.IsTrue(actual);
           }
    
           [TestMethod()]
           public void RemoveTest()
           {
               _cacheProvider.Remove(_testCacheKey);
               bool actual = _cacheProvider.IsSet(_testCacheKey);
               Assert.IsFalse(actual);
           }
    
           [TestMethod()]
           public void RemoveByPatternTest()
           {
               string _loginKey = string.Format(_testKeyFormat, "123");
               _cacheProvider.Set(_loginKey, _testCache, 10);
               bool actual = _cacheProvider.IsSet(_loginKey);
               Assert.IsTrue(actual);
               _cacheProvider.RemoveByPattern(_testKeyFormat);
               actual = _cacheProvider.IsSet(_loginKey);
               Assert.IsFalse(actual);
               actual = _cacheProvider.IsSet(_testCacheKey);
               Assert.IsTrue(actual);
           }
    
           [TestMethod()]
           public void SetTest()
           {
               _cacheProvider.Set("sampleSetKey", "sampleSetCache", 10);
               bool actual = _cacheProvider.IsSet("sampleSetKey");
               Assert.IsTrue(actual);
           }
       }
   }
   
   ```

#### 4. 配置

a. 目前支持配置文件本地持久化，并且支持配置文件缓存依赖减少读取文件次数；

b. 基于IConfigProvider接口，可以很容易扩展其他配置实现；

代码使用说明：

1. 配置依赖注入，配置实现方式，这里采用FileConfigProvider缓存实现；

   ```c#
   using MasterChief.DotNet.Core.Config;
   using Ninject.Modules;
    
   namespace MasterChief.DotNet.Core.ConfigTests
   {
       public sealed class ConfigModule : NinjectModule
       {
           public override void Load()
           {
               Bind<IConfigProvider>().To<FileConfigService>().InSingletonScope();
               // Bind<ConfigContext>().ToSelf().InSingletonScope();
               Bind<ConfigContext>().To<CacheConfigContext>().InSingletonScope();
           }
       }
   }
   ```

2. 扩展配置上下文基于文件依赖

   ```c#
   using MasterChief.DotNet.Core.Config;
   using MasterChief.DotNet4.Utilities.WebForm.Core;
   using System;
   using System.Web.Caching;
    
   namespace MasterChief.DotNet.Core.ConfigTests
   {
       public sealed class CacheConfigContext : ConfigContext
       {
           public override T Get<T>(string index = null)
           {
               if (!(base.ConfigService is FileConfigService))
               {
                   throw new NotSupportedException("CacheConfigContext");
               }
               string filePath = GetClusteredIndex<T>(index);
               string key = filePath;
               object cacheContent = CacheManger.Get(key);
               if (cacheContent != null)
               {
                   return (T)cacheContent;
               }
               T value = base.Get<T>(index);
               CacheManger.Set(key, value, new CacheDependency(filePath));
               return value;
           }
       }
   }
   ```

3. 单元测试

   ```c#
   using MasterChief.DotNet.Core.ConfigTests;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   using Ninject;
   using System.Collections.Generic;
    
   namespace MasterChief.DotNet.Core.Config.Tests
   {
       [TestClass()]
       public class FileConfigServiceTests
       {
           private IKernel _kernel = null;
           private IConfigProvider _configProvider = null;
           public ConfigContext _configContext = null;
    
           [TestInitialize]
           public void SetUp()
           {
               _kernel = new StandardKernel(new ConfigModule());
               Assert.IsNotNull(_kernel);
    
               _configProvider = _kernel.Get<IConfigProvider>();
               _configContext = _kernel.Get<ConfigContext>();
           }
    
           [TestMethod()]
           public void SaveConfigTest()
           {
               RedisConfig redisConfig = new RedisConfig
               {
                   AutoStart = true,
                   LocalCacheTime = 10,
                   MaxReadPoolSize = 1024,
                   MaxWritePoolSize = 1024,
                   ReadServerList = "10",
                   RecordeLog = true,
                   WriteServerList = "10"
               };
               redisConfig.RedisItems = new List<RedisItemConfig>
               {
                   new RedisItemConfig() { Text = "MasterChief" },
                   new RedisItemConfig() { Text = "Config." }
               };
    
               _configContext.Save(redisConfig, "prod");
               _configContext.Save(redisConfig, "alpha");
    
               RedisConfig prodRedisConfig = _configContext.Get<RedisConfig>("prod");
               Assert.IsNotNull(prodRedisConfig);
   
               prodRedisConfig = _configContext.Get<RedisConfig>("prod");//文件缓存测试
               Assert.IsNotNull(prodRedisConfig);
   
               RedisConfig alphaRedisConfig = _configContext.Get<RedisConfig>("alpha");
               Assert.IsNotNull(alphaRedisConfig);
    
               DaoConfig daoConfig = new DaoConfig
               {
                   Log = "server=localhost;database=Sample;uid=sa;pwd=sasa"
               };
               _configContext.Save(daoConfig, "prod");
               _configContext.Save(daoConfig, "alpha");
               DaoConfig prodDaoConfig = _configContext.Get<DaoConfig>("prod");
               Assert.IsNotNull(prodDaoConfig);
    
               DaoConfig alphaDaoConfig = _configContext.Get<DaoConfig>("alpha");
               Assert.IsNotNull(alphaDaoConfig);
           }
       }
   }
   ```

4. 本地配置会在程序根目录Config下，如图：

   ![1552231625890](https://8y6yzw.dm.files.1drv.com/y4mibGGlIfda5MwK941vhcR5zHNBGgF1UEQAjozRAJHrcAF4wr6PpkKwY4uQyLVTYIjUTcEeje88BJzOhIdPACqtDMsfWQw22v6sxy8jU4tLLF3FOpe_oZlVol4ieeiRPa_wWUIfY_5TwRzQ1eOze0EpocLrxks30kcg73LZD8P-XJR5hH0fujGPqBP0cyw5KmG-s5maLn9a-ODDEZf-LxmmQ?width=607&height=147&cropmode=none)

5. 配置文件基于XML持久化存储，如图：

   ![1552231725395](https://8y5n3a.dm.files.1drv.com/y4mqjcZUrUGGJzfE_S09gBfz-ZrWnH7vfrzxBbIb922zzqP7PU5ae5f7HgZk49_SfqZE8U3YY3H0Fn9WddI1oXRSAU9vBMcrlxX4FrXUzHBaJq9s5E8TEvaSsv-4ATHsLkHZfdttbF7h02Fo5451D2uwtxASSh8TPxrCAuJ9byRmD6qOeKYM1Kh9ZoV1HMaIVwdFF5MeS5KZ-LultAHCOvWFw?width=1155&height=235&cropmode=none)



#### 5. 快速构建适用于Mvc和WebForm 验证码

a. 派生实现ValidateCodeType抽象类，来自定义验证码样式；

b. 派生实现VerifyCodeHandler抽象类，快速切换需要显示验证码；

代码使用说明：

1. Mvc 简单使用如下：

   ```c#
   /// <summary>
   ///     处理生成Mvc 程序验证码
   /// </summary>
   public sealed class MvcVerifyCodeHandler : VerifyCodeHandler
   {
       public override void OnValidateCodeCreated(HttpContext context, string validateCode)
       {
           context.Session["validateCode"] = validateCode;
       }
    
       public override byte[] CreateValidateCode(string style)
       {
           ValidateCodeType createCode;
           switch (style)
           {
               case "type1":
                   createCode = new ValidateCode_Style1();
                   break;
               default:
                   createCode = new ValidateCode_Style1();
                   break;
           }
    
           var buffer = createCode.CreateImage(out var validateCode);
           OnValidateCodeCreated(HttpContext.Current, validateCode);
           return buffer;
       }
   }
   ```

2. WebForm 简单使用如下：

   ```c#
   /// <summary>
   ///     WebFormVerifyCodeHandler 的摘要说明
   /// </summary>
   public class WebFormVerifyCodeHandler : VerifyCodeHandler, IHttpHandler, IRequiresSessionState
   {
       public void ProcessRequest(HttpContext context)
       {
           var validateType = context.Request.Params["style"];
           var buffer = CreateValidateCode(validateType);
           context.Response.ClearContent();
           context.Response.ContentType = MimeTypes.ImageGif;
           context.Response.BinaryWrite(buffer);
       }
    
       public bool IsReusable => false;
    
       public override void OnValidateCodeCreated(HttpContext context, string validateCode)
       {
           context.Session["validateCode"] = validateCode;
       }
    
       public override byte[] CreateValidateCode(string style)
       {
           style = style?.Trim();
           ValidateCodeType createCode;
           switch (style)
           {
               case "type1":
                   createCode = new ValidateCode_Style1();
                   break;
    
               default:
                   createCode = new ValidateCode_Style1();
                   break;
           }
    
           var buffer = createCode.CreateImage(out var validateCode);
           OnValidateCodeCreated(HttpContext.Current, validateCode);
           return buffer;
       }
   }
   
   ```

#### 6. 快速构建序列化与反序列化

a. 目前支持Json以及Protobuf两种方式的序列化与反序列化

b. 可以通过实现接口ISerializer扩展实现其他方式；

代码使用说明：

```c#
private static void Main()
{
    SampleSerializer(new JsonSerializer());
    Console.WriteLine(Environment.NewLine);
    SampleSerializer(new ProtocolBufferSerializer());
    Console.ReadLine();
}
 
private static void SampleSerializer(ISerializer serializer)
{
    #region 单个对象序列化与反序列化
 
    var person = new Person();
    person.Age = 10;
    person.FirstName = "yan";
    person.LastName = "zhiwei";
    person.Remark = "ISerializer Sample";
    var jsonText = serializer.Serialize(person);
    Console.WriteLine($"{serializer.GetType().Name}-Serialize" + jsonText);
 
 
    var getPerson = serializer.Deserialize<Person>(jsonText);
    Console.WriteLine($"{serializer.GetType().Name}-Deserialize" + getPerson);
 
    #endregion
 
    #region 集合序列化与反序列化
 
    var persons = new List<Person>();
    for (var i = 0; i < 10; i++)
        persons.Add(new Person
        {
            FirstName = "Yan",
            Age = 20 + i,
            LastName = "Zhiwei",
            Remark = DateTime.Now.ToString(CultureInfo.InvariantCulture)
        });
    jsonText = serializer.Serialize(persons);
    Console.WriteLine($"{serializer.GetType().Name}-Serialize" + jsonText);
 
    var getPersons = serializer.Deserialize<List<Person>>(jsonText);
    foreach (var item in getPersons)
        Console.WriteLine($"{serializer.GetType().Name}-Deserialize" + item);
 
    #endregion
}
```

![](https://846dvq.dm.files.1drv.com/y4mWYJTjMj8XnRRQEUW4U1zgKNRGIVbY_CKcJS0hUipY_ydHAEYW8pW-xc8Bkj0BnF6k4SdJDrPeSxDLx3UcNq-xliJk6N5OyJ3fTJ00nBFuf2wcg_esB8CggR2jwHYgBbvcYGappUbqG4aqi3-sd4e8PAKyRv6DkjmnjPo-B0xX7QaHgbV_kg0YLlhj5_BTHG83qwrk2TfVGkGROnDZMk1Zw?width=960&height=639&cropmode=none)

#### 7. 快速构建EXCEL导入导出

a. 基于Npoi实现，可以基于接口IExcelManger扩展实现诸如MyXls等；

b. 目前实现了将Excel导出DataTable和DataTable导出到Excel文件；

c. 后续完善诸如整个Excel文件导入导出等；

代码使用说明：

1. 将DataTable导出到Excel文件

   ```c#
   private void BtnToExcel_Click(object sender, EventArgs e)
   {
       var mockTable = BuilderExcelData();
       _mockExcelPath = $"D:\\ExcelSample{DateTime.Now.FormatDate(12)}.xls";
       _excelManger.ToExcel(mockTable, "员工信息汇总", "员工列表", _mockExcelPath);
       Process.Start(_mockExcelPath);
   }
    
   private DataTable BuilderExcelData()
   {
       var mockTable = new DataTable();
       mockTable.Columns.Add(new DataColumn {ColumnName = "序号"});
       mockTable.Columns.Add(new DataColumn {ColumnName = "姓名"});
       mockTable.Columns.Add(new DataColumn {ColumnName = "工作单位"});
       mockTable.Columns.Add(new DataColumn {ColumnName = "性别"});
       mockTable.Columns.Add(new DataColumn {ColumnName = "入职时间"});
    
       for (var i = 0; i < 100; i++)
           mockTable.Rows.Add(i.ToString(), $"张{i}", $"李{i}计算机公司", i % 2 == 0 ? "男" : "女",
               DateTime.Now.AddDays(i));
       return mockTable;
   }
   ```

   ![](https://845n1a.dm.files.1drv.com/y4mfwfj5Ba0Y2KQLZoyW7I8f1mw5Z2m6KuAIyOiNPiVwzFMtqXPrHGQ2sCa3Ugl0OpcV-Tr0Y1Qbcf009TqtkwLQPcmaAg0Y7jM5guS7e9GYC_mbPLnBUgxxob4IbYrVoxxGh_Vfad2nwRX0wmI1clxx1cgFd4Xpoc_clXpOjJ8zZ1zj4Q_099E6Sk2ucuzpXJt3KRzYlo1TgCtotELq1roDg?width=745&height=982&cropmode=none)

2. 将Excel文件导出DataTable

   ```c#
   private void BtnToDataTable_Click(object sender, EventArgs e)
   {
       if (string.IsNullOrEmpty(_mockExcelPath))
       {
           MessageBox.Show("请生成模拟测试EXCEL文件");
           return;
       }
    
       var excleTable = _excelManger.ToDataTable(_mockExcelPath, 0, 1, 2);
       var jsonText = _jsonSerializer.Serialize(excleTable);
       MessageBox.Show(jsonText);
   }
   ```

