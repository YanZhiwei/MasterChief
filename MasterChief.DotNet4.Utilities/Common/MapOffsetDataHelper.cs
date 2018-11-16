namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Collections;
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
        public ArrayList GetMapCoordArrayList()
        {
            ArrayList _mapCoordArrayList = new ArrayList();
            GetOffsetData(c => _mapCoordArrayList.Add(c));
            return _mapCoordArrayList;
        }

        /// <summary>
        /// 获取纠偏数据集合
        /// </summary>
        /// <returns>纠偏数据集合</returns>
        public List<MapCoord> GetMapCoordList()
        {
            List<MapCoord> _mapCoordList = new List<MapCoord>();
            GetOffsetData(c => _mapCoordList.Add(c));
            return _mapCoordList;
        }

        private void GetOffsetData(Action<MapCoord> mapCoordHanlder)
        {
            using (FileStream stream = new FileStream(offsetFullPath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int _size = (int)stream.Length / 8;

                    for (int i = 0; i < _size; i++)
                    {
                        byte[] _source = reader.ReadBytes(8);
                        MapCoord _coord = ToCoord(_source);
                        mapCoordHanlder(_coord);
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
            MapCoord _coord = new MapCoord();
            byte[] _b1 = new byte[2], _b2 = new byte[2], _b3 = new byte[2], _b4 = new byte[2];
            Array.Copy(bytes, 0, _b1, 0, 2);
            Array.Copy(bytes, 2, _b2, 0, 2);
            Array.Copy(bytes, 4, _b3, 0, 2);
            Array.Copy(bytes, 6, _b4, 0, 2);
            _coord.Lon = BitConverter.ToInt16(_b1, 0);
            _coord.Lat = BitConverter.ToInt16(_b2, 0);
            _coord.X_off = BitConverter.ToInt16(_b3, 0);
            _coord.Y_off = BitConverter.ToInt16(_b4, 0);
            return _coord;
        }

        #endregion Methods
    }
}