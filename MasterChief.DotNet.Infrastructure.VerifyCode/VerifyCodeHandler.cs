using System.Web;

namespace MasterChief.DotNet.Infrastructure.VerifyCode
{
    /// <summary>
    ///     验证码 HttpHandler
    /// </summary>
    public abstract class VerifyCodeHandler
    {
        #region Methods

        /// <summary>
        ///     生成验证码完成方法
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="validateCode">生成的验证码</param>
        /// <returns>验证码字符</returns>
        public abstract void OnValidateCodeCreated(HttpContext context, string validateCode);

        /// <summary>
        ///     创建验证码
        /// </summary>
        /// <param name="style">验证码样式</param>
        /// <returns>byte[]</returns>
        public abstract byte[] CreateValidateCode(string style);
        ///// <summary>
        /////     处理请求
        ///// </summary>
        ///// <param name="context">HttpContext</param>
        //public void ProcessRequest(HttpContext context)
        //{
        //    var validateType = context.Request.Params["style"];
        //    if (!string.IsNullOrEmpty(validateType))
        //    {
        //        validateType = validateType.Trim();
        //        string validateCode;
        //        ValidateCodeType createCode;
        //        switch (validateType)
        //        {
        //            case "type1":
        //                createCode = new ValidateCode_Style1();
        //                break;

        //            default:
        //                createCode = new ValidateCode_Style1();
        //                break;
        //        }

        //        var buffer = createCode.CreateImage(out validateCode);
        //        OnValidateCodeCreated(context, validateCode);
        //        //  context.Session["validateCode"] = _validateCode;
        //        context.Response.ClearContent();
        //        context.Response.ContentType = MimeTypes.ImageGif;
        //        context.Response.BinaryWrite(buffer);
        //    }
        //}

        #endregion Methods
    }
}