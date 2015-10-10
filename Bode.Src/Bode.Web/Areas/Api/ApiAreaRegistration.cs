using System.Web.Http;
using System.Web.Mvc;

namespace Bode.Web.Areas.Api
{
    public class ApiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Api";
            }
        }


        public override void RegisterArea(AreaRegistrationContext context)
        {

            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Routes.MapHttpRoute(
                "Api_default",
                "Api/{controller}/{action}/{id}",
                new { area = "Api", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                "FluentApi",
                "api/service/{controller}/{id}",
                new { area = "Service", id = RouteParameter.Optional }
            );
        }
    }
}