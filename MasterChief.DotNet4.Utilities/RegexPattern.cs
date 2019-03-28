namespace MasterChief.DotNet4.Utilities
{
    /// <summary>
    ///     检查正则表达式
    /// </summary>
    public static class RegexPattern
    {
        #region Fields

        /// <summary>
        ///     Base64检测正则表达式
        /// </summary>
        public const string Base64Check = "^[A-Z0-9/+=]*$";

        /// <summary>
        ///     验证Bcd码 e.g. "01" or "3456"
        /// </summary>
        public const string BinaryCodedDecimal = @"^([0-9]{2})+$";

        /// <summary>
        ///     车牌格式检测正则表达式
        /// </summary>
        public const string CarLicenseCheck = @"^([\u4e00-\u9fa5]|[A-Z]){1,2}[A-Za-z0-9]{1,2}-[0-9A-Za-z]{5}$";

        /// <summary>
        ///     中文检测正则表达式
        /// </summary>
        public const string ChineseCheck = @"^[\u4e00-\u9fa5]+$";

        /// <summary>
        ///     日期格式检测正则表达式
        /// </summary>
        public const string DateCheck = @"\d{4}([/-年])\d{1,2}([/-月])\d{1,2}([日])\s?\d{0,2}:?\d{0,2}:?\d{0,2}";

        /// <summary>
        ///     验证两位小数
        /// </summary>
        public const string DecimalCheck = @"^[0-9]+(.[0-9]{2})?$";

        /// <summary>
        ///     电子邮箱检测正则表达式
        /// </summary>
        public const string EmailCheck =
            @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$";

        /// <summary>
        ///     文件路径检测正则表达式
        /// </summary>
        public const string FileCheck =
            @"^(?<fpath>([a-zA-Z]:\\)([\s\.\-\w]+\\)*)(?<fname>[\w]+)(?<namext>(\.[\w]+)*)(?<suffix>\.[\w]+)";

        /// <summary>
        ///     是否十六进制字符串检测正则表达式
        /// </summary>
        public const string HexStringCheck = @"\A\b[0-9a-fA-F]+\b\Z";

        /// <summary>
        ///     身份证检测正则表达式
        /// </summary>
        public const string IdCardCheck =
            @"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|50|51|52|53|54|61|62|63|64|65|71|81|82|91)(\d{13}|\d{15}[\dx])$";

        /// <summary>
        ///     整数检测正则表达式
        /// </summary>
        public const string IntCheck = @"^[0-9]+[0-9]*$";

        /// <summary>
        ///     检查只能中文或字母
        /// </summary>
        public const string ChineseOrCharacterCheck = "^[\u4e00-\u9fa5a-zA-Z]+$";

        /// <summary>
        ///     Ip检测正则表达式
        /// </summary>
        public const string
            IpCheck =
                @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"; // @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

        /// <summary>
        ///     A~Z,a~z字幕检测正则表达式
        /// </summary>
        public const string LetterCheck = @"^[A-Za-z]+$";

        /// <summary>
        ///     数字检测正则表达式
        /// </summary>
        public const string NumberCheck = @"^[0-9]+[0-9]*[.]?[0-9]*$";

        /// <summary>
        ///     验证非零的正整数
        /// </summary>
        public const string NumberWithoutZeroCheck = @"^[A-Za-z]+$";

        /// <summary>
        ///     验证密码长度(要求长度为6-18位)
        /// </summary>
        public const string PassWordLengthCheck = @"^\d{6,18}$";

        /// <summary>
        ///     邮政编码检测正则表达式
        /// </summary>
        public const string PostCodeCheck = @"^\d{6}$";

        /// <summary>
        ///     URL检测正则表达式
        /// </summary>
        public const string UrlCheck =
            @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?$";

        /// <summary>
        ///     Mac地址(7个长度)正则表达式
        /// </summary>
        public const string MacAddr7Check =
            @"^([0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F])$";

        /// <summary>
        ///     Mac地址（6个长度）正则表达式
        /// </summary>
        public const string MacAddr6Check =
            @"^([0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F]-[0-9A-F][0-9A-F])$";

        /// <summary>
        ///     手机号码
        /// </summary>
        public const string MobilePhone = @"^1[0-9]{10}$";

        #endregion Fields
    }
}