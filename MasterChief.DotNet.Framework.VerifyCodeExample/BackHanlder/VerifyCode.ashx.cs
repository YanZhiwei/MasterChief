using MasterChief.DotNet.Infrastructure.VerifyCode;
using System.Web;

namespace MasterChief.DotNet.Infrastructure.VerifyCodeExample.BackHanlder
{
    /// <summary>
    /// VerifyCode 的摘要说明
    /// </summary>
    public class VerifyCode : VerifyCodeHandler
    {
        public override void OnValidateCodeCreated(HttpContext context, string validateCode)
        {
            context.Session["validateCode"] = validateCode;
        }
    }
}