using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BootstrapMvcSample.Controllers;
using FC.Controllers;
using NavigationRoutes;

namespace BootstrapMvcSample
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           // routes.MapNavigationRoute<HomeController>("Automatic Scaffolding", c => c.Index());
            routes.MapNavigationRoute<MatchesController>("Матчі", c => c.ShowAllMatches());
            routes.MapNavigationRoute<PlayersController>("Гравці", c => c.ShowAllPlayers());
            routes.MapNavigationRoute<TeamsController>("Команди", c => c.ShowAllTeams());
            routes.MapNavigationRoute<CoachesController>("Тренери", c => c.ShowAllCoaches());
            routes.MapNavigationRoute<AddressesController>("Адреси", c => c.ShowAllAddresses());
            routes.MapNavigationRoute<LeaguesController>("Ліги", c => c.ShowAllLeagues());
           // routes.MapNavigationRoute<MatchesController>("Навігація", c => c.ShowAllMatches())
                 /* .AddChildRoute<PlayersController>("Гравці", c => c.ShowAllPlayers())
                  .AddChildRoute<TeamsController>("Команди", c => c.ShowAllTeams())
                  .AddChildRoute<CoachesController>("Тренери", c => c.ShowAllCoaches())
                  .AddChildRoute<AddressesController>("Адреси", c => c.ShowAllAddresses())
                  .AddChildRoute<LeaguesController>("Ліги", c => c.ShowAllLeagues())
                  .AddChildRoute<MatchesController>("Матчі", c => c.ShowAllMatches())
                //  .AddChildRoute<ExampleLayoutsController>("Sign In", c => c.SignIn())
                ;*/
            routes.MapNavigationRoute<ExampleLayoutsController>("Адміністратор", c => c.Starter())
                .AddChildRoute<AccountController>("Увійти", c => c.Login())
                .AddChildRoute<AccountController>("Вийти", c => c.Logout());
        }
    }
}
