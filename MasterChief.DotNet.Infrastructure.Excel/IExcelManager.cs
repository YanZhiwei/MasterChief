using System;
using System.Data;

namespace MasterChief.DotNet.Infrastructure.Excel
{
    /// <summary>
    /// Excel manger.
    /// </summary>
    public interface IExcelManger
    {
        /// <summary>
        /// 将EXCEL文件导入到DataTable
        /// </summary>
        /// <returns>DataTable.</returns>
        /// <param name="filePath">Excel文件路径.</param>
        /// <param name="sheetIndex">Sheet 索引.</param>
        /// <param name="headIndex">列索引.</param>
        /// <param name="rowIndex">行起始索引.</param>
        DataTable ToDataTable(string filePath, ushort sheetIndex = 0, ushort headIndex = 0, ushort rowIndex = 0);

        /// <summary>
        /// 将DataTable导出Excel
        /// </summary>
        /// <param name="table">DataTable.</param>
        /// <param name="sheetName">Sheet 索引.</param>
        /// <param name="title">标题.</param>
        /// <param name="filePath">Excel存储路径</param>
        void ToExcel(DataTable table, string sheetName, string title, string filePath);
    }
}
