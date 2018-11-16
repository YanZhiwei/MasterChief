namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    ///Image帮助类
    /// </summary>
    public static class ImageHelper
    {
        #region Fields

        /// <summary>
        /// 图片允许的格式
        /// </summary>
        public const string AllowExt = ".jpeg|.jpg|.bmp|.gif|.png";

        #endregion Fields

        #region Methods

        /// <summary>
        /// 调节图片亮度值
        /// </summary>
        /// <param name="image">需要处理的图片</param>
        /// <param name="brightnessValue">亮度值【0~100，其中0表示最暗，100表示最亮】</param>
        /// <returns>调节好后的图片</returns>
        public static Bitmap AdjustBrightness(this Bitmap image, int brightnessValue)
        {
            /*
             *参考：
             *1. http://www.nullskull.com/faq/528/c-net--change-brightness-of-image--jpg-gif-or-bmp.aspx
             *2. http://blog.csdn.net/jiangxinyu/article/details/6222302
             *3. http://www.smokycogs.com/blog/image-processing-in-c-sharp-adjusting-the-brightness/
             */
            ValidateOperator.Begin().InRange(brightnessValue, 0, 100, "图片亮度值");
            brightnessValue = 100 - brightnessValue;
            Bitmap _tempBitmap = image;
            float _finalValue = brightnessValue / 255.0f;
            Bitmap _newBitmap = new Bitmap(_tempBitmap.Width, _tempBitmap.Height);
            Graphics _newGraphics = Graphics.FromImage(_newBitmap);
            float[][] _floatColorMatrix =
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {_finalValue, _finalValue, _finalValue, 1, 1}
            };
            ColorMatrix _newColorMatrix = new ColorMatrix(_floatColorMatrix);
            ImageAttributes _attributes = new ImageAttributes();
            _attributes.SetColorMatrix(_newColorMatrix);
            _newGraphics.DrawImage(_tempBitmap, new Rectangle(0, 0, _tempBitmap.Width, _tempBitmap.Height), 0, 0, _tempBitmap.Width, _tempBitmap.Height, GraphicsUnit.Pixel, _attributes);
            _attributes.Dispose();
            _newGraphics.Dispose();
            return _newBitmap;
        }

        /// <summary>
        /// 获取Bitmap基本信息
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns>Bitmap基本信息</returns>
        public static BitmapInfo GetBitmapInfo(string path)
        {
            ValidateOperator.Begin().NotNullOrEmpty(path, "图片路径").IsFilePath(path);
            using (Bitmap sourceBmp = new Bitmap(path))
            {
                return new BitmapInfo() { Height = sourceBmp.Width, Width = sourceBmp.Width };
            }
        }

        ///<summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="watermarkImageFile">水印图片</param>
        /// <param name="sourceImageFile">原文件</param>
        /// <param name="position">水印位置</param>
        /// 时间：2015-12-17 15:48
        /// 备注：
        public static void AttachPng(string watermarkImageFile, string sourceImageFile, SetWaterPosition position)
        {
            CheckedAttachPngParamters(watermarkImageFile, sourceImageFile);
            string _sourceFileTemp = sourceImageFile.CreateTempFilePath();
            Image _sourceImage = Image.FromFile(_sourceFileTemp);
            ImageFormat _sourceImageFormat = _sourceImage.RawFormat;
            int _sourceImageHight = _sourceImage.Height,
                _sourceImageWidth = _sourceImage.Width;
            using (Bitmap sourceBmp = new Bitmap(_sourceImageWidth, _sourceImageHight))
            {
                using (Graphics graphics = Graphics.FromImage(sourceBmp))
                {
                    // 设置画布的描绘质量
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(_sourceImage, new Rectangle(0, 0, _sourceImageWidth, _sourceImageHight),
                                       0, 0, _sourceImageWidth, _sourceImageHight, GraphicsUnit.Pixel);
                    _sourceImage.Dispose();
                    _sourceImage = Image.FromFile(watermarkImageFile);
                    SetWaterMarkImagePosition(graphics, _sourceImage, _sourceImageWidth, _sourceImageHight, position);
                }
                SetWaterMarkImageQuality(sourceBmp, sourceImageFile, _sourceImageFormat);
                _sourceImage.Dispose();
            }
            File.Delete(_sourceFileTemp);
        }

        /// <summary>
        /// 添加水印文字
        /// </summary>
        /// <param name="waterText">水印文字</param>
        /// <param name="sourceImageFile">原文件</param>
        /// 时间：2015-12-18 9:32
        /// 备注：
        public static void AttachText(string waterText, string sourceImageFile)
        {
            string _sourceFileTemp = sourceImageFile.CreateTempFilePath();
            using (Image sourceImage = Image.FromFile(_sourceFileTemp))
            {
                ImageFormat _sourceImageFormat = sourceImage.RawFormat;
                int _sourceImageHight = sourceImage.Height,
                    _sourceImageWidth = sourceImage.Width;
                using (Bitmap sourceBmp = new Bitmap(_sourceImageWidth, _sourceImageHight))
                {
                    using (Graphics graphics = Graphics.FromImage(sourceBmp))
                    {
                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(sourceImage, new Rectangle(0, 0, _sourceImageWidth, _sourceImageHight),
                                           0, 0, _sourceImageWidth, _sourceImageHight, GraphicsUnit.Pixel);
                        SetWaterMarkTextPosition(graphics, waterText, sourceImage, _sourceImageWidth, _sourceImageHight);
                    }
                    SetWaterMarkImageQuality(sourceBmp, sourceImageFile, _sourceImageFormat);
                }
            }
            File.Delete(_sourceFileTemp);
        }

        /// <summary>
        /// 通过比较bitmap两者byte[]来判断是否相等
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByArray(this Bitmap originalImage, Bitmap compareImage)
        {
            /*
            *参考：
            *1. http://www.cnblogs.com/zgqys1980/archive/2009/07/13/1522546.html
            */
            IntPtr _result = new IntPtr(-1);
            MemoryStream _ms = new MemoryStream();

            try
            {
                originalImage.Save(_ms, ImageFormat.Png);
                byte[] _b1Array = _ms.ToArray();
                _ms.Position = 0;
                compareImage.Save(_ms, ImageFormat.Png);
                byte[] _b2Array = _ms.ToArray();
                _result = memcmp(_b1Array, _b2Array, new IntPtr(_b1Array.Length));
            }
            finally
            {
                _ms.Close();
            }

            return _result.ToInt32() == 0;
        }

        /// <summary>
        /// 通过比较bitmap两者ToBase64String来判断是否相等
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByBase64(this Bitmap originalImage, Bitmap compareImage)
        {
            /*
            *参考
            *1. http://blogs.msdn.com/b/domgreen/archive/2009/09/06/comparing-two-images-in-c.aspx
            */
            string _b1Base64String, _b2Base64String;
            MemoryStream _ms = new MemoryStream();

            try
            {
                originalImage.Save(_ms, ImageFormat.Png);
                _b1Base64String = Convert.ToBase64String(_ms.ToArray());
                _ms.Position = 0;
                compareImage.Save(_ms, ImageFormat.Png);
                _b2Base64String = Convert.ToBase64String(_ms.ToArray());
            }
            finally
            {
                _ms.Close();
            }

            return _b1Base64String.Equals(_b2Base64String);
        }

        /// <summary>
        /// 通过比较bitmap两者memcmp来判断是否相等
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByMemCmp(this Bitmap originalImage, Bitmap compareImage)
        {
            /*
             *参考：
             *1. http://stackoverflow.com/questions/2031217/what-is-the-fastest-way-i-can-compare-two-equal-size-bitmaps-to-determine-whethe
             */
            if ((originalImage == null) != (compareImage == null))
            {
                return false;
            }

            if (originalImage.Size != compareImage.Size)
            {
                return false;
            }

            BitmapData _bdata1 = originalImage.LockBits(new Rectangle(new Point(0, 0), originalImage.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData _bdata2 = compareImage.LockBits(new Rectangle(new Point(0, 0), compareImage.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr _bd1scan0 = _bdata1.Scan0;
                IntPtr _bd2scan0 = _bdata2.Scan0;
                int _stride = _bdata1.Stride;
                int _len = _stride * originalImage.Height;
                return memcmp(_bd1scan0, _bd2scan0, _len) == 0;
            }
            finally
            {
                originalImage.UnlockBits(_bdata1);
                compareImage.UnlockBits(_bdata2);
            }
        }

        /// <summary>
        /// 通过比较bitmap两者像素颜色来判断两者是否相等
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByPixel(this Bitmap originalImage, Bitmap compareImage)
        {
            /*
             *参考：
             *1. http://blogs.msdn.com/b/domgreen/archive/2009/09/06/comparing-two-images-in-c.aspx
             */
            bool _flag = false;

            if (originalImage.Width == compareImage.Width && originalImage.Height == compareImage.Height)
            {
                _flag = true;
                Color _pixel1;
                Color _pixel2;

                for (int i = 0; i < originalImage.Width; i++)
                {
                    for (int j = 0; j < originalImage.Height; j++)
                    {
                        _pixel1 = originalImage.GetPixel(i, j);
                        _pixel2 = compareImage.GetPixel(i, j);

                        if (_pixel1 != _pixel2)
                        {
                            _flag = false;
                            break;
                        }
                    }
                }
            }

            return _flag;
        }

        /// <summary>
        /// 将图片压缩到指定大小
        /// </summary>
        /// <param name="originalImagePath">待压缩图片</param>
        /// <param name="size">期望压缩后的尺寸</param>
        public static void CompressPhoto(string originalImagePath, int size)
        {
            int _count = 0;
            FileInfo _imgFile = new FileInfo(originalImagePath);
            long _imgLength = _imgFile.Length;

            while (_imgLength > size * 1024 && _count < 10)
            {
                string _directory = _imgFile.Directory.FullName;
                string _tempFile = Path.Combine(_directory, Guid.NewGuid().ToString() + "." + _imgFile.Extension);
                _imgFile.CopyTo(_tempFile, true);
                KiSaveAsJPEG(_tempFile, originalImagePath, 70);

                try
                {
                    File.Delete(_tempFile);
                }
                catch { }

                _count++;
                _imgFile = new FileInfo(originalImagePath);
                _imgLength = _imgFile.Length;
            }
        }

        /// <summary>
        /// 生成缩略图，不加水印
        /// </summary>
        /// <param name="originalImagePath">源文件</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        public static void CreateSmallPhoto(string originalImagePath, int width, int height, string destfile)
        {
            using (Image sourceImg = Image.FromFile(originalImagePath))
            {
                ImageFormat _imageFormat = sourceImg.RawFormat;
                Size _imageCutSize = CutRegion(width, height, sourceImg);
                using (Bitmap sourceBmp = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage(sourceBmp))
                    {
                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        int _startX = (sourceImg.Width - _imageCutSize.Width) / 2;
                        int _startY = (sourceImg.Height - _imageCutSize.Height) / 2;
                        graphics.DrawImage(sourceImg, new Rectangle(0, 0, width, height),
                                           _startX, _startY, _imageCutSize.Width, _imageCutSize.Height, GraphicsUnit.Pixel);
                    }
                    SetWaterMarkImageQuality(sourceBmp, destfile, _imageFormat);
                }
            }
        }

        /// <summary>
        /// 生成缩略图，不加水印
        /// </summary>
        /// <param name="sourceImageFile">源文件</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        /// <param name="cMode">裁剪模式</param>
        public static void CreateSmallPhoto(string sourceImageFile, int width, int height, string destfile, CutType cMode)
        {
            using (Image _sourceImg = Image.FromFile(sourceImageFile))
            {
                if (width <= 0)
                {
                    width = _sourceImg.Width;
                }

                if (height <= 0)
                {
                    height = _sourceImg.Height;
                }

                int _towidth = width,
                    _toheight = height;

                switch (cMode)
                {
                    case CutType.CutWH://指定高宽缩放（可能变形）
                        break;

                    case CutType.CutW://指定宽，高按比例
                        _toheight = _sourceImg.Height * width / _sourceImg.Width;
                        break;

                    case CutType.CutH://指定高，宽按比例
                        _towidth = _sourceImg.Width * height / _sourceImg.Height;
                        break;

                    case CutType.CutNo: //缩放不剪裁
                        int maxSize = (width >= height ? width : height);

                        if (_sourceImg.Width >= _sourceImg.Height)
                        {
                            _towidth = maxSize;
                            _toheight = _sourceImg.Height * maxSize / _sourceImg.Width;
                        }
                        else
                        {
                            _toheight = maxSize;
                            _towidth = _sourceImg.Width * maxSize / _sourceImg.Height;
                        }

                        break;

                    default:
                        break;
                }

                width = _towidth;
                height = _toheight;
                ImageFormat _imageFormat = _sourceImg.RawFormat;
                Size _imageCutSize = new Size(width, height);

                if (cMode != CutType.CutNo)
                {
                    _imageCutSize = CutRegion(width, height, _sourceImg);
                }

                using (Bitmap sourceBmp = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage(sourceBmp))
                    {
                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        int _startX = (_sourceImg.Width - _imageCutSize.Width) / 2;
                        int _startY = (_sourceImg.Height - _imageCutSize.Height) / 2;
                        graphics.DrawImage(_sourceImg, new Rectangle(0, 0, width, height),
                                           _startX, _startY, _imageCutSize.Width, _imageCutSize.Height, GraphicsUnit.Pixel);
                    }
                    SetWaterMarkImageQuality(sourceBmp, destfile, _imageFormat);
                }
            }
        }

        /// <summary>
        /// 获取图片格式
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>ImageFormat</returns>
        /// 时间：2015-12-17 14:21
        /// 备注：
        public static ImageFormat GetImageFormat(this string imagePath)
        {
            Image _sourceImage = Image.FromFile(imagePath);
            ImageFormat _sourceImageFormat = _sourceImage.RawFormat;
            return _sourceImageFormat;
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="mergerImageWidth">合并图片的宽度</param>
        /// <param name="mergerImageHeight">合并图片的高度</param>
        /// <param name="imageX">所绘制图像的左上角的X坐标</param>
        /// <param name="imageY">所绘制图像的左上角的Y坐标</param>
        /// <param name="backgroundColor">合并图片的背景色</param>
        /// <param name="maps">所需要绘制的图片集合</param>
        /// <returns>绘制的图片</returns>
        public static Bitmap MergerImage(int mergerImageWidth, int mergerImageHeight, int imageX, int imageY, Color backgroundColor, params Bitmap[] maps)
        {
            int _imageCount = maps.Length;
            //创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap _drawImage = new Bitmap(mergerImageWidth, mergerImageHeight);
            Graphics _graphics = Graphics.FromImage(_drawImage);

            try
            {
                //清除画布,背景设置为白色
                _graphics.Clear(Color.White);

                for (int j = 0; j < _imageCount; j++)
                {
                    _graphics.DrawImage(maps[j], j * imageX, imageY, maps[j].Width, maps[j].Height);
                }
            }
            finally
            {
                _graphics.Dispose();
            }

            return _drawImage;
        }

        /// <summary>
        /// 将Base64字符串转换成Image
        /// </summary>
        /// <param name="base64String">Base64字符串</param>
        /// <returns>Image</returns>
        public static Image ParseBase64String(this string base64String)
        {
            /*
            * 参考：
            * 1.http://www.dailycoding.com/Posts/convert_image_to_base64_string_and_base64_string_to_image.aspx
            */
            byte[] _imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(_imageBytes, 0, _imageBytes.Length))
            {
                ms.Write(_imageBytes, 0, _imageBytes.Length);
                Image _image = Image.FromStream(ms, true);
                return _image;
            }
        }

        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="buffer">二进制图片流</param>
        /// <returns>Image</returns>
        public static Image ParseByteArray(this byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                Image _saveImage = Image.FromStream(ms);
                ms.Flush();
                return _saveImage;
            }
        }

        /// <summary>
        /// 将Image转换Base64字符串
        /// </summary>
        /// <param name="sourceImageFile">图片文件</param>
        /// <param name="format">ImageFormat</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this Image sourceImageFile, ImageFormat format)
        {
            /*
             * 参考：
             * 1.http://www.dailycoding.com/Posts/convert_image_to_base64_string_and_base64_string_to_image.aspx
             */
            using (MemoryStream ms = new MemoryStream())
            {
                sourceImageFile.Save(ms, format);
                byte[] _imageBytes = ms.ToArray();
                string _base64String = Convert.ToBase64String(_imageBytes);
                return _base64String;
            }
        }

        /// <summary>
        /// 将.png,.bmp,.jpg类型图片转换成bitmap类型
        /// </summary>
        /// <param name="sourceImageFile">图片路径</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ToBitmap(string sourceImageFile)
        {
            return (Bitmap)Bitmap.FromFile(sourceImageFile, false);
        }

        /// <summary>
        /// 将图片转换成byte数组
        /// </summary>
        /// <param name="sourceImageFile">图片文件</param>
        /// <param name="imageFormat">ImageFormat</param>
        /// <returns>BYTE数组</returns>
        public static byte[] ToBytes(this Image sourceImageFile, ImageFormat imageFormat)
        {
            byte[] _data = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap bitmap = new Bitmap(sourceImageFile))
                {
                    bitmap.Save(ms, imageFormat);
                    ms.Position = 0;
                    _data = new byte[ms.Length];
                    ms.Read(_data, 0, Convert.ToInt32(ms.Length));
                    ms.Flush();
                }
            }
            return _data;
        }

        /// <summary>
        /// 将图片转换成byte数组
        /// </summary>
        /// <param name="sourceImageFile">图片文件</param>
        /// <returns>BYTE数组</returns>
        public static byte[] ToBytes(this Bitmap sourceImageFile)
        {
            return ToBytes(sourceImageFile, sourceImageFile.RawFormat);
        }

        /// <summary>
        /// 检查添加图片水印参数
        /// </summary>
        private static void CheckedAttachPngParamters(string watermarkImageFile, string sourceImageFile)
        {
            ValidateOperator.Begin().NotNullOrEmpty(watermarkImageFile, "水印图片路径")
            .CheckFileExists(watermarkImageFile)
            .NotNullOrEmpty(sourceImageFile, "水印图片路径")
            .CheckFileExists(sourceImageFile)
            ;
        }

        private static Size CreateImageSize(int width, int height, Image img)
        {
            double _w = 0.0,
                   _h = 0.0,
                   _sw = Convert.ToDouble(img.Width),
                   _sh = Convert.ToDouble(img.Height),
                   _mw = Convert.ToDouble(width),
                   _mh = Convert.ToDouble(height);

            if (_sw < _mw && _sh < _mh)
            {
                _w = _sw;
                _h = _sh;
            }
            else if ((_sw / _sh) > (_mw / _mh))
            {
                _w = width;
                _h = (_w * _sh) / _sw;
            }
            else
            {
                _h = height;
                _w = (_h * _sw) / _sh;
            }

            return new Size(Convert.ToInt32(_w), Convert.ToInt32(_h));
        }

        /// <summary>
        /// 根据需要的图片尺寸，按比例剪裁原始图片
        /// </summary>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="img">原始图片</param>
        /// <returns>剪裁区域尺寸</returns>
        private static Size CutRegion(int width, int height, Image img)
        {
            double _width = 0.0,
                   _height = 0.0;
            double _nw = width,
                   _nh = height,
                   _pw = (double)img.Width,
                   _ph = (double)img.Height;

            if (_nw / _nh > _pw / _ph)
            {
                _width = _pw;
                _height = _pw * _nh / _nw;
            }
            else if (_nw / _nh < _pw / _ph)
            {
                _width = _ph * _nw / _nh;
                _height = _ph;
            }
            else
            {
                _width = _pw;
                _height = _ph;
            }

            return new Size(Convert.ToInt32(_width), Convert.ToInt32(_height));
        }

        /// <summary>
        /// 保存JPG时用
        /// </summary>
        /// <param name="mimeType"> </param>
        /// <returns>得到指定mimeType的ImageCodecInfo </returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType)
                {
                    return ici;
                }
            }

            return null;
        }

        /// <summary>
        /// 保存为JPEG格式，支持压缩质量选项
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="fileName"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        private static bool KiSaveAsJPEG(string sourceFile, string fileName, int qty)
        {
            Bitmap _bmp = new Bitmap(sourceFile);

            try
            {
                EncoderParameter _p;
                EncoderParameters _ps;
                _ps = new EncoderParameters(1);
                _p = new EncoderParameter(Encoder.Quality, qty);
                _ps.Param[0] = _p;
                _bmp.Save(fileName, GetCodecInfo("image/jpeg"), _ps);
                _bmp.Dispose();
                return true;
            }
            catch
            {
                _bmp.Dispose();
                return false;
            }
        }

        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        /// <summary>
        /// memcmp API
        /// </summary>
        /// <param name="b1">字节数组1</param>
        /// <param name="b2">字节数组2</param>
        /// <param name="count">The count.</param>
        /// <returns>如果两个数组相同，返回0；如果数组1小于数组2，返回小于0的值；如果数组1大于数组2，返回大于0的值。</returns>
        /// 时间：2015-12-28 11:03
        /// 备注：
        [DllImport("msvcrt.dll")]
        private static extern IntPtr memcmp(byte[] b1, byte[] b2, IntPtr count);

        private static void SetWaterMarkImagePosition(Graphics sourceG, Image sourceImage, int sourceImageWidth, int sourceImageHight, SetWaterPosition position)
        {
            Size _sourceSize = CreateImageSize(sourceImageWidth, sourceImageHight, sourceImage);
            int _nx = 0,
                _ny = 0,
                _padding = 10;

            switch (position)
            {
                case SetWaterPosition.center:
                    _nx = (sourceImageWidth - _sourceSize.Width) / 2;
                    _ny = (sourceImageHight - _sourceSize.Height) / 2;
                    break;

                case SetWaterPosition.topLeft:
                    _nx = _padding;
                    _ny = _padding;
                    break;

                case SetWaterPosition.topRight:
                    _nx = (sourceImageWidth - _sourceSize.Width) - _padding;
                    _ny = _padding;
                    break;

                case SetWaterPosition.bottomLeft:
                    _nx = _padding;
                    _ny = (sourceImageHight - _sourceSize.Height) - _padding;
                    break;

                default:
                    _nx = (sourceImageWidth - _sourceSize.Width) - _padding;
                    _ny = (sourceImageHight - _sourceSize.Height) - _padding;
                    break;
            }

            sourceG.DrawImage(sourceImage, new Rectangle(_nx, _ny, _sourceSize.Width, _sourceSize.Height),
                              0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// 设置水印图片质量
        /// </summary>
        /// <param name="sourceBmp">The source BMP.</param>
        /// <param name="sourceImageFile">The source image file.</param>
        /// <param name="sourceImageFormat">The source image format.</param>
        /// 时间：2015-12-17 14:43
        /// 备注：
        private static void SetWaterMarkImageQuality(Bitmap sourceBmp, string sourceImageFile, ImageFormat sourceImageFormat)
        {
            // 以下代码为保存图片时，设置压缩质量
            EncoderParameters _encoderParams = new EncoderParameters();
            long[] _quality = new long[1] { 100 };
            EncoderParameter _encoderParam = new EncoderParameter(Encoder.Quality, _quality);
            _encoderParams.Param[0] = _encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] _arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo _jpegICI = null;

            for (int x = 0; x < _arrayICI.Length; x++)
            {
                if (_arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    _jpegICI = _arrayICI[x];//设置JPEG编码
                    break;
                }
            }

            if (_jpegICI != null)
            {
                sourceBmp.Save(sourceImageFile, _jpegICI, _encoderParams);
            }
            else
            {
                sourceBmp.Save(sourceImageFile, sourceImageFormat);
            }
        }

        private static void SetWaterMarkTextPosition(Graphics graphics, string text, Image _sourceImage, int _sourceImageWidth, int _sourceImageHight)
        {
            int[] _sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font _font = null;
            SizeF _crSize = new SizeF();

            //通过循环这个数组，来选用不同的字体大小
            //如果它的大小小于图像的宽度，就选用这个大小的字体
            for (int i = 0; i < 7; i++)
            {
                //设置字体，这里是用arial，黑体
                _font = new Font("arial", _sizes[i], FontStyle.Bold);
                _crSize = graphics.MeasureString(text, _font);

                if ((ushort)_crSize.Width < (ushort)_sourceImageWidth)
                {
                    break;
                }
            }

            //因为图片的高度可能不尽相同, 所以定义了
            //从图片底部算起预留了5%的空间
            int _yPixlesFromBottom = (int)(_sourceImageHight * .08);
            //现在使用版权信息字符串的高度来确定要绘制的图像的字符串的y坐标
            float _yPosFromBottom = ((_sourceImageHight - _yPixlesFromBottom) - (_crSize.Height / 2));
            //计算x坐标
            float _xCenterOfImg = (_sourceImageWidth / 2);
            //把文本布局设置为居中
            StringFormat _format = new StringFormat
            {
                Alignment = StringAlignment.Center
            };
            //通过Brush来设置黑色半透明
            SolidBrush _semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            //绘制版权字符串
            graphics.DrawString(text,                 //版权字符串文本
                                _font,                                   //字体
                                _semiTransBrush2,                           //Brush
                                new PointF(_xCenterOfImg + 1, _yPosFromBottom + 1),  //位置
                                _format);
            //设置成白色半透明
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            //第二次绘制版权字符串来创建阴影效果
            //记住移动文本的位置1像素
            graphics.DrawString(text,                 //版权文本
                                _font,                                   //字体
                                semiTransBrush,                           //Brush
                                new PointF(_xCenterOfImg, _yPosFromBottom),  //位置
                                _format);
        }

        #endregion Methods
    }
}