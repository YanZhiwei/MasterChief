namespace MasterChief.DotNet4.Utilities.Common
{
    using System;

    /// <summary>
    /// 公式计算帮助类
    /// </summary>
    public class FormulaHelper
    {
        #region Methods

        /// <summary>
        /// 计算复杂公式
        /// </summary>
        /// <param name="formulaString">The formula string.</param>
        /// <returns></returns>
        /// 创建时间:2015-05-22 13:49
        /// 备注说明:<c>null</c>
        public static string CalculateComplexExpression(string formulaString)
        {
            string _tempString = "";
            string _exp = "";

            while (formulaString.IndexOf("(") != -1)
            {
                _tempString = formulaString.Substring(formulaString.LastIndexOf("(") + 1, formulaString.Length - formulaString.LastIndexOf("(") - 1);
                _exp = _tempString.Substring(0, _tempString.IndexOf(")"));
                formulaString = formulaString.Replace("(" + _exp + ")", CalculateExpress(_exp).ToString());
            }

            if (formulaString.IndexOf("+") != -1 || formulaString.IndexOf("-") != -1
                    || formulaString.IndexOf("*") != -1 || formulaString.IndexOf("/") != -1)
            {
                formulaString = CalculateExpress(formulaString).ToString();
            }

            return formulaString;
        }

        /// <summary>
        /// 公式计算
        /// </summary>
        /// <param name="formulaString">公式计算表达式</param>
        /// <returns>计算结果</returns>
        public static double CalculateExpress(string formulaString)
        {
            string _tempStringA = string.Empty,
                   _tempStringB = string.Empty,
                   _tempStringC = string.Empty,
                   _tempStringD = string.Empty;
            double _replaceValue = 0;

            while (formulaString.IndexOf("+") != -1 || formulaString.IndexOf("-") != -1
                    || formulaString.IndexOf("*") != -1 || formulaString.IndexOf("/") != -1)
            {
                if (formulaString.IndexOf("*") != -1)
                {
                    _tempStringA = formulaString.Substring(formulaString.IndexOf("*") + 1, formulaString.Length - formulaString.IndexOf("*") - 1);
                    _tempStringB = formulaString.Substring(0, formulaString.IndexOf("*"));
                    _tempStringC = _tempStringB.Substring(GetPrivorPos(_tempStringB) + 1, _tempStringB.Length - GetPrivorPos(_tempStringB) - 1);
                    _tempStringD = _tempStringA.Substring(0, GetNextPos(_tempStringA));
                    _replaceValue = Convert.ToDouble(GetExpType(_tempStringC)) * Convert.ToDouble(GetExpType(_tempStringD));
                    formulaString = formulaString.Replace(_tempStringC + "*" + _tempStringD, _replaceValue.ToString());
                }
                else if (formulaString.IndexOf("/") != -1)
                {
                    _tempStringA = formulaString.Substring(formulaString.IndexOf("/") + 1, formulaString.Length - formulaString.IndexOf("/") - 1);
                    _tempStringB = formulaString.Substring(0, formulaString.IndexOf("/"));
                    _tempStringC = _tempStringB.Substring(GetPrivorPos(_tempStringB) + 1, _tempStringB.Length - GetPrivorPos(_tempStringB) - 1);
                    _tempStringD = _tempStringA.Substring(0, GetNextPos(_tempStringA));
                    _replaceValue = Convert.ToDouble(GetExpType(_tempStringC)) / Convert.ToDouble(GetExpType(_tempStringD));
                    formulaString = formulaString.Replace(_tempStringC + "/" + _tempStringD, _replaceValue.ToString());
                }
                else if (formulaString.IndexOf("+") != -1)
                {
                    _tempStringA = formulaString.Substring(formulaString.IndexOf("+") + 1, formulaString.Length - formulaString.IndexOf("+") - 1);
                    _tempStringB = formulaString.Substring(0, formulaString.IndexOf("+"));
                    _tempStringC = _tempStringB.Substring(GetPrivorPos(_tempStringB) + 1, _tempStringB.Length - GetPrivorPos(_tempStringB) - 1);
                    _tempStringD = _tempStringA.Substring(0, GetNextPos(_tempStringA));
                    _replaceValue = Convert.ToDouble(GetExpType(_tempStringC)) + Convert.ToDouble(GetExpType(_tempStringD));
                    formulaString = formulaString.Replace(_tempStringC + "+" + _tempStringD, _replaceValue.ToString());
                }
                else if (formulaString.IndexOf("-") != -1)
                {
                    _tempStringA = formulaString.Substring(formulaString.IndexOf("-") + 1, formulaString.Length - formulaString.IndexOf("-") - 1);
                    _tempStringB = formulaString.Substring(0, formulaString.IndexOf("-"));
                    _tempStringC = _tempStringB.Substring(GetPrivorPos(_tempStringB) + 1, _tempStringB.Length - GetPrivorPos(_tempStringB) - 1);
                    _tempStringD = _tempStringA.Substring(0, GetNextPos(_tempStringA));
                    _replaceValue = Convert.ToDouble(GetExpType(_tempStringC)) - Convert.ToDouble(GetExpType(_tempStringD));
                    formulaString = formulaString.Replace(_tempStringC + "-" + _tempStringD, _replaceValue.ToString());
                }
            }

            return Convert.ToDouble(formulaString);
        }

        private static double CalculateExExpress(string formulaString, FormulaType ExpressType)
        {
            double _retValue = 0;

            switch (ExpressType)
            {
                case FormulaType.Sin:
                    _retValue = Math.Sin(Convert.ToDouble(formulaString));
                    break;

                case FormulaType.Cos:
                    _retValue = Math.Cos(Convert.ToDouble(formulaString));
                    break;

                case FormulaType.Tan:
                    _retValue = Math.Tan(Convert.ToDouble(formulaString));
                    break;

                case FormulaType.ATan:
                    _retValue = Math.Atan(Convert.ToDouble(formulaString));
                    break;

                case FormulaType.Sqrt:
                    _retValue = Math.Sqrt(Convert.ToDouble(formulaString));
                    break;

                case FormulaType.Pow:
                    _retValue = Math.Pow(Convert.ToDouble(formulaString), 2);
                    break;
            }

            if (_retValue == 0)
            {
                return Convert.ToDouble(formulaString);
            }

            return _retValue;
        }

        private static string GetExpType(string formulaString)
        {
            formulaString = formulaString.ToUpper();

            if (formulaString.IndexOf("SIN") != -1)
            {
                return CalculateExExpress(formulaString.Substring(formulaString.IndexOf("N") + 1, formulaString.Length - 1 - formulaString.IndexOf("N")), FormulaType.Sin).ToString();
            }

            if (formulaString.IndexOf("COS") != -1)
            {
                return CalculateExExpress(formulaString.Substring(formulaString.IndexOf("S") + 1, formulaString.Length - 1 - formulaString.IndexOf("S")), FormulaType.Cos).ToString();
            }

            if (formulaString.IndexOf("TAN") != -1)
            {
                return CalculateExExpress(formulaString.Substring(formulaString.IndexOf("N") + 1, formulaString.Length - 1 - formulaString.IndexOf("N")), FormulaType.Tan).ToString();
            }

            if (formulaString.IndexOf("ATAN") != -1)
            {
                return CalculateExExpress(formulaString.Substring(formulaString.IndexOf("N") + 1, formulaString.Length - 1 - formulaString.IndexOf("N")), FormulaType.ATan).ToString();
            }

            if (formulaString.IndexOf("SQRT") != -1)
            {
                return CalculateExExpress(formulaString.Substring(formulaString.IndexOf("T") + 1, formulaString.Length - 1 - formulaString.IndexOf("T")), FormulaType.Sqrt).ToString();
            }

            if (formulaString.IndexOf("POW") != -1)
            {
                return CalculateExExpress(formulaString.Substring(formulaString.IndexOf("W") + 1, formulaString.Length - 1 - formulaString.IndexOf("W")), FormulaType.Pow).ToString();
            }

            return formulaString;
        }

        private static int GetNextPos(string formulaString)
        {
            int[] _expPos = new int[4];
            _expPos[0] = formulaString.IndexOf("+");
            _expPos[1] = formulaString.IndexOf("-");
            _expPos[2] = formulaString.IndexOf("*");
            _expPos[3] = formulaString.IndexOf("/");
            int tmpMin = formulaString.Length;

            for (int count = 1; count <= _expPos.Length; count++)
            {
                if (tmpMin > _expPos[count - 1] && _expPos[count - 1] != -1)
                {
                    tmpMin = _expPos[count - 1];
                }
            }

            return tmpMin;
        }

        private static int GetPrivorPos(string formulaString)
        {
            int[] _expPos = new int[4];
            _expPos[0] = formulaString.LastIndexOf("+");
            _expPos[1] = formulaString.LastIndexOf("-");
            _expPos[2] = formulaString.LastIndexOf("*");
            _expPos[3] = formulaString.LastIndexOf("/");
            int tmpMax = -1;

            for (int count = 1; count <= _expPos.Length; count++)
            {
                if (tmpMax < _expPos[count - 1] && _expPos[count - 1] != -1)
                {
                    tmpMax = _expPos[count - 1];
                }
            }

            return tmpMax;
        }

        #endregion Methods
    }
}