using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using MasterChief.DotNet4.Utilities.Common;

namespace MasterChief.DotNet.Infrastructure.VerifyCode.ValidateCode
{
    /// <summary>
    ///     线条干扰(蓝色)
    /// </summary>
    public class ValidateCode_Style1 : ValidateCodeType
    {
        #region Fields

        /// <summary>
        ///     验证码类型名称
        /// </summary>
        public override string Name => "线条干扰(蓝色)";

        #endregion Fields

        #region Properties

        /// <summary>
        ///     背景色
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.White;

        /// <summary>
        ///     是否干扰线
        /// </summary>
        public bool Chaos { get; set; } = true;

        /// <summary>
        ///     干扰色
        /// </summary>
        public Color ChaosColor { get; set; } = Color.FromArgb(170, 170, 0x33);

        /// <summary>
        ///     绘画色
        /// </summary>
        public Color DrawColor { get; set; } = Color.FromArgb(50, 0x99, 0xcc);

        /// <summary>
        ///     图片高度
        /// </summary>
        public int ImageHeight { get; set; } = 30;

        /// <summary>
        ///     间距
        /// </summary>
        public int Padding { get; set; } = 1;

        /// <summary>
        ///     长度
        /// </summary>
        public int ValidataCodeLength { get; set; } = 4;

        /// <summary>
        ///     尺寸
        /// </summary>
        public int ValidataCodeSize { get; set; } = 0x10;

        /// <summary>
        ///     验证码字体
        /// </summary>
        public string ValidateCodeFont { get; set; } = "Arial";


        /// <summary>
        ///     文本的呈现模式
        /// </summary>
        public bool FontTextRenderingHint { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     创建验证码抽象方法
        /// </summary>
        /// <param name="validataCode">验证码</param>
        /// <returns>数组</returns>
        public override byte[] CreateImage(out string validataCode)
        {
            var formatString = "abcdefghijklmnopqrstuvwxyz";
            validataCode = RandomHelper.NextString(formatString, ValidataCodeLength, false);
            using (var stream = new MemoryStream())
            {
                using (var bitmap = CreateValidataImage(validataCode))
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.GetBuffer();
                }
            }
        }

        private Bitmap CreateValidataImage(string validataCode)
        {
            var width = (int) (ValidataCodeLength * ValidataCodeSize * 1.3 + 4.0);
            var bitMap = new Bitmap(width, ImageHeight);
            DrawChaosLine(ref bitMap);
            DrawValidataCode(ref bitMap, validataCode);
            return bitMap;
        }

        private void DrawChaosLine(ref Bitmap bitmap)
        {
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                var pen = new Pen(DrawColor, 1f);
                var pointArray = new Point[2];
                var random = new Random();
                if (Chaos)
                {
                    pen = new Pen(ChaosColor, 1f);
                    for (var i = 0; i < ValidataCodeLength * 2; i++)
                    {
                        pointArray[0] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                        pointArray[1] = new Point(random.Next(bitmap.Width), random.Next(bitmap.Height));
                        graphics.DrawLine(pen, pointArray[0], pointArray[1]);
                    }
                }
            }
        }

        private void DrawValidataCode(ref Bitmap bitMap, string validateCode)
        {
            using (var graphics = Graphics.FromImage(bitMap))
            {
                if (FontTextRenderingHint)
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                else
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                var font = new Font(ValidateCodeFont, ValidataCodeSize, FontStyle.Regular);
                Brush brush = new SolidBrush(DrawColor);
                var maxValue = Math.Max(ImageHeight - ValidataCodeSize - 5, 0);
                var random = new Random();
                for (var i = 0; i < ValidataCodeLength; i++)
                {
                    int[] numArray = {i * ValidataCodeSize + random.Next(1) + 3, random.Next(maxValue) - 4};
                    var point = new Point(numArray[0], numArray[1]);
                    graphics.DrawString(validateCode[i].ToString(), font, brush, point);
                }
            }
        }

        #endregion Methods
    }
}