using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Bode.Plugin.WebControles.MvcCaptcha
{
    public static class MvcCaptchaHelper
    {
        private static MvcHtmlString MvcCaptcha(this HtmlHelper helper, string actionName, string controllerName, MvcCaptchaOptions options)
        {
            if (options == null)
            {
                options = new MvcCaptchaOptions();
            }
            MvcCaptchaImage mvcCaptchaImage = new MvcCaptchaImage(options);
            HttpContext.Current.Session.Add(mvcCaptchaImage.UniqueId, mvcCaptchaImage);
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            StringBuilder stringBuilder = new StringBuilder(1500);
            stringBuilder.Append("\r\n<!--MvcCaptcha 1.1 for ASP.NET MVC 2 RC Copyright:2009-2010 Webdiyer (http://www.webdiyer.com)-->\r\n");
            stringBuilder.Append("<input type=\"hidden\" name=\"_mvcCaptchaGuid\" id=\"_mvcCaptchaGuid\"");
            if (options.DelayLoad)
            {
                stringBuilder.Append("/><script language=\"javascript\" type=\"text/javascript\">if (typeof (jQuery) == \"undefined\") { alert(\"jQuery脚本库未加载，请检查！\"); }");
                stringBuilder.Append("var _mvcCaptchaPrevGuid = null,_mvcCaptchaImgLoaded = false;function _loadMvcCaptchaImage(){");
                stringBuilder.Append("if(!_mvcCaptchaImgLoaded){$.ajax({type:'GET',url:'");
                stringBuilder.Append(urlHelper.Action("MvcCaptchaLoader", "_MvcCaptcha", new RouteValueDictionary
                {

                    {
                        "area",
                        null
                    }
                }));
                stringBuilder.Append("?'+_mvcCaptchaPrevGuid,global:false,success:function(data){_mvcCaptchaImgLoaded=true;");
                stringBuilder.Append("$(\"#_mvcCaptchaGuid\").val(data);_mvcCaptchaPrevGuid=data;$(\"#");
                stringBuilder.Append(options.CaptchaImageContainerId).Append("\").html('");
                stringBuilder.Append(MvcCaptchaHelper.CreateImgTag(urlHelper.Action(actionName, controllerName, new RouteValueDictionary
                {

                    {
                        "area",
                        null
                    }
                }) + "?'+data+'", options, null));
                stringBuilder.Append("');}});} };function _reloadMvcCaptchaImage(){_mvcCaptchaImgLoaded=false;_loadMvcCaptchaImage();};$(function(){");
                stringBuilder.Append("if($(\"#").Append(options.ValidationInputBoxId).Append("\").length==0){alert(\"未能找到验证码输入文本框，请检查ValidationInputBoxId属性是否设置正确！\");}");
                stringBuilder.Append("if($(\"#").Append(options.CaptchaImageContainerId).Append("\").length==0){alert(\"未能找到验证码图片父容器，请检查CaptchaImageContainerId属性是否设置正确！\");}");
                stringBuilder.Append("$(\"#").Append(options.ValidationInputBoxId);
                stringBuilder.Append("\").bind(\"focus\",_loadMvcCaptchaImage)});</script>");
            }
            else
            {
                stringBuilder.AppendFormat(" value=\"{0}\" />", mvcCaptchaImage.UniqueId);
                stringBuilder.Append(MvcCaptchaHelper.CreateImgTag(urlHelper.Action(actionName, controllerName, new RouteValueDictionary
                {

                    {
                        "area",
                        null
                    }
                }) + "?" + mvcCaptchaImage.UniqueId, options, mvcCaptchaImage.UniqueId));
                stringBuilder.Append("<script language=\"javascript\" type=\"text/javascript\">function _reloadMvcCaptchaImage(){var ci=document.getElementById(\"");
                stringBuilder.Append(mvcCaptchaImage.UniqueId);
                stringBuilder.Append("\");var sl=ci.src.length;if(ci.src.indexOf(\"&\")>-1)sl=ci.src.indexOf(\"&\");ci.src=ci.src.substr(0,sl)+\"&\"+(new Date().valueOf());}</script>");
            }
            stringBuilder.Append("\r\n<!--MvcCaptcha 1.1 for ASP.NET MVC 2 RC Copyright:2009-2010 Webdiyer (http://www.webdiyer.com)-->\r\n");
            return MvcHtmlString.Create(stringBuilder.ToString());
        }
        private static string CreateImgTag(string url, MvcCaptchaOptions options, string id)
        {
            StringBuilder stringBuilder = new StringBuilder("<a href=\"javascript:_reloadMvcCaptchaImage()\"><img src=\"");
            stringBuilder.Append(url);
            stringBuilder.Append("\" alt=\"MvcCaptcha\" title=\"刷新图片\" width=\"");
            stringBuilder.Append(options.Width);
            stringBuilder.Append("\" height=\"");
            stringBuilder.Append(options.Height);
            if (!string.IsNullOrEmpty(id))
            {
                stringBuilder.Append("\" id=\"").Append(id);
            }
            stringBuilder.Append("\" border=\"0\"/></a><a href=\"javascript:_reloadMvcCaptchaImage()\">").Append(options.ReloadLinkText).Append("</a>");
            return stringBuilder.ToString();
        }
        public static MvcHtmlString MvcCaptcha(this HtmlHelper helper)
        {
            return helper.MvcCaptcha(new MvcCaptchaOptions());
        }
        public static MvcHtmlString MvcCaptcha(this HtmlHelper helper, MvcCaptchaOptions options)
        {
            return helper.MvcCaptcha("MvcCaptchaImage", "_MvcCaptcha", options);
        }
    }
}
