using System.Web.Http;

namespace OperadorDestinoCarpetaMock.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
            //xml.UseXmlSerializer = true;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}