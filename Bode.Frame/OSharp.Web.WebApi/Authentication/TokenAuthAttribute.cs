using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using OSharp.Core;
using OSharp.Core.Context;
using OSharp.Utility.Data;
using OSharp.Utility.Extensions;
using OSharp.Utility.Secutiry;
using OSharp.Web.Http.Internal;
using OSharp.Web.Http.Messages;
using System.Web;

namespace OSharp.Web.Http.Authentication
{
    public class TokenAuthAttribute : AuthorizeAttribute
    {
        public bool AllowAnonymous = false;

        /// <summary>
        /// 重写身份验证方法
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            try
            {
                string token = string.Empty;
                if (httpContext.Request.Headers.Contains(HttpHeaderNames.OSharpAuthenticationToken))
                {
                    token = httpContext.Request.Headers.GetValues(HttpHeaderNames.OSharpAuthenticationToken).FirstOrDefault();// 从客户端请求中获取 token
                }
                else
                {
                    HttpContextBase context = (HttpContextBase)httpContext.Request.Properties["MS_HttpContext"];//获取传统context     
                    HttpRequestBase request = context.Request;//定义传统request对象
                    token = request.Form[HttpHeaderNames.OSharpAuthenticationToken] ?? "";
                }

                var strAuth = DesHelper.Decrypt(token, Constants.BodeAuthDesKey);
                Operator user = strAuth.FromJsonString<Operator>() ?? new Operator();
                OSharpContext.Current.SetOperator(user);

                if (AllowAnonymous) return true;
                return int.Parse(user.UserId) > 0 && user.ValidatePeriod > DateTime.Now;
            }
            catch
            {
                OSharpContext.Current.SetOperator(new Operator());
                return AllowAnonymous;
            }
        }

        /// <summary>
        /// 重写身份验证失败执行方法
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            actionContext.Response = request.CreateResponse(HttpStatusCode.Forbidden);

            ApiResult result = new ApiResult(OperationResultType.ValidError, "用户登录已过期");
            actionContext.Response.Content = new StringContent(result.ToJsonString(), Encoding.UTF8, "application/json");
        }
    }
}