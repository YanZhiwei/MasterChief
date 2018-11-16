namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// 墨卡托坐标与GS-84转换 帮助类
    /// </summary>
    public static class WGSMercatorLatLonHelper
    {
        #region Methods

        /// <summary>
        /// 将墨卡托坐标转换成地球坐标(WGS-84)
        /// </summary>
        /// <param name="mercator">墨卡托坐标</param>
        /// <returns>球坐标(WGS-84)</returns>
        public static LatLngPoint MercatorToWGS84(LatLngPoint mercator)
        {
            LatLngPoint _wgsPoint = new LatLngPoint();
            double _lonX = mercator.LonX / 20037508.34 * 180;
            double _latY = mercator.LatY / 20037508.34 * 180;
            _latY = 180 / Math.PI * (2 * Math.Atan(Math.Exp(_latY * Math.PI / 180)) - Math.PI / 2);
            _wgsPoint.LonX = _lonX;
            _wgsPoint.LatY = _latY;
            return _wgsPoint;
        }

        /// <summary>
        /// 将地球坐标(WGS-84)转换成墨卡托坐标
        /// </summary>
        /// <param name="wgsPoint">地球坐标(WGS-84)</param>
        /// <returns>墨卡托坐标</returns>
        public static LatLngPoint WGS84ToMercator(LatLngPoint wgsPoint)
        {
            LatLngPoint _mercatorPoint = new LatLngPoint();
            double _lonX = wgsPoint.LonX * 20037508.34 / 180;
            double _latY = Math.Log(Math.Tan((90 + wgsPoint.LatY) * Math.PI / 360)) / (Math.PI / 180);
            _latY = _latY * 20037508.34 / 180;
            _mercatorPoint.LonX = _lonX;
            _mercatorPoint.LatY = _latY;
            return _mercatorPoint;
        }

        #endregion Methods
    }
}