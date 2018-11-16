using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 分页查询条件信息
    /// </summary>
    public class PageCondition
    {
        /// <summary>
        /// 初始化一个 默认参数（第1页，每页20，排序条件为空）的分页查询条件信息类 的新实例
        /// </summary>
        public PageCondition()
        : this(1, 20)
        { }

        /// <summary>
        /// 初始化一个 指定页索引与页大小的分页查询条件信息类 的新实例
        /// </summary>
        /// <param name="pageIndex"> 页索引 </param>
        /// <param name="pageSize"> 页大小 </param>
        public PageCondition(int pageIndex, int pageSize)
        {
            ValidateOperator.Begin().CheckGreaterThan<int>(pageIndex, "页索引", 0, false).CheckGreaterThan<int>(pageSize, "页大小", 0, false);
            PageIndex = pageIndex;
            PageSize = pageSize;
            SortConditions = new SortCondition[] { };
        }

        /// <summary>
        /// 获取或设置 页索引
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置 页大小
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 主键字段
        /// </summary>
        public string PrimaryKeyField
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置 排序条件组
        /// </summary>
        public SortCondition[] SortConditions
        {
            get;
            set;
        }
    }
}