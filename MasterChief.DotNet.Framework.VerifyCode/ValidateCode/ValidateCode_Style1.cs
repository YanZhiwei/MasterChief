namespace MasterChief.DotNet.Infrastructure.VerifyCode
{
    using MasterChief.DotNet4.Utilities.Common;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;

    /// <summary>
    /// 线条干扰(蓝色)
    /// </summary>
    public class ValidateCode_Style1 : ValidateCodeType
    {
        #region Fields

        /// <summary>
        /// 验证码类型名称
        /// </summary>
        public override string Name => "线条干扰(蓝色)";

        private Color _backgroundColor = Color.White;
        private bool _chaos = true;
        private Color _chaosColor = Color.FromArgb(170, 170, 0x33);
        private Color _drawColor = Color.FromArgb(50, 0x99, 0xcc);
        private bool _fontTextRenderingHint;
        private int _imageHeight = 30;
        private int _padding = 1;
        private int _validataCodeLength = 4;
        private int _validataCodeSize = 0x10;
        private string _validateCodeFont = "Arial";

        #endregion Fields

        #region Properties

        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        /// <summary>
        /// 是否干扰线
        /// </summary>
        public bool Chaos
        {
            get => _chaos;
            set => _chaos = value;
        }

        /// <summary>
        /// 干扰色
        /// </summary>
        public Color ChaosColor
        {
            get => _chaosColor;
            set => _chaosColor = value;
        }

        /// <summary>
        /// 绘画色
        /// </summary>
        public Color DrawColor
        {
            get => _drawColor;
            set => _drawColor = value;
        }

        /// <summary>
        /// 图片高度
        /// </summary>
        public int ImageHeight
        {
            get => _imageHeight;
            set => _imageHeight = value;
        }

        public int Padding
        {
            get => _padding;
            set => _padding = value;
        }

        public int ValidataCodeLength
        {
            get => _validataCodeLength;
            set => _validataCodeLength = value;
        }

        public int ValidataCodeSize
        {
            get => _validataCodeSize;
            set => _validataCodeSize = value;
        }

        public string ValidateCodeFont
        {
            get => _validateCodeFont;
            set => _validateCodeFont = value;
        }

        private bool FontTextRenderingHint
        {
            get => _fontTextRenderingHint;
            set => _fontTextRenderingHint = value;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 创建验证码抽象方法
        /// </summary>
        /// <param name="validataCode">验证码</param>
        /// <returns>数组</returns>
        public override byte[] CreateImage(out string validataCode)
        {
            string formatString = "abcdefghijklmnopqrstuvwxyz";
            validataCode = RandomHelper.NextString(formatString, ValidataCodeLength, false);
            using (MemoryStream stream = new MemoryStream())
            {
                using (Bitmap bitmap = CreateValidataImage(validataCode))
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.GetBuffer();
                }
            }
        }

        private Bitmap CreateValidataImage(string validataCode)
        {
            int width = (int)(((_validataCodeLength * _validataCodeSize) * 1.3) + 4.0);
            Bitmap bitMap = new Bitmap(width, ImageHeight);
            DrawChaosLine(ref bitMap);
            DrawValidataCode(ref bitMap, validataCode);
            return bitMap;
        }

        private void DrawChaosLine(ref Bitmap bitmap)
        {
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                Pen pen = new Pen(DrawColor, 1f);
                Point[] pointArray = new Point[2];
                Random random = new Random();
                if (Chaos)
                {
                    pen = new Pen(ChaosColor, 1f);
                    for (int i = 0; i < (_validataCodeLength * 2); i++)
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
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                if (_fontTextRenderingHint)
                {
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                }
                else
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                Font font = new Font(_validateCodeFont, _validataCodeSize, FontStyle.Regular);
                Brush brush = new SolidBrush(_drawColor);
                int maxValue = Math.Max((ImageHeight - _validataCodeSize) - 5, 0);
                Random random = new Random();
                for (int i = 0; i < _validataCodeLength; i++)
                {
                    int[] numArray = { ((i * _validataCodeSize) + random.Next(1)) + 3, random.Next(maxValue) - 4 };
                    Point point = new Point(numArray[0], numArray[1]);
                    graphics.DrawString(validateCode[i].ToString(), font, brush, point);
                }
            }
        }

        #endregion Methods
    }
}