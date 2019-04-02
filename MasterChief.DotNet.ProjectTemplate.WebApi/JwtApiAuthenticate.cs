using System;
using JWT;
using JWT.Serializers;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;
using MasterChief.DotNet.ProjectTemplate.WebApi.Result;
using MasterChief.DotNet4.Utilities.Common;
using MasterChief.DotNet4.Utilities.Operator;
using Newtonsoft.Json.Linq;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    /// <summary>
    ///     基于Jwt 授权验证实现
    /// </summary>
    public sealed class JwtApiAuthenticate : IApiAuthenticate
    {
        /// <summary>
        ///     检查Token是否合法
        /// </summary>
        /// <param name="token">用户令牌</param>
        /// <param name="appConfig">AppConfig</param>
        /// <returns></returns>
        public ApiResult<string> CheckIdentityToken(string token, AppConfig appConfig)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(token, "Token")
                .NotNull(appConfig, "AppConfig");
            try
            {
                var tokenText = ParseTokens(token, appConfig.SharedKey);
                if (string.IsNullOrEmpty(tokenText))
                    return ApiResult<string>.Fail("用户令牌Token为空");

                dynamic root = JObject.Parse(tokenText);
                string userid = root.iss;
                double iat = root.iat;
                var validTokenExpired =
                    new TimeSpan((int) (UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds - iat))
                        .TotalDays > appConfig.TokenExpiredDay;
                return validTokenExpired
                    ? ApiResult<string>.Fail($"用户ID{userid}令牌失效")
                    : ApiResult<string>.Success(userid);
            }
            catch (FormatException)
            {
                return ApiResult<string>.Fail("用户令牌非法");
            }
            catch (SignatureVerificationException)
            {
                return ApiResult<string>.Fail("用户令牌非法");
            }
        }

        /// <summary>
        ///     转换Token
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="secret">密钥</param>
        /// <returns>Token以及负载数据</returns>
        private string ParseTokens(string token, string secret)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(token, "令牌")
                .NotNullOrEmpty(secret, "密钥");

            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            return decoder.Decode(token, secret, true);
        }
    }
}