namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// 地图坐标
    /// </summary>
    public class MapCoord
    {
        #region Properties

        /// <summary>
        /// 纬度
        /// 3130表示31.30
        /// </summary>
        public int Lat
        {
            set; get;
        }

        /// <summary>
        /// 经度
        /// 12151表示121.51
        /// </summary>
        public int Lon
        {
            set; get;
        }

        /// <summary>
        ///地图x轴偏移像素值
        /// </summary>
        public int X_off
        {
            set; get;
        }

        /// <summary>
        ///地图y轴偏移像素值
        /// </summary>
        public int Y_off
        {
            set; get;
        }

        #endregion Properties
    }
}