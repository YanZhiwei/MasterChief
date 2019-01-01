namespace MasterChief.DotNet4.Utilities.Result
{
    /// <summary>
    /// 检查结果
    /// </summary>
    public sealed class CheckResult : BasicResult<string>
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="state">检查状态</param>
        public CheckResult(string message, bool state)
            : base(message, null)
        {
            State = state;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 检查状态
        /// </summary>
        public bool State
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="message">检查附加错误信息</param>
        /// <returns>CheckResult</returns>
        public static CheckResult Fail(string message)
        {
            CheckResult _item = new CheckResult(message, false);
            return _item;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message">成功附加信息</param>
        /// <returns>CheckResult</returns>
        public static CheckResult Success(string message)
        {
            CheckResult _item = new CheckResult(message, true);
            return _item;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns>CheckResult</returns>
        public static CheckResult Success()
        {
            return Success(null);
        }

        #endregion Methods
    }

    /// <summary>
    /// 泛型检查结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CheckResult<T> : BasicResult<T>
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="state">检查状态</param>
        /// <param name="data">附加数据</param>
        public CheckResult(string message, bool state, T data)
            : base(message, data)
        {
            State = state;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 检查状态
        /// </summary>
        public bool State
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="message">检查附加错误信息</param>
        /// <returns>CheckResult</returns>
        public static CheckResult<T> Fail(string message)
        {
            CheckResult<T> _item = new CheckResult<T>(message, false, default(T));
            return _item;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="message">成功附加信息</param>
        /// <param name="data">附加数据</param>
        /// <returns>CheckResult</returns>
        public static CheckResult<T> Success(string message, T data)
        {
            CheckResult<T> _item = new CheckResult<T>(message, true, data);
            return _item;
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">附加数据</param>
        /// <returns>CheckResult</returns>
        public static CheckResult<T> Success(T data)
        {
            return Success(string.Empty, data);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns>CheckResult</returns>
        public static CheckResult<T> Success()
        {
            return Success(default(T));
        }

        #endregion Methods
    }
}