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

2. 日志模块说明

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

