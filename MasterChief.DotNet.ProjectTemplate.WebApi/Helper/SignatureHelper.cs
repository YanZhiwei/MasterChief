using MasterChief.DotNet4.Utilities.Common;
using MasterChief.DotNet4.Utilities.Encryptor;
using MasterChief.DotNet4.Utilities.Result;
using System;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Helper
{
    /// <summary>
    /// WebApi 签名辅助类
    /// </summary>
    public sealed class SignatureHelper
    {
        #region Methods

        /// <summary>
        /// 生成签名字符串
        /// </summary>
        /// <param name="appSecret">签名加密键</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        public static string Create(string appSecret, string timestamp, string nonce)
        {
            string[] data = { appSecret, timestamp, nonce };
            Array.Sort(data);
            string signatureString = string.Join("", data);
            signatureString = Md5Encryptor.Encrypt(signatureString);
            return signatureString;
        }

        /// <summary>
        /// 验证WebApi签名
        /// </summary>
        /// <param name="signature">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appSecret">签名加密键</param>
        /// <param name="signatureExpiredMinutes">签名过期分钟</param>
        /// <returns>CheckResult</returns>
        internal static CheckResult Validate(string signature, string timestamp, string nonce, string appSecret, int signatureExpiredMinutes)
        {
            string[] data = { appSecret, timestamp, nonce };
            Array.Sort(data);
            string signatureString = string.Join("", data);
            signatureString = Md5Encryptor.Encrypt(signatureString);

            if (signature.CompareIgnoreCase(signatureString) && CheckHelper.IsNumber(timestamp))
            {
                DateTime timestampMillis =
                    UnixEpochHelper.DateTimeFromUnixTimestampMillis(timestamp.ToDoubleOrDefault(0f));
                double minutes = DateTime.UtcNow.Subtract(timestampMillis).TotalMinutes;

                if (minutes > signatureExpiredMinutes)
                {
                    return CheckResult.Fail("签名时间戳失效");
                }
            }

            return CheckResult.Success();
        }

        #endregion Methods
    }
}