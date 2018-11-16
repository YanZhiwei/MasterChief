namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// RMB 帮助类
    /// </summary>
    public class RMBHelper
    {
        #region Methods

        /// <summary>
        /// 将数字转换成人民币大小金额
        /// </summary>
        /// <param name="rmbValue">数字</param>
        /// <returns>人民币大小金额</returns>
        public static string ToRMBDescription(decimal rmbValue)
        {
            string _pattern1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字
            string _pattern2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string _tmp3 = "";    //从原num值中取出的值
            string _tmp4 = "";    //数字的字符串形式
            string _tmp5 = "";  //人民币大写金额形式
            int i;    //循环变量
            int j;    //num的值乘以100的字符串长度
            string ch1 = "";    //数字的汉语读法
            string ch2 = "";    //数字位的汉字读法
            int nzero = 0;  //用来计算连续的零值是几个
            int temp;            //从原num值中取出的值
            rmbValue = Math.Round(Math.Abs(rmbValue), 2);    //将num取绝对值并四舍五入取2位小数
            _tmp4 = ((long)(rmbValue * 100)).ToString();        //将num乘100并转换成字符串形式
            j = _tmp4.Length;      //找出最高位

            if (j > 15)
            {
                return "溢出";
            }

            _pattern2 = _pattern2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分

            //循环取出每一位需要转换的值
            for (i = 0; i < j; i++)
            {
                _tmp3 = _tmp4.Substring(i, 1);          //取出需转换的某一位的值
                temp = Convert.ToInt32(_tmp3);      //转换为数字

                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (_tmp3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (_tmp3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + _pattern1.Substring(temp * 1, 1);
                            ch2 = _pattern2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = _pattern1.Substring(temp * 1, 1);
                            ch2 = _pattern2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (_tmp3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + _pattern1.Substring(temp * 1, 1);
                        ch2 = _pattern2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (_tmp3 != "0" && nzero == 0)
                        {
                            ch1 = _pattern1.Substring(temp * 1, 1);
                            ch2 = _pattern2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (_tmp3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = _pattern2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }

                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    ch2 = _pattern2.Substring(i, 1);
                }

                _tmp5 = _tmp5 + ch1 + ch2;

                if (i == j - 1 && _tmp3 == "0")
                {
                    //最后一位（分）为0时，加上“整”
                    _tmp5 = _tmp5 + '整';
                }
            }

            if (rmbValue == 0)
            {
                _tmp5 = "零元整";
            }

            return _tmp5;
        }

        #endregion Methods
    }
}