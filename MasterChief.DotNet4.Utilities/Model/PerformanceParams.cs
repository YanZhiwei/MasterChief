namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 性能测试参数
    /// </summary>
    public class PerformanceParams
    {
        #region Properties

        /// <summary>
        /// 是否多线程测试
        /// </summary>
        public bool IsMultithread
        {
            get; set;
        }

        /// <summary>
        /// 测试次数
        /// </summary>
        public int RunCount
        {
            get; set;
        }

        #endregion Properties
    }
}