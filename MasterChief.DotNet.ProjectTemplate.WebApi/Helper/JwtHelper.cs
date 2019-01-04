using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using MasterChief.DotNet4.Utilities.Operator;
using System;
using System.Collections.Generic;

namespace MasterChief.DotNet.ProjectTemplate.WebApi.Helper
{
    /// <summary>
    /// JSON Web Token辅助类
    /// </summary>
    public class JwtHelper
    {
        #region Methods

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="payload">负载数据</param>
        /// <returns>Token令牌</returns>
        public static string CreateTokens(string secret, Dictionary<string, object> payload)
        {
            ValidateOperator.Begin().NotNull(payload, "负载数据").NotNullOrEmpty(secret, "密钥");
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            return encoder.Encode(payload, secret);
        }

        /// <summary>
        /// 转换Token
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="secret">密钥</param>
        /// <returns>Token以及负载数据</returns>
        public static string ParseTokens(string token, string secret)
        {
            ValidateOperator.Begin().NotNullOrEmpty(token, "令牌").NotNullOrEmpty(secret, "密钥");
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                return decoder.Decode(token, secret, true);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Methods
    }
}