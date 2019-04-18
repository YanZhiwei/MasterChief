using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MasterChief.DotNet4.Utilities.WinForm
{
    /// <summary>
    ///     TextBox 帮助类
    /// </summary>
    public static class TextBoxHelper
    {
        #region Methods

        /// <summary>
        ///     清除水印文字
        /// </summary>
        /// <param name="textBox">TextBox</param>
        public static void ClearWatermark(this TextBox textBox)
        {
            SendMessage(textBox.Handle, 0x1501, 0, string.Empty);
        }

        /// <summary>
        ///     只能输入正浮点型数字
        /// </summary>
        /// <param name="textBox">TextBox</param>
        public static void OnlyInputFloat(this TextBox textBox)
        {
            textBox.KeyPress += (sender, e) =>
            {
                var curTextBox = sender as TextBox;
                if (e.KeyChar == 46)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    if (curTextBox.Text.Length <= 0)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        var txtValue = curTextBox.Text.Trim();
                        bool oldResult, newResult;
                        oldResult = float.TryParse(txtValue, out _);
                        newResult = float.TryParse(txtValue + e.KeyChar.ToString(), out _);
                        if (!newResult)
                        {
                            if (oldResult)
                                e.Handled = true;
                            else
                                e.Handled = false;
                        }
                    }
                }
            };
        }

        /// <summary>
        ///     只能输入数字
        /// </summary>
        /// <param name="textBox">TextBox</param>
        public static void OnlyInputNumber(this TextBox textBox)
        {
            textBox.KeyPress += (sender, e) =>
            {
                if (e.KeyChar != 8 && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
        }

        /// <summary>
        ///     为TextBox设置水印文字
        /// </summary>
        /// <param name="textBox">TextBox</param>
        /// <param name="watermark">水印文字</param>
        public static void SetWatermark(this TextBox textBox, string watermark)
        {
            SendMessage(textBox.Handle, 0x1501, 0, watermark);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam,
            [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        #endregion Methods
    }
}