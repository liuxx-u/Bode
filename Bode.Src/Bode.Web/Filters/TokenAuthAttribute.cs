using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using OSharp.Core.Context;
using OSharp.Utility.Extensions;
using OSharp.Utility.Secutiry;
using OSharp.Web.Http.Internal;

namespace Bode.Web.Filters
{
    public class TokenAuthAttribute : AuthorizeAttribute
    {
        private const string AuthDesKey = "bodeauth";

        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            try
            {
                string token = httpContext.Request.Headers.GetValues(HttpHeaderNames.OSharpAuthenticationToken).FirstOrDefault();
                if (token.IsNullOrWhiteSpace()) return false;

                var strAuth = DesHelper.Decrypt(token, AuthDesKey);
                Operator user = strAuth.FromJsonString<Operator>() ?? new Operator();
                OSharpContext.Current.SetOperator(user);

                return int.Parse(user.UserId) > 0 && user.ValidatePeriod > DateTime.Now;
            }
            catch
            {
                return false;
            }
        }
    }
}