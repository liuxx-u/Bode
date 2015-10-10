using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OSharp.Core.Context;
using OSharp.Utility.Extensions;
using OSharp.Utility.Secutiry;

namespace OSharp.Web.Http.Auth
{
    public class AuthorizeHandler : DelegatingHandler
    {
        private const string AuthDesKey = "bodeauth";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<string> authValues;
                if (request.Headers.TryGetValues("Auth", out authValues))
                {
                    var token = authValues.LastOrDefault();
                    var strAuth = DesHelper.Decrypt(token, AuthDesKey);
                    Operator user = strAuth.FromJsonString<Operator>() ?? new Operator();
                    OSharpContext.Current.SetOperator(user);
                }

                return base.SendAsync(request, cancellationToken);
            }
            catch
            {
                return SendError("请求验证出现错误.", HttpStatusCode.BadRequest);
            }
        }

        private Task<HttpResponseMessage> SendError(string error, HttpStatusCode code)
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(error),
                StatusCode = code
            };

            return Task<HttpResponseMessage>.Factory.StartNew(() => response);
        }
    }
}