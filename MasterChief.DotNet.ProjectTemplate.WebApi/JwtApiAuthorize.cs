using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;
using MasterChief.DotNet4.Utilities.Common;
using MasterChief.DotNet4.Utilities.Encryptor;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    ///     基于Jwt 授权实现
    /// </summary>
    public sealed class JwtApiAuthorize : IApiAuthorize
    {
        /// <summary>
        ///     检查请求签名合法性
        /// </summary>
        /// <param name="signature">加密签名字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="appConfig">应用接入配置信息</param>
        /// <returns>CheckResult</returns>
        public CheckResult CheckRequestSignature(string signature, string timestamp, string nonce, Model.AppConfig appConfig)
        {
            var appSecret = appConfig.AppSecret;
            var signatureExpired = appConfig.SignatureExpiredMinutes;
            string[] data = {appSecret, timestamp, nonce};
            Array.Sort(data);
            var signatureText = string.Join("", data);
            signatureText = Md5Encryptor.Encrypt(signatureText);

            if (!signature.CompareIgnoreCase(signatureText) && CheckHelper.IsNumber(timestamp))
                return CheckResult.Success();
            var timestampMillis =
                UnixEpochHelper.DateTimeFromUnixTimestampMillis(timestamp.ToDoubleOrDefault());
            var minutes = DateTime.UtcNow.Subtract(timestampMillis).TotalMinutes;

            return minutes > signatureExpired ? CheckResult.Fail("签名时间戳失效") : CheckResult.Success();
        }

        /// <summary>
        ///     创建合法用户获取访问令牌接口数据
        /// </summary>
        /// <param name="identityUser">IdentityUser</param>
        /// <param name="appConfig">AppConfig</param>
        /// <returns>IdentityToken</returns>
        public OperatedResult<IdentityToken> CreateIdentityToken(IdentityUser identityUser, Model.AppConfig appConfig)
        {
            var payload = new Dictionary<string, object>
            {
                {"iss", identityUser.UserId},
                {"iat", UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds}
            };
            var identityToken = new IdentityToken
            {
                AccessToken = CreateIdentityToken(appConfig.SharedKey, payload),
                ExpiresIn = appConfig.TokenExpiredDay * 24 * 3600
            };
            return OperatedResult<IdentityToken>.Success(identityToken);
        }

        /// <summary>
        ///     创建Token
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="payload">负载数据</param>
        /// <returns>Token令牌</returns>
        public static string CreateIdentityToken(string secret, Dictionary<string, object> payload)
        {
            ValidateOperator.Begin().NotNull(payload, "负载数据").NotNullOrEmpty(secret, "密钥");
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, secret);
        }
    }
}