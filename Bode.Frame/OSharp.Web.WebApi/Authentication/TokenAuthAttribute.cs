using System.Web.Http;
using System.Web.Http.Controllers;
using OSharp.Core.Context;

namespace OSharp.Web.Http.Authentication
{
    public class TokenAuthAttribute : AuthorizeAttribute
    {
        public bool AuthIgnore { get; set; }

        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            //string userId=httpContext.Request.

            int userId = 0;
            int.TryParse(OSharpContext.Current.Operator.UserId, out userId);
            return userId > 0;
        }
    }
}