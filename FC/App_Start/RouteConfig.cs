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
          /*  routes.MapRoute(
             "Players",
             "players",
             new { controller = "PlayersController", action = "ShowAllPlayers" }
           );
            routes.MapRoute(
                "EditPlayer",
               "Players/EditPlayer/{id}",
               new { controller = "PlayersController", action = "CreatePlayer",id="{id}" }

            );*/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Players", action = "ShowAllPlayers", id = UrlParameter.Optional }
            );
           
        }
    }
}