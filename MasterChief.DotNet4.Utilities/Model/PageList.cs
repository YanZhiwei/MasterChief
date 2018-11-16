namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 数据分页信息
    /// </summary>
    public sealed class PageList<T>
    {
        #region Properties

        /// <summary>
        /// 获取或设置 分页数据
        /// </summary>
        public T[] Data
        {
            get; set;
        }

        /// <summary>
        /// 获取或设置 总记录数
        /// </summary>
        public int TotalCount
        {
            get; set;
        }

        /// <summary>
        /// 获取或设置 总页数
        /// </summary>
        public int TotalPage
        {
            get; set;
        }

        #endregion Properties
    }
}