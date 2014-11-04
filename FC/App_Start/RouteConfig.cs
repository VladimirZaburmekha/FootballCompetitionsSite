using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "ShowTeams",
                "Teams/ShowTeams/{template}",
                new {controller = "Teams", action = "ShowTeams", template = UrlParameter.Optional}
                );
            routes.MapRoute(
             "ShowCoaches",
             "Coaches/ShowCoaches/{template}",
             new { controller = "Coaches", action = "ShowCoaches", template = UrlParameter.Optional }
           );
           routes.MapRoute(
             "ShowAddresses",
             "Addresses/ShowAddresses/{template}",
             new { controller = "Addresses", action = "ShowAddresses", template = UrlParameter.Optional }
           );
            routes.MapRoute(
                "ShowPlayer",
               "Players/ShowPlayers/{template}",
               new { controller = "Players", action = "ShowPlayers",template=UrlParameter.Optional }

            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Players", action = "ShowAllPlayers", id = UrlParameter.Optional }
            );
           
        }
    }
}