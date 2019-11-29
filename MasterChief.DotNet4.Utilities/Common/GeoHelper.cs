using System;
using MasterChief.DotNet4.Utilities.Models;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     地图操作 帮助类
    /// </summary>
    public static class GeoHelper
    {
        #region Fields

        private const double E = 2.71828182845904523536028747135266250;
        private const double Pi = 3.14159265358979323846264338327950288;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     经纬度测距
        /// </summary>
        /// <param name="fromPoint">起始经纬度</param>
        /// <param name="toPoint">结束经纬度</param>
        /// <returns>距离</returns>
        public static double GetDistance(LatLngPoint fromPoint, LatLngPoint toPoint)
        {
            double earthR = 6371000;
            var x = Math.Cos(fromPoint.LatY * Math.PI / 180) * Math.Cos(toPoint.LatY * Math.PI / 180) *
                    Math.Cos((fromPoint.LonX - toPoint.LonX) * Math.PI / 180);
            var y = Math.Sin(fromPoint.LatY * Math.PI / 180) * Math.Sin(toPoint.LatY * Math.PI / 180);
            var s = x + y;

            if (s > 1) s = 1;

            if (s < -1) s = -1;

            var alpha = Math.Acos(s);
            return alpha * earthR;
        }

        /// <summary>
        ///     将经纬度转换地图上X,Y坐标
        /// </summary>
        /// <param name="point">经纬度</param>
        /// <returns>X,Y坐标</returns>
        public static GeoPoint GetQueryLocation(LatLngPoint point)
        {
            var lat = (int) (point.LatY * 100);
            var lng = (int) (point.LonX * 100);
            var clat = (int) (point.LatY * 1000 + 0.499999) / 10.0;
            var clng = (int) (point.LonX * 1000 + 0.499999) / 10.0;

            for (var x = point.LonX; x < point.LonX + 1; x += 0.5)
            for (var y = point.LatY; x < point.LatY + 1; y += 0.5)
                if (x <= clng && clng < x + 0.5 && clat >= y && clat < y + 0.5)
                    return new GeoPoint((int) (x + 0.5), (int) (y + 0.5));

            return new GeoPoint(lng, lat);
        }

        /// <summary>
        ///     将纬度转换成地图y轴坐标
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns>坐标</returns>
        public static double LatToPixel(double lat, int zoom)
        {
            var siny = Math.Sin(lat * Pi / 180);
            var y = Math.Log((1 + siny) / (1 - siny));
            return (128 << zoom) * (1 - y / (2 * Pi));
        }

        /// <summary>
        ///     将经度转换成地图x轴坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns>坐标</returns>
        public static double LonToPixel(double lng, int zoom)
        {
            return (lng + 180) * (256L << zoom) / 360;
        }

        /// <summary>
        ///     坐标是否在国外
        /// </summary>
        /// <param name="latlon">经纬度</param>
        /// <returns>坐标是否在国外</returns>
        public static bool OutOfChina(LatLngPoint latlon)
        {
            if (latlon.LonX < 72.004 || latlon.LatY > 137.8347) return true;

            if (latlon.LonX < 0.8293 || latlon.LatY > 55.8271) return true;

            return false;
        }

        /// <summary>
        ///     将Y轴坐标转换成纬度
        /// </summary>
        /// <param name="pixelY">Y轴坐标</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns>纬度</returns>
        public static double PixelToLat(double pixelY, int zoom)
        {
            var y = 2 * Pi * (1 - pixelY / (128 << zoom));
            var z = Math.Pow(E, y);
            var siny = (z - 1) / (z + 1);
            return Math.Asin(siny) * 180 / Pi;
        }

        /// <summary>
        ///     将X轴坐标转换成经度
        /// </summary>
        /// <param name="pixelX">X轴坐标</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns>经度</returns>
        public static double PixelToLon(double pixelX, int zoom)
        {
            return pixelX * 360 / (256L << zoom) - 180;
        }

        #endregion Methods
    }
}