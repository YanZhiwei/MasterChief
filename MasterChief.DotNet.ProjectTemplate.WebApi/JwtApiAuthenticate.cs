using System;
using JWT;
using JWT.Serializers;
using MasterChief.DotNet.ProjectTemplate.WebApi.Model;
using MasterChief.DotNet4.Utilities.Common;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.Utilities.Result;
using Newtonsoft.Json.Linq;

namespace MasterChief.DotNet.ProjectTemplate.WebApi
{
    public sealed class JwtApiAuthenticate : IApiAuthenticate
    {
        public OperatedResult<string> CheckIdentityToken(string token, AppConfig appConfig)
        {
            ValidateOperator.Begin()
                .NotNullOrEmpty(token, "Token")
                .NotNull(appConfig, "AppConfig");
            try
            {
                var tokenText = ParseTokens(token, appConfig.SharedKey);
                if (string.IsNullOrEmpty(tokenText))
                    return OperatedResult<string>.Fail("用户令牌Token为空");

                dynamic root = JObject.Parse(tokenText);
                string userid = root.iss;
                var iat = root.iat;
                var validTokenExpired =
                    new TimeSpan((int) (UnixEpochHelper.GetCurrentUnixTimestamp().TotalSeconds - iat))
                        .TotalDays > appConfig.TokenExpiredDay;
                return validTokenExpired
                    ? OperatedResult<string>.Fail($"用户ID{userid}令牌失效")
                    : OperatedResult<string>.Success(userid);
            }
            catch (FormatException)
            {
                return OperatedResult<string>.Fail("用户令牌非法");
            }
            catch (SignatureVerificationException)
            {
                return OperatedResult<string>.Fail("用户令牌非法");
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
    }
}