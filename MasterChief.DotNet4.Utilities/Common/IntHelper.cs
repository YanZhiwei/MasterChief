namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     Int 辅助类
    /// </summary>
    public static class IntHelper
    {
        /// <summary>
        ///     获取低位
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns>低位数值</returns>
        public static int GetLow(this int number)
        {
            return number & 0x0000FFFF;
        }
    }
}