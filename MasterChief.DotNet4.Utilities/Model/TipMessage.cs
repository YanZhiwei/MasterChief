namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// SplashForm 提示消息
    /// </summary>
    public struct TipMessage
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="caption">标题</param>
        /// <param name="message">消息</param>
        public TipMessage(string caption, string message)
            : this()
        {
            this.Caption = caption;
            this.Message = message;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 标题
        /// </summary>
        public string Caption
        {
            get; set;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get; set;
        }

        #endregion Properties
    }
}