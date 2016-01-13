using OSharp.Core.Dependency;
using OSharp.Utility;
using Bode.Plugin.Core.Push;

namespace Bode.Push.Jpush
{
    /// <summary>
    /// 服务映射信息集合扩展操作
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddJPushServices(this IServiceCollection services)
        {
            services.CheckNotNull("services");
            services.AddScoped<IPush,JPush>();
        }
    }
}
