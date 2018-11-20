namespace MasterChief.DotNet4.Utilities.WebForm.Core
{
    /// <summary>
    /// 生成分页样式
    /// </summary>
    /// 时间：2016-01-05 11:21
    /// 备注：
    public static class PagerHtmlScript
    {
        /// <summary>
        /// 创建普通分页HTML脚本
        /// </summary>
        /// <param name="pageIndex">当前分页索引</param>
        /// <param name="totalPageCount">总共分页数量</param>
        /// <returns>分页Html</returns>
        /// 时间：2016-01-05 11:21
        /// 备注：
        public static string BuilderNormal(int pageIndex, int totalPageCount)
        {
            return BuilderNormal(pageIndex, totalPageCount, string.Empty);
        }

        /// <summary>
        /// 创建普通分页HTML脚本
        /// </summary>
        /// <param name="pageIndex">当前分页索引</param>
        /// <param name="totalPageCount">总共分页数量</param>
        /// <param name="extraParams">额外参数</param>
        /// <returns>分页Html</returns>
        /// 时间：2016-01-05 11:22
        /// 备注：
        public static string BuilderNormal(int pageIndex, int totalPageCount, string extraParams)
        {
            return BuilderNormal(pageIndex, totalPageCount, extraParams, string.Empty);
        }

        /// <summary>
        /// 创建普通分页HTML脚本
        /// </summary>
        /// <param name="pageIndex">当前分页索引</param>
        /// <param name="totalPageCount">总共分页数量</param>
        /// <param name="extraParams">额外参数</param>
        /// <param name="absoluteAddress">The absolute address.</param>
        /// <returns>分页Html</returns>
        /// 时间：2016-01-05 11:23
        /// 备注：
        public static string BuilderNormal(int pageIndex, int totalPageCount, string extraParams, string absoluteAddress)
        {
            if (pageIndex > totalPageCount)
                pageIndex = totalPageCount;

            if (pageIndex < 1)
                pageIndex = 1;

            string pageIndexInfo = pageIndex.ToString() + "/" + totalPageCount.ToString();

            if (extraParams != string.Empty)
                extraParams = "&" + extraParams;

            if (pageIndex > 2)
            {
                pageIndexInfo = "<a href=\"" + absoluteAddress + "?pageIndex=1" + extraParams + "\">第一页</a> <a href=\"" + absoluteAddress + "?pageIndex=" + (pageIndex - 1).ToString() + "" + extraParams + "\">上一页</a> " + pageIndexInfo;
            }
            else if (pageIndex == 2)
            {
                pageIndexInfo = "<a href=\"" + absoluteAddress + "?pageIndex=1" + extraParams + "\">第一页</a> " + pageIndexInfo;
            }

            if (pageIndex == totalPageCount - 1)
            {
                pageIndexInfo = pageIndexInfo + " <a href=\"" + absoluteAddress + "?pageIndex=" + totalPageCount.ToString() + extraParams + "\">末页</a>";
            }
            else if (pageIndex < totalPageCount - 1)
            {
                pageIndexInfo = pageIndexInfo + " <a href=\"" + absoluteAddress + "?pageIndex=" + (pageIndex + 1).ToString() + extraParams + "\">下一页</a> " + "<a href=\"" + absoluteAddress + "?pageIndex=" + totalPageCount.ToString() + extraParams + "\">末页</a> ";
            }
            return pageIndexInfo;
        }
    }
}