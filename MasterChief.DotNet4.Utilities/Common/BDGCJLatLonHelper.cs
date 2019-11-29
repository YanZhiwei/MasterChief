using System;
using MasterChief.DotNet4.Utilities.Models;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     火星坐标系 (GCJ-02)与百度坐标系 (BD-09) 转换帮助类
    /// </summary>
    public class BdgcjLatLonHelper
    {
        #region Fields

        /*
         *参考：
         *BD09坐标系：即百度坐标系，GCJ02坐标系经加密后的坐标系。
         */

        /// <summary>
        ///     The x_pi
        /// </summary>
        /// 时间：2015-09-14 9:07
        /// 备注：
        private const double Pi = 3.14159265358979324 * 3000.0 / 180.0;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     将BD-09坐标转换成GCJ-02坐标
        /// </summary>
        /// <param name="bdPoint">BD-09坐标</param>
        /// <returns>GCJ-02坐标</returns>
        public LatLngPoint Bd09ToGcj02(LatLngPoint bdPoint)
        {
            var latLngPoint = new LatLngPoint();
            double x = bdPoint.LonX - 0.0065, y = bdPoint.LatY - 0.006;
            var z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * Pi);
            var theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * Pi);
            latLngPoint.LonX = z * Math.Cos(theta);
            latLngPoint.LatY = z * Math.Sin(theta);
            return latLngPoint;
        }

        /// <summary>
        ///     将GCJ-02坐标转换成BD-09坐标
        /// </summary>
        /// <param name="gcjPoint">GCJ-02坐标</param>
        /// <returns>BD-09坐标</returns>
        public LatLngPoint Gcj02ToBd09(LatLngPoint gcjPoint)
        {
            var latLng = new LatLngPoint();
            double x = gcjPoint.LonX, y = gcjPoint.LatY;
            var z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * Pi);
            var theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * Pi);
            latLng.LonX = z * Math.Cos(theta) + 0.0065;
            latLng.LatY = z * Math.Cos(theta) + 0.006;
            return latLng;
        }

        #endregion Methods
    }
}