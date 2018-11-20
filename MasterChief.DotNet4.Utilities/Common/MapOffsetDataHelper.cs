namespace MasterChief.DotNet4.Utilities.Common
{
    using MasterChief.DotNet4.Utilities.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 地图纠偏数据帮助类
    /// </summary>
    public class MapOffsetDataHelper
    {
        #region Fields

        private readonly string offsetFullPath = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">纠偏数据文件路径</param>
        public MapOffsetDataHelper(string path)
        {
            offsetFullPath = path;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取纠偏数据集合
        /// </summary>
        /// <returns>纠偏数据集合</returns>
        public List<MapCoord> GetMapCoordList()
        {
            List<MapCoord> mapCoords = new List<MapCoord>();
            GetOffsetData(c => mapCoords.Add(c));
            return mapCoords;
        }

        private void GetOffsetData(Action<MapCoord> mapCoordHanlder)
        {
            using (FileStream stream = new FileStream(offsetFullPath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int size = (int)stream.Length / 8;

                    for (int i = 0; i < size; i++)
                    {
                        byte[] buffer = reader.ReadBytes(8);
                        MapCoord coord = ToCoord(buffer);
                        mapCoordHanlder(coord);
                    }
                }
            }
        }

        /// <summary>
        /// 将字节转化为具体的数据对象
        /// </summary>
        /// <param name="bytes">bytes</param>
        /// <returns>MapCoord</returns>
        private MapCoord ToCoord(byte[] bytes)
        {
            //经度,纬度,x偏移量,y偏移量 【均两个字节】
            MapCoord coord = new MapCoord();
            byte[] b1 = new byte[2], b2 = new byte[2], b3 = new byte[2], b4 = new byte[2];
            Array.Copy(bytes, 0, b1, 0, 2);
            Array.Copy(bytes, 2, b2, 0, 2);
            Array.Copy(bytes, 4, b3, 0, 2);
            Array.Copy(bytes, 6, b4, 0, 2);
            coord.Lon = BitConverter.ToInt16(b1, 0);
            coord.Lat = BitConverter.ToInt16(b2, 0);
            coord.X_off = BitConverter.ToInt16(b3, 0);
            coord.Y_off = BitConverter.ToInt16(b4, 0);
            return coord;
        }

        #endregion Methods
    }
}