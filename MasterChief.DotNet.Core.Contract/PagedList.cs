using System;
using System.Collections.Generic;

namespace MasterChief.DotNet.Core.Contract
{
    /// <summary>
    /// 分页集合
    /// </summary>
    public class PagedList<T> : List<T>, IPagedList
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="allItems">集合</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        public PagedList(IList<T> allItems, int pageIndex, int pageSize)
        {
            PageSize = pageSize;
            TotalRecordCount = allItems.Count;
            PageIndex = pageIndex;

            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Add(allItems[i]);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="allItems">分页集合</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalItemCount">分页集合记录条数</param>
        public PagedList(IEnumerable<T> allItems, int pageIndex, int pageSize, int totalItemCount)
        {
            AddRange(allItems);
            TotalRecordCount = totalItemCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 总共记录条数
        /// </summary>
        public int TotalRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// 总分页记录数
        /// </summary>
        public int TotalPageCount => (int)Math.Ceiling(TotalRecordCount / (double)PageSize);

        /// <summary>
        /// 分页数据起始索引
        /// </summary>
        public int StartRecordIndex => (PageIndex - 1) * PageSize + 1;

        /// <summary>
        /// 分页数据结束索引
        /// </summary>
        public int EndRecordIndex => TotalRecordCount > PageIndex * PageSize ? PageIndex * PageSize : TotalRecordCount;
    }
}