using Bode.Plugin.Core.SMS;
using OSharp.Core.Dependency;
using OSharp.Utility;

namespace Bode.Sms.Md
{
    /// <summary>
    /// 服务映射信息集合扩展操作
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加漫道短信功能相关映射信息
        /// </summary>
        public static void AddMdSmsServices(this IServiceCollection services)
        {
            services.CheckNotNull("services");
            services.AddSingleton<ISms, MdSms>();
        }
    }
}
