namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 用户Combox 键值对绑定实体类
    /// </summary>
    public class ListItem
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public ListItem()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_key">键</param>
        /// <param name="_value">值</param>
        public ListItem(string _key, string _value)
        {
            this.Key = _key;
            this.Value = _value;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 键
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Value;
        }

        #endregion Methods
    }
}