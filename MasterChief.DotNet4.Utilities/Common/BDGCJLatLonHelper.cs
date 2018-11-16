namespace MasterChief.DotNet4.Utilities.Common
{
    using MasterChief.DotNet4.Utilities.Model;
    using System;

    /// <summary>
    /// 火星坐标系 (GCJ-02)与百度坐标系 (BD-09) 转换帮助类
    /// </summary>
    public class BDGCJLatLonHelper
    {
        #region Fields

        /*
         *参考：
         *BD09坐标系：即百度坐标系，GCJ02坐标系经加密后的坐标系。
         */

        /// <summary>
        /// The x_pi
        /// </summary>
        /// 时间：2015-09-14 9:07
        /// 备注：
        private const double pi = 3.14159265358979324 * 3000.0 / 180.0;

        #endregion Fields

        #region Methods

        /// <summary>
        /// 将BD-09坐标转换成GCJ-02坐标
        /// </summary>
        /// <param name="bdPoint">BD-09坐标</param>
        /// <returns>GCJ-02坐标</returns>
        public LatLngPoint BD09ToGCJ02(LatLngPoint bdPoint)
        {
            LatLngPoint latLngPoint = new LatLngPoint();
            double x = bdPoint.LonX - 0.0065, _y = bdPoint.LatY - 0.006;
            double z = Math.Sqrt(x * x + _y * _y) - 0.00002 * Math.Sin(_y * pi);
            double theta = Math.Atan2(_y, x) - 0.000003 * Math.Cos(x * pi);
            latLngPoint.LonX = z * Math.Cos(theta);
            latLngPoint.LatY = z * Math.Sin(theta);
            return latLngPoint;
        }

        /// <summary>
        /// 将GCJ-02坐标转换成BD-09坐标
        /// </summary>
        /// <param name="gcjPoint">GCJ-02坐标</param>
        /// <returns>BD-09坐标</returns>
        public LatLngPoint GCJ02ToBD09(LatLngPoint gcjPoint)
        {
            LatLngPoint latLng = new LatLngPoint();
            double _x = gcjPoint.LonX, y = gcjPoint.LatY;
            double _z = Math.Sqrt(_x * _x + y * y) + 0.00002 * Math.Sin(y * pi);
            double _theta = Math.Atan2(y, _x) + 0.000003 * Math.Cos(_x * pi);
            latLng.LonX = _z * Math.Cos(_theta) + 0.0065;
            latLng.LatY = _z * Math.Cos(_theta) + 0.006;
            return latLng;
        }

        #endregion Methods
    }
}