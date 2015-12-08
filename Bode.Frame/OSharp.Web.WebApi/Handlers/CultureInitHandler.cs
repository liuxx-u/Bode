using OSharp.Core.Context;
using OSharp.Web.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSharp.Web.Http.Handlers
{
    /// <summary>
    /// 国际区域化信息初始化处理器
    /// </summary>
    public class CultureInitHandler : DelegatingHandler
    {
        /// <summary>
        /// 请求处理
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(HttpHeaderNames.OSharpCultureKey))
            {
                return base.SendAsync(request, cancellationToken);
            }
            string culture = request.Headers.GetValues(HttpHeaderNames.OSharpCultureKey).FirstOrDefault();
            OSharpContext.Current.SetCulture(culture);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
