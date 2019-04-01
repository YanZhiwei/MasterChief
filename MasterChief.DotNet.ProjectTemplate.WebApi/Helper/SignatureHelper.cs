using System;
using System.Globalization;
using MasterChief.DotNet4.Utilities.Common;
using MasterChief.DotNet4.Utilities.Encryptor;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Helper
{
    /// <summary>
    ///     WebApi 签名处理辅助方法
    /// </summary>
    public sealed class SignatureHelper
    {

        /// <summary>
        ///     生成签名字符串
        /// </summary>
        /// <param name="appSecret">签名密钥</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns>WebApi签名</returns>
        public static string Create(Guid appSecret, string timestamp, string nonce)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(appSecret.ToString(), "签名密钥")
                .NotNullOrEmpty(timestamp, "时间戳")
                .NotNullOrEmpty(nonce, "随机数");
            string[] array = { appSecret.ToString(), timestamp, nonce };
            Array.Sort(array);
            var signatureText = string.Join("", array);
            signatureText = Md5Encryptor.Encrypt(signatureText);
            return signatureText;
        }

        /// <summary>
        /// 生成签名字符串
        /// </summary>
        /// <param name="appSecret">签名密钥</param>
        /// <returns>WebApi签名</returns>
        public static string Create(Guid appSecret)
        {
            string timestamp = UnixEpochHelper.GetCurrentUnixTimestamp().TotalMilliseconds.ToString(CultureInfo.InvariantCulture);
            string nonce = new Random().NextDouble().ToString(CultureInfo.InvariantCulture);
            return Create(appSecret, timestamp, nonce);
        }
    }
}