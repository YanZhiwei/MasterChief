namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Text;

    /// <summary>
    /// 获取照片Exif信息
    /// </summary>
    /// 时间：2015-09-15 8:59
    /// 备注：
    public class ExifHelper
    {
        #region Fields

        /// <summary>
        /// exif 数据信息
        /// </summary>
        private ExifMetadata exifMataData = new ExifMetadata();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExifHelper()
        {
            exifMataData.EquipmentMake.Hex = "10f";
            exifMataData.CameraModel.Hex = "110";
            exifMataData.DatePictureTaken.Hex = "9003";
            exifMataData.ExposureTime.Hex = "829a";
            exifMataData.Fstop.Hex = "829d";
            exifMataData.ShutterSpeed.Hex = "9201";
            exifMataData.MeteringMode.Hex = "9207";
            exifMataData.Flash.Hex = "9209";
            exifMataData.FNumber.Hex = "829d";
            exifMataData.ExposureProg.Hex = "8822";
            exifMataData.SpectralSense.Hex = "8824";
            exifMataData.ISOSpeed.Hex = "8827";
            exifMataData.OECF.Hex = "8828";
            exifMataData.Ver.Hex = "9000";
            exifMataData.CompConfig.Hex = "9101";
            exifMataData.CompBPP.Hex = "9102";
            exifMataData.Aperture.Hex = "9202";
            exifMataData.Brightness.Hex = "9203";
            exifMataData.ExposureBias.Hex = "9204";
            exifMataData.MaxAperture.Hex = "9205";
            exifMataData.SubjectDist.Hex = "9206";
            exifMataData.LightSource.Hex = "9208";
            exifMataData.FocalLength.Hex = "920a";
            exifMataData.FPXVer.Hex = "a000";
            exifMataData.ColorSpace.Hex = "a001";
            exifMataData.FocalXRes.Hex = "a20e";
            exifMataData.FocalYRes.Hex = "a20f";
            exifMataData.FocalResUnit.Hex = "a210";
            exifMataData.ExposureIndex.Hex = "a215";
            exifMataData.SensingMethod.Hex = "a217";
            exifMataData.SceneType.Hex = "a301";
            exifMataData.CfaPattern.Hex = "a302";
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 获取Image Exif信息
        /// </summary>
        /// <param name="iamgePath">图片路径</param>
        /// <returns>Exif信息</returns>
        public ExifMetadata GetMetaData(string iamgePath)
        {
            Image _pictrue = Image.FromFile(iamgePath);
            int[] _pictrueProperty = _pictrue.PropertyIdList;
            PropertyItem[] _pictrueExif = new PropertyItem[_pictrueProperty.Length];
            ASCIIEncoding _asciiEncoding = new ASCIIEncoding();
            int _index = 0;
            int _picPropertyItemCount = _pictrueProperty.Length;

            if (_picPropertyItemCount != 0)
            {
                foreach (int propertyId in _pictrueProperty)
                {
                    _pictrueExif[_index] = _pictrue.GetPropertyItem(propertyId);
                    string _propertyIdString = _pictrue.GetPropertyItem(propertyId).Id.ToString("x");
                    GetExifMetaDataBy_10f(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_110(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9003(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9207(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9209(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_829a(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_829d(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9201(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_8822(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_8824(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_8827(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_8828(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9000(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9101(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9102(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9202(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9203(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9204(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9205(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9206(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_9208(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_920a(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a000(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a001(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a20e(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a20f(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a210(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a215(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a217(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a301(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    GetExifMetaDataBy_a302(_pictrue, _asciiEncoding, _propertyIdString, propertyId, _index, _pictrueExif);
                    _index++;
                }
            }

            exifMataData.XResolution.DisplayValue = _pictrue.HorizontalResolution.ToString();
            exifMataData.YResolution.DisplayValue = _pictrue.VerticalResolution.ToString();
            exifMataData.ImageHeight.DisplayValue = _pictrue.Height.ToString();
            exifMataData.ImageWidth.DisplayValue = _pictrue.Width.ToString();
            _pictrue.Dispose();
            return exifMataData;
        }

        /// <summary>
        /// 获取Long型的10进制数据
        /// </summary>
        /// <param name="hexString">16进制数据的字符串表示</param>
        /// <returns>Double型10进制数据</returns>
        private static double GetLongValue(string[] hexString)
        {
            int _length = hexString.Length;
            double _longValue = 0.0;

            for (int i = 0; i < _length; ++i)
            {
                _longValue += double.Parse(Convert.ToInt64(hexString[i], 16).ToString("d")) * Math.Pow(16, i * 2);
            }

            return _longValue;
        }

        /// <summary>
        /// 获取Long型的10进制数据
        /// </summary>
        /// <param name="longValueLow">Long型数据高位</param>
        /// <param name="longValueHigh">Long型数据低位</param>
        /// <returns>10进制数据</returns>
        /// 时间：2015-09-15 11:28
        /// 备注：
        private static double GetLongValue(string longValueLow, string longValueHigh)
        {
            double _longValue = double.Parse(Convert.ToInt64(longValueLow, 16).ToString("d")) + double.Parse(Convert.ToInt64(longValueHigh, 16).ToString("d")) * Math.Pow(16, 2);
            return _longValue;
        }

        /// <summary>
        /// 计算Rational数据
        /// </summary>
        /// <param name="numerator">分子</param>
        /// <param name="denominator">分母</param>
        /// <returns>Rational数据</returns>
        private static double GetRationalValue(double numerator, double denominator)
        {
            double _value = numerator / denominator;
            return _value;
        }

        /// <summary>
        /// 获取Exif类型为10f的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:22
        /// 备注：
        private void GetExifMetaDataBy_10f(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "10f", true) == 0)
            {
                exifMataData.EquipmentMake.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.EquipmentMake.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为100的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:24
        /// 备注：
        private void GetExifMetaDataBy_110(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "110", true) == 0)
            {
                exifMataData.CameraModel.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.CameraModel.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为829a的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:24
        /// 备注：
        private void GetExifMetaDataBy_829a(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "829a", true) == 0)
            {
                exifMataData.ExposureTime.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                StringBuilder _builder = new StringBuilder();

                for (int offset = 0; offset < pictrue.GetPropertyItem(propertyId).Len; offset = offset + 4)
                {
                    _builder.AppendFormat("{0}/", BitConverter.ToInt32(pictrue.GetPropertyItem(propertyId).Value, offset).ToString());
                }

                string _builderString = _builder.ToString();
                exifMataData.ExposureTime.DisplayValue = _builderString.Substring(0, _builderString.Length - 1);
            }
        }

        /// <summary>
        /// 获取Exif类型为829d的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:24
        /// 备注：
        private void GetExifMetaDataBy_829d(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "829d", true) == 0)
            {
                pictrueExif[index] = pictrue.GetPropertyItem(propertyId);
                byte[] _eixfValueArr = pictrue.GetPropertyItem(propertyId).Value;
                string _itemValue = BitConverter.ToString(_eixfValueArr);
                string[] _longValueArr = _itemValue.Split('-');
                exifMataData.Fstop.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                string[] _valueArr = new string[4] { _longValueArr[0], _longValueArr[1], _longValueArr[2], _longValueArr[3] };
                double _exifValueLow = GetLongValue(_valueArr);
                _valueArr = new string[4] { _longValueArr[4], _longValueArr[5], _longValueArr[6], _longValueArr[7] };
                double _exifValueHigth = GetLongValue(_valueArr),
                       _exifValueRational = GetRationalValue(_exifValueLow, _exifValueHigth);
                exifMataData.FNumber.DisplayValue = "F " + _exifValueRational.ToString();
            }
        }

        /// <summary>
        /// 获取Exif类型为8822的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:24
        /// 备注：
        private void GetExifMetaDataBy_8822(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "8822", true) == 0)
            {
                exifMataData.ExposureProg.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.ExposureProg.DisplayValue = TransEXIF_ExposureProg("ExposureProg", BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString());
            }
        }

        /// <summary>
        /// 获取Exif类型为8824的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:24
        /// 备注：
        private void GetExifMetaDataBy_8824(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "8824", true) == 0)
            {
                exifMataData.SpectralSense.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.SpectralSense.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为8827的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:25
        /// 备注：
        private void GetExifMetaDataBy_8827(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "8827", true) == 0)
            {
                pictrueExif[index] = pictrue.GetPropertyItem(propertyId);
                byte[] _eixfValueArr = pictrue.GetPropertyItem(propertyId).Value;
                string _itemValue = BitConverter.ToString(_eixfValueArr);
                string[] _longValueArr = _itemValue.Split('-');
                string _hexVal = string.Empty;
                exifMataData.ISOSpeed.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                int length = _longValueArr.Length;
                double longValue = 0.0;

                for (int i = 0; i < length; ++i)
                {
                    longValue += double.Parse(Convert.ToInt64(_longValueArr[i], 16).ToString("d")) * Math.Pow(16, i * 2);
                }

                exifMataData.ISOSpeed.DisplayValue = longValue.ToString();
            }
        }

        /// <summary>
        /// 获取Exif类型为8828的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:25
        /// 备注：
        private void GetExifMetaDataBy_8828(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "8828", true) == 0)
            {
                exifMataData.OECF.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.OECF.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为9000的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:25
        /// 备注：
        private void GetExifMetaDataBy_9000(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9000", true) == 0)
            {
                exifMataData.Ver.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.Ver.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value).Substring(1, 1) + "." + asciiEncoding.GetString(pictrueExif[index].Value).Substring(2, 2);
            }
        }

        /// <summary>
        /// 获取Exif类型为9003的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:25
        /// 备注：
        private void GetExifMetaDataBy_9003(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9003", true) == 0)
            {
                exifMataData.DatePictureTaken.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.DatePictureTaken.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为9101的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:25
        /// 备注：
        private void GetExifMetaDataBy_9101(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9101", true) == 0)
            {
                exifMataData.CompConfig.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.CompConfig.DisplayValue = TransEXIF_CompConfig("CompConfig", BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString());
            }
        }

        /// <summary>
        /// 获取Exif类型为9102的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:25
        /// 备注：
        private void GetExifMetaDataBy_9102(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9102", true) == 0)
            {
                exifMataData.CompBPP.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.CompBPP.DisplayValue = BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString();
            }
        }

        /// <summary>
        /// 获取Exif类型为9201的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:26
        /// 备注：
        private void GetExifMetaDataBy_9201(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9201", true) == 0)
            {
                exifMataData.ShutterSpeed.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                string _shutterSpeed = BitConverter.ToInt32(pictrue.GetPropertyItem(propertyId).Value, 0).ToString();
                exifMataData.ShutterSpeed.DisplayValue = "1/" + _shutterSpeed + "s";
            }
        }

        /// <summary>
        /// 获取Exif类型为9202的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:26
        /// 备注：
        private void GetExifMetaDataBy_9202(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9202", true) == 0)
            {
                string _hexVal = string.Empty;
                exifMataData.Aperture.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                _hexVal = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value).Substring(0, 2);
                _hexVal = Convert.ToInt32(_hexVal, 16).ToString();
                _hexVal = _hexVal + "00";
                exifMataData.Aperture.DisplayValue = _hexVal.Substring(0, 1) + "." + _hexVal.Substring(1, 2);
            }
        }

        /// <summary>
        /// 获取Exif类型为9203的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:26
        /// 备注：
        private void GetExifMetaDataBy_9203(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9203", true) == 0)
            {
                string _hexVal = string.Empty;
                exifMataData.Brightness.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                _hexVal = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value).Substring(0, 2);
                _hexVal = Convert.ToInt32(_hexVal, 16).ToString();
                _hexVal = _hexVal + "00";
                exifMataData.Brightness.DisplayValue = _hexVal.Substring(0, 1) + "." + _hexVal.Substring(1, 2);
            }
        }

        /// <summary>
        /// 获取Exif类型为9204的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:26
        /// 备注：
        private void GetExifMetaDataBy_9204(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9204", true) == 0)
            {
                exifMataData.ExposureBias.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.ExposureBias.DisplayValue = BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString();
            }
        }

        /// <summary>
        /// 获取Exif类型为9205的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:26
        /// 备注：
        private void GetExifMetaDataBy_9205(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9205", true) == 0)
            {
                string _hexVal = string.Empty;
                exifMataData.MaxAperture.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                _hexVal = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value).Substring(0, 2);
                _hexVal = Convert.ToInt32(_hexVal, 16).ToString();
                _hexVal = _hexVal + "00";
                exifMataData.MaxAperture.DisplayValue = _hexVal.Substring(0, 1) + "." + _hexVal.Substring(1, 2);
            }
        }

        /// <summary>
        /// 获取Exif类型为9206的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:26
        /// 备注：
        private void GetExifMetaDataBy_9206(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9206", true) == 0)
            {
                exifMataData.SubjectDist.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.SubjectDist.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为9207的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:26
        /// 备注：
        private void GetExifMetaDataBy_9207(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9207", true) == 0)
            {
                exifMataData.MeteringMode.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.MeteringMode.DisplayValue = TransEXIF_MeteringMode("MeteringMode", BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString());
            }
        }

        /// <summary>
        /// 获取Exif类型为9208的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:27
        /// 备注：
        private void GetExifMetaDataBy_9208(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9208", true) == 0)
            {
                exifMataData.LightSource.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.LightSource.DisplayValue = TransEXIF_LightSource("LightSource", BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString());
            }
        }

        /// <summary>
        /// 获取Exif类型为9209的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:27
        /// 备注：
        private void GetExifMetaDataBy_9209(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "9209", true) == 0)
            {
                exifMataData.Flash.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.Flash.DisplayValue = TransEXIF_Flash("Flash", BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString());
            }
        }

        /// <summary>
        /// 获取Exif类型为920a的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:27
        /// 备注：
        private void GetExifMetaDataBy_920a(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "920a", true) == 0)
            {
                string _hexVal = string.Empty;
                exifMataData.FocalLength.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                _hexVal = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value).Substring(0, 2);
                _hexVal = Convert.ToInt32(_hexVal, 16).ToString();
                _hexVal = _hexVal + "00";
                exifMataData.FocalLength.DisplayValue = _hexVal.Substring(0, 1) + "." + _hexVal.Substring(1, 2);
            }
        }

        /// <summary>
        /// 获取Exif类型为a000的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:27
        /// 备注：
        private void GetExifMetaDataBy_a000(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a000", true) == 0)
            {
                exifMataData.FPXVer.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.FPXVer.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value).Substring(1, 1) + "." + asciiEncoding.GetString(pictrueExif[index].Value).Substring(2, 2);
            }
        }

        /// <summary>
        /// 获取Exif类型为a001的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:27
        /// 备注：
        private void GetExifMetaDataBy_a001(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a001", true) == 0)
            {
                exifMataData.ColorSpace.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);

                if (BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString() == "1")
                {
                    exifMataData.ColorSpace.DisplayValue = "RGB";
                }

                if (BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString() == "65535")
                {
                    exifMataData.ColorSpace.DisplayValue = "Uncalibrated";
                }
            }
        }

        /// <summary>
        /// 获取Exif类型为a20e的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:27
        /// 备注：
        private void GetExifMetaDataBy_a20e(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a20e", true) == 0)
            {
                exifMataData.FocalXRes.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.FocalXRes.DisplayValue = BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString();
            }
        }

        /// <summary>
        /// 获取Exif类型为a20f的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:27
        /// 备注：
        private void GetExifMetaDataBy_a20f(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a20f", true) == 0)
            {
                exifMataData.FocalYRes.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.FocalYRes.DisplayValue = BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString();
            }
        }

        /// <summary>
        /// 获取Exif类型为a210的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:28
        /// 备注：
        private void GetExifMetaDataBy_a210(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a210", true) == 0)
            {
                exifMataData.FocalResUnit.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                string _unitValue = BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString();

                if (_unitValue == "1")
                {
                    exifMataData.FocalResUnit.DisplayValue = "没有单位";
                }

                if (_unitValue == "2")
                {
                    exifMataData.FocalResUnit.DisplayValue = "英尺";
                }

                if (_unitValue == "3")
                {
                    exifMataData.FocalResUnit.DisplayValue = "厘米";
                }
            }
        }

        /// <summary>
        /// 获取Exif类型为a215的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:28
        /// 备注：
        private void GetExifMetaDataBy_a215(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a215", true) == 0)
            {
                exifMataData.ExposureIndex.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.ExposureIndex.DisplayValue = asciiEncoding.GetString(pictrueExif[index].Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为a217的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:28
        /// 备注：
        private void GetExifMetaDataBy_a217(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a217", true) == 0)
            {
                string _sensingValue = BitConverter.ToInt16(pictrue.GetPropertyItem(propertyId).Value, 0).ToString();
                exifMataData.SensingMethod.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);

                if (_sensingValue == "2")
                {
                    exifMataData.SensingMethod.DisplayValue = "1 chip color area sensor";
                }
            }
        }

        /// <summary>
        /// 获取Exif类型为a301的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:28
        /// 备注：
        private void GetExifMetaDataBy_a301(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a301", true) == 0)
            {
                exifMataData.SceneType.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.SceneType.DisplayValue = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
            }
        }

        /// <summary>
        /// 获取Exif类型为a302的数据信息
        /// </summary>
        /// <param name="pictrue">图片.</param>
        /// <param name="asciiEncoding">字符编码</param>
        /// <param name="propertyIdString">id字符串</param>
        /// <param name="propertyId">id</param>
        /// <param name="index">索引</param>
        /// <param name="pictrueExif">PropertyItem数组</param>
        /// 时间：2015-09-15 11:28
        /// 备注：
        private void GetExifMetaDataBy_a302(Image pictrue, ASCIIEncoding asciiEncoding, string propertyIdString, int propertyId, int index, PropertyItem[] pictrueExif)
        {
            if (string.Compare(propertyIdString, "a302", true) == 0)
            {
                exifMataData.CfaPattern.RawValueAsString = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
                exifMataData.CfaPattern.DisplayValue = BitConverter.ToString(pictrue.GetPropertyItem(propertyId).Value);
            }
        }

        /// <summary>
        /// 将Aperture转换成对应的值信息
        /// </summary>
        /// <param name="description">类型描述</param>
        /// <param name="value">类型对应的元数据值</param>
        /// <returns>元数据所对应的EXIF信息</returns>
        /// 时间：2015-09-15 11:48
        /// 备注：
        private string TransEXIF_Aperture(string description, string value)
        {
            string _descriptionValue = string.Empty;

            if (string.Compare(description, "Aperture", true) == 0)
            {
                _descriptionValue = value;
            }

            return _descriptionValue;
        }

        /// <summary>
        /// 将LightSource转换成对应的值信息
        /// </summary>
        /// <param name="description">类型描述</param>
        /// <param name="value">类型对应的元数据值</param>
        /// <returns>元数据所对应的EXIF信息</returns>
        /// 时间：2015-09-15 11:48
        /// 备注：
        private string TransEXIF_CompConfig(string description, string value)
        {
            string _descriptionValue = string.Empty;

            if (string.Compare(description, "LightSource", true) == 0)
            {
                switch (value)
                {
                    case "513":
                        _descriptionValue = "YCbCr";
                        break;
                }
            }

            return _descriptionValue;
        }

        /// <summary>
        /// 将ExposureProg转换成对应的值信息
        /// </summary>
        /// <param name="description">类型描述</param>
        /// <param name="value">类型对应的元数据值</param>
        /// <returns>元数据所对应的EXIF信息</returns>
        /// 时间：2015-09-15 11:48
        /// 备注：
        private string TransEXIF_ExposureProg(string description, string value)
        {
            string _descriptionValue = string.Empty;

            if (string.Compare(description, "ExposureProg", true) == 0)
            {
                switch (value)
                {
                    case "0":
                        _descriptionValue = "没有定义";
                        break;

                    case "1":
                        _descriptionValue = "手动控制";
                        break;

                    case "2":
                        _descriptionValue = "程序控制";
                        break;

                    case "3":
                        _descriptionValue = "光圈优先";
                        break;

                    case "4":
                        _descriptionValue = "快门优先";
                        break;

                    case "5":
                        _descriptionValue = "夜景模式";
                        break;

                    case "6":
                        _descriptionValue = "运动模式";
                        break;

                    case "7":
                        _descriptionValue = "肖像模式";
                        break;

                    case "8":
                        _descriptionValue = "风景模式";
                        break;

                    case "9":
                        _descriptionValue = "保留的";
                        break;
                }
            }

            return _descriptionValue;
        }

        /// <summary>
        /// 将Flash转换成对应的值信息
        /// </summary>
        /// <param name="description">类型描述</param>
        /// <param name="value">类型对应的元数据值</param>
        /// <returns>元数据所对应的EXIF信息</returns>
        /// 时间：2015-09-15 11:48
        /// 备注：
        private string TransEXIF_Flash(string description, string value)
        {
            string _descriptionValue = string.Empty;

            if (string.Compare(description, "Flash", true) == 0)
            {
                switch (value)
                {
                    case "0":
                        _descriptionValue = "未使用";
                        break;

                    case "1":
                        _descriptionValue = "闪光";
                        break;

                    case "5":
                        _descriptionValue = "Flash fired but strobe return light not detected";
                        break;

                    case "7":
                        _descriptionValue = "Flash fired and strobe return light detected";
                        break;
                }
            }

            return _descriptionValue;
        }

        /// <summary>
        /// 将LightSource转换成对应的值信息
        /// </summary>
        /// <param name="description">类型描述</param>
        /// <param name="value">类型对应的元数据值</param>
        /// <returns>元数据所对应的EXIF信息</returns>
        /// 时间：2015-09-15 11:48
        /// 备注：
        private string TransEXIF_LightSource(string description, string value)
        {
            string _descriptionValue = string.Empty;

            if (string.Compare(description, "LightSource", true) == 0)
            {
                switch (value)
                {
                    case "0":
                        _descriptionValue = "未知";
                        break;

                    case "1":
                        _descriptionValue = "日光";
                        break;

                    case "2":
                        _descriptionValue = "荧光灯";
                        break;

                    case "3":
                        _descriptionValue = "白炽灯";
                        break;

                    case "10":
                        _descriptionValue = "闪光灯";
                        break;

                    case "17":
                        _descriptionValue = "标准光A";
                        break;

                    case "18":
                        _descriptionValue = "标准光B";
                        break;

                    case "19":
                        _descriptionValue = "标准光C";
                        break;

                    case "20":
                        _descriptionValue = "标准光D55";
                        break;

                    case "21":
                        _descriptionValue = "标准光D65";
                        break;

                    case "22":
                        _descriptionValue = "标准光D75";
                        break;

                    case "255":
                        _descriptionValue = "其它";
                        break;
                }
            }

            return _descriptionValue;
        }

        /// <summary>
        /// 将MeteringMode转换成对应的值信息
        /// </summary>
        /// <param name="description">类型描述</param>
        /// <param name="value">类型对应的元数据值</param>
        /// <returns>元数据所对应的EXIF信息</returns>
        /// 时间：2015-09-15 11:48
        /// 备注：
        private string TransEXIF_MeteringMode(string description, string value)
        {
            string _descriptionValue = string.Empty;

            if (string.Compare(description, "MeteringMode", true) == 0)
            {
                switch (value)
                {
                    case "0":
                        _descriptionValue = "Unknown";
                        break;

                    case "1":
                        _descriptionValue = "Average";
                        break;

                    case "2":
                        _descriptionValue = "Center Weighted Average";
                        break;

                    case "3":
                        _descriptionValue = "Spot";
                        break;

                    case "4":
                        _descriptionValue = "Multi-spot";
                        break;

                    case "5":
                        _descriptionValue = "Multi-segment";
                        break;

                    case "6":
                        _descriptionValue = "Partial";
                        break;

                    case "255":
                        _descriptionValue = "Other";
                        break;
                }
            }

            return _descriptionValue;
        }

        /// <summary>
        /// 将ResolutionUnit转换成对应的值信息
        /// </summary>
        /// <param name="description">类型描述</param>
        /// <param name="value">类型对应的元数据值</param>
        /// <returns>元数据所对应的EXIF信息</returns>
        /// 时间：2015-09-15 11:48
        /// 备注：
        private string TransEXIF_ResolutionUnit(string description, string value)
        {
            string _descriptionValue = string.Empty;

            if (string.Compare(description, "ResolutionUnit", true) == 0)
            {
                switch (value)
                {
                    case "1":
                        _descriptionValue = "No Units";
                        break;

                    case "2":
                        _descriptionValue = "Inch";
                        break;

                    case "3":
                        _descriptionValue = "Centimeter";
                        break;
                }
            }

            return _descriptionValue;
        }

        #endregion Methods
    }
}