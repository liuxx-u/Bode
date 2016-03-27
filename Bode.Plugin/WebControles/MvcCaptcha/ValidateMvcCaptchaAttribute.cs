using System;
using System.Web.Mvc;
namespace Bode.Plugin.WebControles.MvcCaptcha
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class ValidateMvcCaptchaAttribute : ActionFilterAttribute
    {
        public string Field
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否保存Session
        /// </summary>
        public bool IsRetainSession { get; set; }

        public ValidateMvcCaptchaAttribute() : this("_mvcCaptchaText")
        {
        }
        public ValidateMvcCaptchaAttribute(string field)
        {
            this.Field = field;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string text = filterContext.HttpContext.Request.Form["_MvcCaptchaGuid"];
            MvcCaptchaImage cachedCaptcha = MvcCaptchaImage.GetCachedCaptcha(text);
            string text2 = filterContext.HttpContext.Request.Form[this.Field];
            string text3 = (cachedCaptcha == null) ? string.Empty : cachedCaptcha.Text;
            if (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3) || !string.Equals(text2, text3, StringComparison.OrdinalIgnoreCase))
            {
                ((Controller)filterContext.Controller).ModelState.AddModelError(this.Field, (string)filterContext.HttpContext.GetGlobalResourceObject("LangPack", "ValidationCode_Not_Match"));
            }
            if (!IsRetainSession)
            {
                filterContext.HttpContext.Session.Remove(text);
            }
        }
    }
}
