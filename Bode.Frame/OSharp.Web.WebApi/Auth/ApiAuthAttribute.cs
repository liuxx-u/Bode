using System.Web.Http;
using System.Web.Http.Controllers;
using OSharp.Core.Context;

namespace OSharp.Web.Http.Auth
{
    public class ApiAuthAttribute : AuthorizeAttribute
    {
        public bool AuthIgnore { get; set; }

        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            int userId = 0;
            int.TryParse(OSharpContext.Current.Operator.UserId, out userId);
            return userId > 0;
        }
    }
}