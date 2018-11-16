namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// 地图操作 帮助类
    /// </summary>
    public class GeoHelper
    {
        #region Fields

        private const double e = 2.71828182845904523536028747135266250;
        private const double pi = 3.14159265358979323846264338327950288;

        #endregion Fields

        #region Methods

        /// <summary>
        /// 经纬度测距
        /// </summary>
        /// <param name="fromPoint">起始经纬度</param>
        /// <param name="toPoint">结束经纬度</param>
        /// <returns>距离</returns>
        public static double GetDistance(LatLngPoint fromPoint, LatLngPoint toPoint)
        {
            double _earthR = 6371000;
            double _x = Math.Cos(fromPoint.LatY * Math.PI / 180) * Math.Cos(toPoint.LatY * Math.PI / 180) * Math.Cos((fromPoint.LonX - toPoint.LonX) * Math.PI / 180);
            double _y = Math.Sin(fromPoint.LatY * Math.PI / 180) * Math.Sin(toPoint.LatY * Math.PI / 180);
            double _s = _x + _y;

            if (_s > 1)
            {
                _s = 1;
            }

            if (_s < -1)
            {
                _s = -1;
            }

            double _alpha = Math.Acos(_s);
            return _alpha * _earthR;
        }

        /// <summary>
        /// 将经纬度转换地图上X,Y坐标
        /// </summary>
        /// <param name="point">经纬度</param>
        /// <returns>X,Y坐标</returns>
        public static GeoPoint GetQueryLocation(LatLngPoint point)
        {
            int _lat = (int)(point.LatY * 100);
            int _lng = (int)(point.LonX * 100);
            double _lat1 = ((int)(point.LatY * 1000 + 0.499999)) / 10.0;
            double _lng1 = ((int)(point.LonX * 1000 + 0.499999)) / 10.0;

            for (double x = point.LonX; x < point.LonX + 1; x += 0.5)
            {
                for (double y = point.LatY; x < point.LatY + 1; y += 0.5)
                {
                    if (x <= _lng1 && _lng1 < (x + 0.5) && _lat1 >= y && _lat1 < (y + 0.5))
                    {
                        return new GeoPoint((int)(x + 0.5), (int)(y + 0.5));
                    }
                }
            }

            return new GeoPoint(_lng, _lat);
        }

        /// <summary>
        /// 将纬度转换成地图y轴坐标
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns>坐标</returns>
        public static double LatToPixel(double lat, int zoom)
        {
            double _siny = Math.Sin(lat * pi / 180);
            double _y = Math.Log((1 + _siny) / (1 - _siny));
            return (128 << zoom) * (1 - _y / (2 * pi));
        }

        /// <summary>
        /// 将经度转换成地图x轴坐标
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns>坐标</returns>
        public static double LonToPixel(double lng, int zoom)
        {
            return (lng + 180) * (256L << zoom) / 360;
        }

        /// <summary>
        /// 坐标是否在国外
        /// </summary>
        /// <param name="latlon">经纬度</param>
        /// <returns>坐标是否在国外</returns>
        public static bool OutOfChina(LatLngPoint latlon)
        {
            if (latlon.LonX < 72.004 || latlon.LatY > 137.8347)
            {
                return true;
            }

            if (latlon.LonX < 0.8293 || latlon.LatY > 55.8271)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将Y轴坐标转换成纬度
        /// </summary>
        /// <param name="pixelY">Y轴坐标</param>
        /// <param name="zoom">缩放级别</param>
        /// <returns>纬度</returns>
        public static double PixelToLat(double pixelY, int zoom)
        {
            double y = 2 * pi * (1 - pixelY / (128 << zoom));
            double z = Math.Pow(e, y);
            double siny = (z - 1) / (z + 1);
            return Math.Asin(siny) * 180 / pi;
        }

        /// <summary>
        /// 将X轴坐标转换成经度
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