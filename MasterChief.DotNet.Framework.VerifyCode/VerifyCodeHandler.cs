using MasterChief.DotNet4.Utilities;
using System.Web;
using System.Web.SessionState;

namespace MasterChief.DotNet.Framework.VerifyCode
{
    public abstract class VerifyCodeHandler : IHttpHandler, IRequiresSessionState
    {
        #region Properties

        /// <summary>
        /// IsReusable
        /// </summary>
        public bool IsReusable => false;

        #endregion Properties

        #region Methods

        /// <summary>
        /// 生成验证码完成方法
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="validateCode">生成的验证码</param>
        public abstract void OnValidateCodeCreated(HttpContext context, string validateCode);

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void ProcessRequest(HttpContext context)
        {
            string validateCode = string.Empty;
            string validateType = context.Request.Params["style"];
            ValidateCodeType createCode = null;

            //if (string.IsNullOrEmpty(validateType))
            //{
            createCode = new ValidateCode_Style1();
            //}

            byte[] buffer = createCode.CreateImage(out validateCode);
            OnValidateCodeCreated(context, validateCode);
            //  context.Session["validateCode"] = _validateCode;
            context.Response.ClearContent();
            context.Response.ContentType = MimeTypes.ImageGif;
            context.Response.BinaryWrite(buffer);
        }

        #endregion Methods
    }
}