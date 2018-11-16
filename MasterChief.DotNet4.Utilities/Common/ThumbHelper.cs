namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// 缩略图帮助类
    /// </summary>
    public class ThumbHelper
    {
        #region Methods

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式: HW=指定高宽,W=指定宽，高按比例, H=指定高，宽按比例, Cut=指定高宽裁减(不变形), DB=等比缩放（不变形，如果高大按高，宽大按宽缩放） </param>
        /// <param name="type">缩略图的类型：JPG, PNG, GIF, BMP</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, string type)
        {
            Image _originalImage = Image.FromFile(originalImagePath);
            int _towidth = width;
            int _toheight = height;
            int _x = 0;
            int _y = 0;
            int _ow = _originalImage.Width;
            int _oh = _originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）
                    break;

                case "W"://指定宽，高按比例
                    _toheight = _originalImage.Height * width / _originalImage.Width;
                    break;

                case "H"://指定高，宽按比例
                    _towidth = _originalImage.Width * height / _originalImage.Height;
                    break;

                case "Cut"://指定高宽裁减（不变形）
                    if ((double)_originalImage.Width / (double)_originalImage.Height > _towidth / (double)_toheight)
                    {
                        _oh = _originalImage.Height;
                        _ow = _originalImage.Height * _towidth / _toheight;
                        _y = 0;
                        _x = (_originalImage.Width - _ow) / 2;
                    }
                    else
                    {
                        _ow = _originalImage.Width;
                        _oh = _originalImage.Width * height / _towidth;
                        _x = 0;
                        _y = (_originalImage.Height - _oh) / 2;
                    }

                    break;

                case "DB"://等比缩放（不变形，如果高大按高，宽大按宽缩放）
                    if ((double)_originalImage.Width / _towidth < (double)_originalImage.Height / _toheight)
                    {
                        _toheight = height;
                        _towidth = _originalImage.Width * height / _originalImage.Height;
                    }
                    else
                    {
                        _towidth = width;
                        _toheight = _originalImage.Height * width / _originalImage.Width;
                    }

                    break;

                default:
                    break;
            }

            Image _bitmap = new Bitmap(_towidth, _toheight); //新建一个bmp图片
            Graphics _g = Graphics.FromImage(_bitmap); //新建一个画板
            _g.InterpolationMode = InterpolationMode.High; //设置高质量插值法
            _g.SmoothingMode = SmoothingMode.HighQuality; //设置高质量,低速度呈现平滑程度
            _g.Clear(Color.Transparent);  //清空画布并以透明背景色填充
            _g.DrawImage(_originalImage, new Rectangle(0, 0, _towidth, _toheight), new Rectangle(_x, _y, _ow, _oh), GraphicsUnit.Pixel); //在指定位置并且按指定大小绘制原图片的指定部分

            try
            {
                //保存缩略图
                if (type == "JPG")
                {
                    _bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
                }
                else if (type == "BMP")
                {
                    _bitmap.Save(thumbnailPath, ImageFormat.Bmp);
                }
                else if (type == "GIF")
                {
                    _bitmap.Save(thumbnailPath, ImageFormat.Gif);
                }
                else if (type == "PNG")
                {
                    _bitmap.Save(thumbnailPath, ImageFormat.Png);
                }
                else
                {
                    _bitmap.Save(thumbnailPath, _originalImage.RawFormat);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _originalImage.Dispose();
                _bitmap.Dispose();
                _g.Dispose();
            }
        }

        #endregion Methods
    }
}