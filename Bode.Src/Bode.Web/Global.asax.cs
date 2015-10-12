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
using OSharp.SiteBase.Initialize;
using OSharp.Web.Http.Authentication;
using OSharp.Web.Http.Caching;
using OSharp.Web.Http.Handlers;
using OSharp.Web.Http.Initialize;
using OSharp.Web.Mvc.Initialize;
using OSharp.Web.Mvc.Routing;

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
            config.MessageHandlers.Add(new ThrottlingHandler(new InMemoryThrottleStore(), id => 60, TimeSpan.FromMinutes(1)));
            config.MessageHandlers.Add(new TokenAuthenticationHandler(new OnlineUserStore()));
        }

        private static void Initialize()
        {
            ICacheProvider provider = new RuntimeMemoryCacheProvider();
            CacheManager.SetProvider(provider, CacheLevel.First);

            //MVC初始化
            IFrameworkInitializer initializer = new MvcFrameworkInitializer()
            {
                BasicLoggingInitializer = new Log4NetLoggingInitializer(),
                IocInitializer = new MvcAutofacIocInitializer()
            };
            initializer.Initialize();

            //WebApi初始化
            initializer = new WebApiFrameworkInitializer()
            {
                BasicLoggingInitializer = new Log4NetLoggingInitializer(),
                IocInitializer = new WebApiAutofacIocInitializer()
            };
            initializer.Initialize();
        }
    }
}
