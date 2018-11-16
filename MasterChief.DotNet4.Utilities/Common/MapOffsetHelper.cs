namespace MasterChief.DotNet4.Utilities.Common
{
    using System.Collections;

    /// <summary>
    /// 地图纠偏 帮助类
    /// </summary>
    public class MapOffsetHelper
    {
        #region Fields

        private ArrayList mapCoordArrayList;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="offsetData">纠偏数据</param>
        public MapOffsetHelper(ArrayList offsetData)
        {
            mapCoordArrayList = offsetData;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 火星坐标转 (GCJ-02)地球坐标(WGS-84)
        /// </summary>
        /// <param name="gcjPoint">火星坐标转 (GCJ-02)</param>
        /// <returns>地球坐标(WGS-84)</returns>
        public LatLngPoint GCJ02ToWGS84(LatLngPoint gcjPoint)
        {
            MapCoord _findedCoord = QueryOffSetData(gcjPoint);
            double _pixY = GeoHelper.LatToPixel(gcjPoint.LatY, 18);
            double _pixX = GeoHelper.LonToPixel(gcjPoint.LonX, 18);
            _pixY -= _findedCoord.Y_off;
            _pixX -= _findedCoord.X_off;
            double _lat = GeoHelper.PixelToLat(_pixY, 18);
            double _lng = GeoHelper.PixelToLon(_pixX, 18);
            return new LatLngPoint(_lat, _lng);
        }

        /// <summary>
        /// 地球坐标(WGS-84)转火星坐标 (GCJ-02)
        /// </summary>
        /// <param name="wgsPoint">地球坐标(WGS-84)</param>
        /// <returns>火星坐标 (GCJ-02)</returns>
        public LatLngPoint WGS84ToGCJ02(LatLngPoint wgsPoint)
        {
            MapCoord _findedCoord = QueryOffSetData(wgsPoint);
            double _pixY = GeoHelper.LatToPixel(wgsPoint.LatY, 18);
            double _pixX = GeoHelper.LonToPixel(wgsPoint.LonX, 18);
            _pixY += _findedCoord.Y_off;
            _pixX += _findedCoord.X_off;
            double _lat = GeoHelper.PixelToLat(_pixY, 18);
            double _lng = GeoHelper.PixelToLon(_pixX, 18);
            return new LatLngPoint(_lat, _lng);
        }

        private MapCoord QueryOffSetData(LatLngPoint point)
        {
            MapCoord _search = new MapCoord
            {
                Lat = (int)(point.LatY * 100),
                Lon = (int)(point.LonX * 100)
            };
            MapOffsetComparer rc = new MapOffsetComparer();
            int _findedIndex = mapCoordArrayList.BinarySearch(0, mapCoordArrayList.Count, _search, rc);
            MapCoord _findedCoord = (MapCoord)mapCoordArrayList[_findedIndex];
            return _findedCoord;
        }

        #endregion Methods

        #region Other

        /*
         *参考：
         *1. http://www.apkbus.com/forum.php?mod=viewthread&tid=137621&extra=page%3D1&page=1
         *2. http://yanue.net/post-122.html
         *3. http://go2log.com/2011/08/30/%E4%B8%AD%E5%9B%BD%E5%9C%B0%E5%9B%BE%E5%81%8F%E7%A7%BB%E6%A0%A1%E6%AD%A3php%E7%AE%97%E6%B3%95/
         *4. http://www.devdiv.com/ios_gps_google_-blog-60266-10835.html
         */

        #endregion Other
    }
}