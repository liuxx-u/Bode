using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSharp.Web.Http.Context
{
    /// <summary>
    /// 保存HttpRequestMessage
    /// </summary>
    public class RequestInitHandler: DelegatingHandler
    {
        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            OSharpWebApiContext.Current = new OSharpWebApiContext();
            OSharpWebApiContext.Current.SetDependencyScope(request.GetDependencyScope());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
