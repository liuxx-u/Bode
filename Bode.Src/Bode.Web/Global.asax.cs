using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Bode.Services.Core.Dtos;
using OSharp.Autofac.Http;
using OSharp.Autofac.Mvc;
using OSharp.Core;
using OSharp.Core.Caching;
using OSharp.Core.Dependency;
using OSharp.Logging.Log4Net;
using OSharp.Web.Http.Caching;
using OSharp.Web.Http.Handlers;
using OSharp.Web.Mvc.Routing;
using Bode.Sms.Md;
using OSharp.Web.Http.Context;
using Bode.Services.Implement;
using Bode.Push.Jpush;

namespace Bode.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RoutesRegister();

            DtoMappers.MapperRegister();
            Initialize();

            //注册WebApi的Handler
            GlobalConfiguration.Configure(DelegatingHandlerRegister);
        }

        private static void RoutesRegister()
        {
            RouteCollection routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapLowerCaseUrlRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Bode.Web.Controllers" });
        }

        private static void DelegatingHandlerRegister(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.MessageHandlers.Add(new RequestInitHandler());
            config.MessageHandlers.Add(new ThrottlingHandler(new InMemoryThrottleStore(), id => 100, TimeSpan.FromMinutes(1)));

            //config.MessageHandlers.Add(new SignValidateHandler());
            //config.MessageHandlers.Add(new CultureInitHandler());
        }

        private static void Initialize()
        {

            ICacheProvider provider = new RuntimeMemoryCacheProvider();
            CacheManager.SetProvider(provider, CacheLevel.First);

            IServicesBuilder builder = new ServicesBuilder();
            IServiceCollection services = builder.Build();
            services.AddLog4NetServices();
            services.AddDataServices();
            services.AddImplementServices();
            services.AddMdSmsServices();
            services.AddJPushServices();

            IFrameworkInitializer initializer = new FrameworkInitializer();
            initializer.Initialize(new MvcAutofacIocBuilder(services));
            initializer.Initialize(new WebApiAutofacIocBuilder(services));
            //initializer.Initialize(new SignalRAutofacIocBuilder(services));
        }
    }
}
