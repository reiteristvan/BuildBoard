using System.Net.Http.Formatting;
using System.Web.Http;
using BuildBoard.Infrastructure;
using Unity.WebApi;

namespace BuildBoard
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.DependencyResolver = new UnityDependencyResolver(UnityDependencies.Register());
        }
    }
}
