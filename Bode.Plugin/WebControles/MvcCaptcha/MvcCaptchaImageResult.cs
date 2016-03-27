using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Mvc;
namespace Bode.Plugin.WebControles.MvcCaptcha
{
    internal class MvcCaptchaImageResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            string text = context.HttpContext.Request.ServerVariables["Query_String"];
            if (text.Contains("&"))
            {
                text = text.Split(new char[]
                {
                    '&'
                })[0];
            }
            MvcCaptchaImage cachedCaptcha = MvcCaptchaImage.GetCachedCaptcha(text);
            if (string.IsNullOrEmpty(text) || cachedCaptcha == null)
            {
                context.HttpContext.Response.StatusCode = 404;
                context.HttpContext.Response.StatusDescription = "Not Found";
                context.HttpContext.Response.End();
                return;
            }
            cachedCaptcha.ResetText();
            using (Bitmap bitmap = cachedCaptcha.RenderImage())
            {
                bitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Gif);
            }
            context.HttpContext.Response.Cache.SetNoStore();
            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.HttpContext.Response.ContentType = "image/gif";
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.StatusDescription = "OK";
            context.HttpContext.ApplicationInstance.CompleteRequest();
        }
    }
}
