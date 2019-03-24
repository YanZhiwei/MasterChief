namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Threading;

    /// <summary>
    /// Thread 帮助类
    /// </summary>
    public static class ThreadHelper
    {
        #region Methods

        /// <summary>
        /// 取消Thread.Sleep状态，继续线程
        /// </summary>
        /// <param name="thread">Thread</param>
        public static void CancelSleep(this Thread thread)
        {
            if (thread.ThreadState != ThreadState.WaitSleepJoin)
            {
                return;
            }

            thread.Interrupt();
        }

        /// <summary>
        /// 启动线程，自动忽略停止线程时触发的<see cref="ThreadAbortException"/>异常
        /// </summary>
        /// <param name="thread">线程</param>
        /// <param name="failFactory">引发非<see cref="ThreadAbortException"/>委托</param>
        public static void StartAndIgnoreAbort(this Thread thread, Action<Exception> failFactory)
        {
            try
            {
                thread.Start();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                failFactory?.Invoke(ex);
            }
        }

        #endregion Methods
    }
}