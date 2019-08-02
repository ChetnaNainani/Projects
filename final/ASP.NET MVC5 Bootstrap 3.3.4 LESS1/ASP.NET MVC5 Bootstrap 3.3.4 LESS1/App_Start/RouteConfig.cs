using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC;
namespace ASP.NET_MVC5_Bootstrap3_3_1_LESS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase( // changed from routes.MapRoute
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}