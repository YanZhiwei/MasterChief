namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        #region Fields

        private static readonly short BackFileSize_MB = 2;
        private static readonly string FilePath;
        private static readonly ThreadSafeQueue<string> LogColQueue;
        private static readonly Thread LogTask;

        //自定义线程安全的Queue
        private static readonly object SyncRoot;

        #endregion Fields

        #region Constructors

        //超过2M就开始备份日志文件
        static LogHelper()
        {
            SyncRoot = new object();
            FilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\";
            Debug.WriteLine("Log Path:" + FilePath);
            LogTask = new Thread(WriteLog);
            LogColQueue = new ThreadSafeQueue<string>();
            LogTask.Start();
            Debug.WriteLine("Log Start......");
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 结束日志线程
        /// </summary>
        public static void FinishedLogTask()
        {
            LogTask.Abort();
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void Write(string msg)
        {
            string _msg = string.Format("{0} : {1}", DateTime.Now.ToString("HH:mm:ss"), msg);
            LogColQueue.Enqueue(_msg);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="type">日志类型</param>
        public static void Write(string msg, LogLevel type)
        {
            string _msg = string.Format("{0} {1}: {2}", DateTime.Now.ToString("HH:mm:ss"), type, msg);
            LogColQueue.Enqueue(_msg);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="msg">日志内容</param>
        public static void Write(LogLevel type, string msg)
        {
            string _msg = string.Format("{0} {1}: {2}", DateTime.Now.ToString("HH:mm:ss"), type, msg);
            LogColQueue.Enqueue(_msg);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="ex">异常</param>
        public static void Write(Exception ex)
        {
            if (ex != null)
            {
                LogColQueue.Enqueue(ExceptionHelper.FormatMessage(ex, true));
            }
        }

        private static void BackLog(string path)
        {
            lock (SyncRoot)
            {
                if (FileHelper.GetMBSize(path) > BackFileSize_MB)
                {
                    FileHelper.CopyToBak(path);
                }
            }
        }

        private static bool CreateDirectory()
        {
            bool _result = true;

            try
            {
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
            }
            catch (Exception)
            {
                _result = false;
            }

            return _result;
        }

        private static bool CreateFile(string path)
        {
            bool _result = true;

            try
            {
                if (!File.Exists(path))
                {
                    FileStream _files = File.Create(path);
                    _files.Close();
                }
            }
            catch (Exception)
            {
                _result = false;
            }

            return _result;
        }

        private static void WriteLog()
        {
            while (true)
            {
                Thread.Sleep(100);

                if (LogColQueue.Count() > 0)
                {
                    string _msg = LogColQueue.Dequeue();
                    Monitor.Enter(SyncRoot);

                    if (!CreateDirectory())
                    {
                        continue;
                    }

                    string _path = string.Format("{0}{1}.log", FilePath, DateTime.Now.ToString("yyyyMMdd"));
                    Monitor.Exit(SyncRoot);

                    lock (SyncRoot)
                    {
                        if (CreateFile(_path))
                        {
                            WriteLog(_path, _msg);//写入日志到文本
                        }
                    }

                    BackLog(_path);//日志备份
                }
            }
        }

        private static void WriteLog(string path, string msg)
        {
            try
            {
                StreamWriter _sw = File.AppendText(path);
                _sw.WriteLine(msg);
                _sw.Flush();
                _sw.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("写入日志失败，原因:{0}", ex.Message));
            }
        }

        #endregion Methods
    }
}