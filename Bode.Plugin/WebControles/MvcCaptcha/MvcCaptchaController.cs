using System;
using System.Web;
using System.Web.Mvc;
namespace Bode.Plugin.WebControles.MvcCaptcha
{
    public class _MvcCaptchaController : Controller
    {
        public ActionResult MvcCaptchaImage()
        {
            return new MvcCaptchaImageResult();
        }
        public ActionResult MvcCaptchaLoader()
        {
            string text = base.Request.ServerVariables["Query_String"];
            if (!string.IsNullOrEmpty(text))
            {
                base.HttpContext.Session.Remove(text);
            }
            MvcCaptchaOptions mvcCaptchaOptions = new MvcCaptchaOptions();
            MvcCaptchaConfigSection config = MvcCaptchaConfigSection.GetConfig();
            if (config != null)
            {
                mvcCaptchaOptions.TextChars = config.TextChars;
                mvcCaptchaOptions.TextLength = config.TextLength;
                mvcCaptchaOptions.FontWarp = config.FontWarp;
                mvcCaptchaOptions.BackgroundNoise = config.BackgroundNoise;
                mvcCaptchaOptions.LineNoise = config.LineNoise;
            }
            MvcCaptchaImage mvcCaptchaImage = new MvcCaptchaImage(mvcCaptchaOptions);
            base.HttpContext.Session.Add(mvcCaptchaImage.UniqueId, mvcCaptchaImage);
            base.HttpContext.Response.Cache.SetNoStore();
            base.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return base.Content(mvcCaptchaImage.UniqueId);
        }
    }
}
