namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        /// 当前页
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 总共记录条数
        /// </summary>
        int TotalRecordCount { get; set; }
    }
}