namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;

    using MasterChief.DotNet4.Utilities.Enum;

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
        /// <param name="lightValue">亮度值【0~100，其中0表示最暗，100表示最亮】</param>
        /// <returns>调节好后的图片</returns>
        public static Bitmap AdjustBrightness(this Bitmap image, int lightValue)
        {
            /*
             *参考：
             *1. http://www.nullskull.com/faq/528/c-net--change-brightness-of-image--jpg-gif-or-bmp.aspx
             *2. http://blog.csdn.net/jiangxinyu/article/details/6222302
             *3. http://www.smokycogs.com/blog/image-processing-in-c-sharp-adjusting-the-brightness/
             */

            lightValue = 100 - lightValue;
            Bitmap tmpImage = image;
            float _finalValue = lightValue / 255.0f;
            Bitmap newImage = new Bitmap(tmpImage.Width, tmpImage.Height);
            Graphics graphics = Graphics.FromImage(newImage);
            float[][] colorMatrix =
            {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {_finalValue, _finalValue, _finalValue, 1, 1}
            };
            ColorMatrix color = new ColorMatrix(colorMatrix);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(color);
            graphics.DrawImage(tmpImage, new Rectangle(0, 0, tmpImage.Width, tmpImage.Height), 0, 0, tmpImage.Width, tmpImage.Height, GraphicsUnit.Pixel, imageAttr);
            imageAttr.Dispose();
            graphics.Dispose();
            return newImage;
        }

        ///<summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="watermarkImageFile">水印图片</param>
        /// <param name="sourceImageFile">原文件</param>
        /// <param name="position">水印位置</param>
        /// 时间：2015-12-17 15:48
        /// 备注：
        public static void AttachPng(string watermarkImageFile, string sourceImageFile, ImgWaterPosition position)
        {
            string sourceImgTmpPath = sourceImageFile.CreateTempFilePath();
            Image sourceImg = Image.FromFile(sourceImgTmpPath);
            ImageFormat sourceImgFmt = sourceImg.RawFormat;
            int sourceImgH = sourceImg.Height,
                sourceImaW = sourceImg.Width;
            using (Bitmap sourceBmp = new Bitmap(sourceImaW, sourceImgH))
            {
                using (Graphics graphics = Graphics.FromImage(sourceBmp))
                {
                    // 设置画布的描绘质量
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(sourceImg, new Rectangle(0, 0, sourceImaW, sourceImgH),
                                       0, 0, sourceImaW, sourceImgH, GraphicsUnit.Pixel);
                    sourceImg.Dispose();
                    sourceImg = Image.FromFile(watermarkImageFile);
                    SetWaterMarkImagePosition(graphics, sourceImg, sourceImaW, sourceImgH, position);
                }

                SetWaterMarkImageQuality(sourceBmp, sourceImageFile, sourceImgFmt);
                sourceImg.Dispose();
            }
            File.Delete(sourceImgTmpPath);
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
            string sourceImgTmpPath = sourceImageFile.CreateTempFilePath();
            using (Image sourceImg = Image.FromFile(sourceImgTmpPath))
            {
                ImageFormat sourcImgFmt = sourceImg.RawFormat;
                int sourceImgH = sourceImg.Height,
                    sourceImgW = sourceImg.Width;
                using (Bitmap sourceBmp = new Bitmap(sourceImgW, sourceImgH))
                {
                    using (Graphics graphics = Graphics.FromImage(sourceBmp))
                    {
                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(sourceImg, new Rectangle(0, 0, sourceImgW, sourceImgH),
                                           0, 0, sourceImgW, sourceImgH, GraphicsUnit.Pixel);
                        SetWaterMarkTextPosition(graphics, waterText, sourceImg, sourceImgW, sourceImgH);
                    }
                    SetWaterMarkImageQuality(sourceBmp, sourceImageFile, sourcImgFmt);
                }
            }
            File.Delete(sourceImgTmpPath);
        }

        /// <summary>
        /// 通过比较bitmap两者byte[]来判断是否相等
        /// </summary>
        /// <param name="sourceImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByArray(this Bitmap sourceImage, Bitmap compareImage)
        {
            /*
            *参考：
            *1. http://www.cnblogs.com/zgqys1980/archive/2009/07/13/1522546.html
            */
            IntPtr result = new IntPtr(-1);
            MemoryStream stream = new MemoryStream();

            try
            {
                sourceImage.Save(stream, ImageFormat.Png);
                byte[] sourceImgArray = stream.ToArray();
                stream.Position = 0;
                compareImage.Save(stream, ImageFormat.Png);
                byte[] compareImgArray = stream.ToArray();
                result = memcmp(sourceImgArray, compareImgArray, new IntPtr(sourceImgArray.Length));
            }
            finally
            {
                stream.Close();
            }

            return result.ToInt32() == 0;
        }

        /// <summary>
        /// 通过比较bitmap两者ToBase64String来判断是否相等
        /// </summary>
        /// <param name="sourceImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByBase64(this Bitmap sourceImage, Bitmap compareImage)
        {
            /*
            *参考
            *1. http://blogs.msdn.com/b/domgreen/archive/2009/09/06/comparing-two-images-in-c.aspx
            */
            string sImgBase64, cImgBase64;
            MemoryStream memory = new MemoryStream();

            try
            {
                sourceImage.Save(memory, ImageFormat.Png);
                sImgBase64 = Convert.ToBase64String(memory.ToArray());
                memory.Position = 0;
                compareImage.Save(memory, ImageFormat.Png);
                cImgBase64 = Convert.ToBase64String(memory.ToArray());
            }
            finally
            {
                memory.Close();
            }

            return sImgBase64.Equals(cImgBase64);
        }

        /// <summary>
        /// 通过比较bitmap两者memcmp来判断是否相等
        /// </summary>
        /// <param name="sourceImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByMemCmp(this Bitmap sourceImage, Bitmap compareImage)
        {
            /*
             *参考：
             *1. http://stackoverflow.com/questions/2031217/what-is-the-fastest-way-i-can-compare-two-equal-size-bitmaps-to-determine-whethe
             */
            if ((sourceImage == null) != (compareImage == null))
            {
                return false;
            }

            if (sourceImage.Size != compareImage.Size)
            {
                return false;
            }

            BitmapData sImgData = sourceImage.LockBits(new Rectangle(new Point(0, 0), sourceImage.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData cImgData = compareImage.LockBits(new Rectangle(new Point(0, 0), compareImage.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr sIntPtr = sImgData.Scan0;
                IntPtr cItPtr = cImgData.Scan0;
                int stride = sImgData.Stride;
                int count = stride * sourceImage.Height;
                return memcmp(sIntPtr, cItPtr, count) == 0;
            }
            finally
            {
                sourceImage.UnlockBits(sImgData);
                compareImage.UnlockBits(cImgData);
            }
        }

        /// <summary>
        /// 通过比较bitmap两者像素颜色来判断两者是否相等
        /// </summary>
        /// <param name="sourceImage">原始图片</param>
        /// <param name="compareImage">需要比较的图片</param>
        /// <returns>比较结果</returns>
        public static bool CompareByPixel(this Bitmap sourceImage, Bitmap compareImage)
        {
            /*
             *参考：
             *1. http://blogs.msdn.com/b/domgreen/archive/2009/09/06/comparing-two-images-in-c.aspx
             */
            bool result = false;

            if (sourceImage.Width == compareImage.Width && sourceImage.Height == compareImage.Height)
            {
                result = true;
                Color sColor;
                Color cColor;

                for (int i = 0; i < sourceImage.Width; i++)
                {
                    for (int j = 0; j < sourceImage.Height; j++)
                    {
                        sColor = sourceImage.GetPixel(i, j);
                        cColor = compareImage.GetPixel(i, j);

                        if (sColor != cColor)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 将图片压缩到指定大小
        /// </summary>
        /// <param name="sourceImage">待压缩图片</param>
        /// <param name="size">期望压缩后的尺寸</param>
        public static void CompressPhoto(string sourceImage, int size)
        {
            int index = 0;
            FileInfo imgFile = new FileInfo(sourceImage);
            long imgCount = imgFile.Length;

            while (imgCount > size * 1024 && index < 10)
            {
                string imgFullName = imgFile.Directory.FullName;
                string imgTmpFile = Path.Combine(imgFullName, Guid.NewGuid().ToString() + "." + imgFile.Extension);
                imgFile.CopyTo(imgTmpFile, true);
                KiSaveAsJPEG(imgTmpFile, sourceImage, 70);

                try
                {
                    File.Delete(imgTmpFile);
                }
                catch { }

                index++;
                imgFile = new FileInfo(sourceImage);
                imgCount = imgFile.Length;
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
                ImageFormat imageFmt = sourceImg.RawFormat;
                Size imgSize = CutRegion(width, height, sourceImg);
                using (Bitmap sourceBmp = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage(sourceBmp))
                    {
                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        int startX = (sourceImg.Width - imgSize.Width) / 2;
                        int startY = (sourceImg.Height - imgSize.Height) / 2;
                        graphics.DrawImage(sourceImg, new Rectangle(0, 0, width, height),
                                           startX, startY, imgSize.Width, imgSize.Height, GraphicsUnit.Pixel);
                    }
                    SetWaterMarkImageQuality(sourceBmp, destfile, imageFmt);
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
        /// <param name="cutType">裁剪模式</param>
        public static void CreateSmallPhoto(string sourceImageFile, int width, int height, string destfile, ImgCutType cutType)
        {
            using (Image sourceImg = Image.FromFile(sourceImageFile))
            {
                if (width <= 0)
                {
                    width = sourceImg.Width;
                }

                if (height <= 0)
                {
                    height = sourceImg.Height;
                }

                int towidth = width,
                    toheight = height;

                switch (cutType)
                {
                    case ImgCutType.CutWh://指定高宽缩放（可能变形）
                        break;

                    case ImgCutType.CutW://指定宽，高按比例
                        toheight = sourceImg.Height * width / sourceImg.Width;
                        break;

                    case ImgCutType.CutH://指定高，宽按比例
                        towidth = sourceImg.Width * height / sourceImg.Height;
                        break;

                    case ImgCutType.CutNo: //缩放不剪裁
                        int maxSize = (width >= height ? width : height);

                        if (sourceImg.Width >= sourceImg.Height)
                        {
                            towidth = maxSize;
                            toheight = sourceImg.Height * maxSize / sourceImg.Width;
                        }
                        else
                        {
                            toheight = maxSize;
                            towidth = sourceImg.Width * maxSize / sourceImg.Height;
                        }

                        break;

                    default:
                        break;
                }

                width = towidth;
                height = toheight;
                ImageFormat imgRawFmt = sourceImg.RawFormat;
                Size imgSize = new Size(width, height);

                if (cutType != ImgCutType.CutNo)
                {
                    imgSize = CutRegion(width, height, sourceImg);
                }

                using (Bitmap sourceBmp = new Bitmap(width, height))
                {
                    using (Graphics graphics = Graphics.FromImage(sourceBmp))
                    {
                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        int _startX = (sourceImg.Width - imgSize.Width) / 2;
                        int _startY = (sourceImg.Height - imgSize.Height) / 2;
                        graphics.DrawImage(sourceImg, new Rectangle(0, 0, width, height),
                                           _startX, _startY, imgSize.Width, imgSize.Height, GraphicsUnit.Pixel);
                    }
                    SetWaterMarkImageQuality(sourceBmp, destfile, imgRawFmt);
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
            using (Image souceImg = Image.FromFile(imagePath))
            {
                return souceImg.RawFormat;
            }
        }

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="mergerImageWidth">合并图片的宽度</param>
        /// <param name="mergerImageHeight">合并图片的高度</param>
        /// <param name="imageX">所绘制图像的左上角的X坐标</param>
        /// <param name="imageY">所绘制图像的左上角的Y坐标</param>
        /// <param name="maps">所需要绘制的图片集合</param>
        /// <returns>绘制的图片</returns>
        public static Bitmap MergerImage(int mergerImageWidth, int mergerImageHeight, int imageX, int imageY, params Bitmap[] maps)
        {
            int imgCount = maps.Length;
            //创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap drawImage = new Bitmap(mergerImageWidth, mergerImageHeight);
            Graphics graphics = Graphics.FromImage(drawImage);

            try
            {
                //清除画布,背景设置为白色
                graphics.Clear(Color.White);

                for (int j = 0; j < imgCount; j++)
                {
                    graphics.DrawImage(maps[j], j * imageX, imageY, maps[j].Width, maps[j].Height);
                }
            }
            finally
            {
                graphics.Dispose();
            }

            return drawImage;
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
            byte[] imgArray = Convert.FromBase64String(base64String);
            using (MemoryStream stream = new MemoryStream(imgArray, 0, imgArray.Length))
            {
                stream.Write(imgArray, 0, imgArray.Length);
                return Image.FromStream(stream, true);
            }
        }

        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="buffer">二进制图片流</param>
        /// <returns>Image</returns>
        public static Image ParseByteArray(this byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                Image saveImage = Image.FromStream(stream);
                stream.Flush();
                return saveImage;
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
            using (MemoryStream stream = new MemoryStream())
            {
                sourceImageFile.Save(stream, format);
                byte[] buffer = stream.ToArray();
                return Convert.ToBase64String(buffer);
            }
        }

        /// <summary>
        /// 将图片转换成byte数组
        /// </summary>
        /// <param name="sourceImageFile">图片文件</param>
        /// <param name="imageFormat">ImageFormat</param>
        /// <returns>BYTE数组</returns>
        public static byte[] ToBytes(this Image sourceImageFile, ImageFormat imageFormat)
        {
            byte[] buffer = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (Bitmap bitmap = new Bitmap(sourceImageFile))
                {
                    bitmap.Save(stream, imageFormat);
                    stream.Position = 0;
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                    stream.Flush();
                }
            }
            return buffer;
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

        private static Size CreateImageSize(int width, int height, Image img)
        {
            double w = 0.0,
                   h = 0.0,
                   sw = Convert.ToDouble(img.Width),
                   sh = Convert.ToDouble(img.Height),
                   mw = Convert.ToDouble(width),
                   mh = Convert.ToDouble(height);

            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = width;
                h = (w * sh) / sw;
            }
            else
            {
                h = height;
                w = (h * sw) / sh;
            }

            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
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
            double hw = 0.0,
                    hh = 0.0;
            double cw = width,
                   ch = height,
                   imgW = img.Width,
                   imgH = img.Height;

            if (cw / ch > imgW / imgH)
            {
                hw = imgW;
                hh = imgW * ch / cw;
            }
            else if (cw / ch < imgW / imgH)
            {
                hw = imgH * cw / ch;
                hh = imgH;
            }
            else
            {
                hw = imgW;
                hh = imgH;
            }

            return new Size(Convert.ToInt32(hw), Convert.ToInt32(hh));
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
            Bitmap sourceImg = new Bitmap(sourceFile);

            try
            {
                EncoderParameter  encoder;
                EncoderParameters encoders;
                encoders = new EncoderParameters(1);
                encoder = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qty);
                encoders.Param[0] = encoder;
                sourceImg.Save(fileName, GetCodecInfo("image/jpeg"), encoders);
                sourceImg.Dispose();
                return true;
            }
            catch
            {
                sourceImg.Dispose();
                return false;
            }
        }

        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);

        [DllImport("msvcrt.dll")]
        private static extern IntPtr memcmp(byte[] b1, byte[] b2, IntPtr count);

        private static void SetWaterMarkImagePosition(Graphics sourceG, Image sourceImage, int sourceImageWidth, int sourceImageHight, ImgWaterPosition position)
        {
            Size sourceSize = CreateImageSize(sourceImageWidth, sourceImageHight, sourceImage);
            int x = 0,
                y = 0,
                padding = 10;

            switch (position)
            {
                case ImgWaterPosition.Center:
                    x = (sourceImageWidth - sourceSize.Width) / 2;
                    y = (sourceImageHight - sourceSize.Height) / 2;
                    break;

                case ImgWaterPosition.TopLeft:
                    x = padding;
                    y = padding;
                    break;

                case ImgWaterPosition.TopRight:
                    x = (sourceImageWidth - sourceSize.Width) - padding;
                    y = padding;
                    break;

                case ImgWaterPosition.BottomLeft:
                    x = padding;
                    y = (sourceImageHight - sourceSize.Height) - padding;
                    break;

                default:
                    x = (sourceImageWidth - sourceSize.Width) - padding;
                    y = (sourceImageHight - sourceSize.Height) - padding;
                    break;
            }

            sourceG.DrawImage(sourceImage, new Rectangle(x, y, sourceSize.Width, sourceSize.Height),
                              0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel);
        }

        private static void SetWaterMarkImageQuality(Bitmap sourceBmp, string sourceImageFile, ImageFormat sourceImageFormat)
        {
            EncoderParameters imgEncoder = new EncoderParameters();
            long[] quality = { 100 };
            imgEncoder.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            ImageCodecInfo[] imgCode = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo imgTmpCode = null;

            for (int x = 0; x < imgCode.Length; x++)
            {
                if (imgCode[x].FormatDescription.Equals("JPEG"))
                {
                    imgTmpCode = imgCode[x];//设置JPEG编码
                    break;
                }
            }

            if (imgTmpCode != null)
            {
                sourceBmp.Save(sourceImageFile, imgTmpCode, imgEncoder);
            }
            else
            {
                sourceBmp.Save(sourceImageFile, sourceImageFormat);
            }
        }

        private static void SetWaterMarkTextPosition(Graphics graphics, string text, Image _sourceImage, int _sourceImageWidth, int _sourceImageHight)
        {
            int[] size = { 16, 14, 12, 10, 8, 6, 4 };
            Font font = null;
            SizeF sizeF = new SizeF();

            for (int i = 0; i < 7; i++)
            {
                font = new Font("arial", size[i], FontStyle.Bold);
                sizeF = graphics.MeasureString(text, font);

                if ((ushort)sizeF.Width < (ushort)_sourceImageWidth)
                {
                    break;
                }
            }

            int yFromBottom = (int)(_sourceImageHight * .08);
            float yPosFromBottom = ((_sourceImageHight - yFromBottom) - (sizeF.Height / 2));
            float centerOfImg = (_sourceImageWidth / 2);
            StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center
            };
            SolidBrush blackBrush = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            graphics.DrawString(text,                 //版权字符串文本
                                font,                                   //字体
                                blackBrush,                           //Brush
                                new PointF(centerOfImg + 1, yPosFromBottom + 1),  //位置
                                format);
            SolidBrush tranBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            graphics.DrawString(text,                 //版权文本
                                font,                                   //字体
                                tranBrush,                           //Brush
                                new PointF(centerOfImg, yPosFromBottom),  //位置
                                format);
        }

        #endregion Methods
    }
}