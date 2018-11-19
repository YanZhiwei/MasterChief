namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 经纬度
    /// </summary>
    public class LatLngPoint
    {
        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        public LatLngPoint(double lat, double lon)
        {
            LatY = lat;
            LonX = lon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LatLngPoint()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// 纬度
        /// </summary>
        public double LatY
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        public double LonX
        {
            get;
            set;
        }

        #endregion Properties
    }
}