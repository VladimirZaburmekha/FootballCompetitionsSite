using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class PlayersController : Controller
    {
        //
        // GET: /Players/

        public ActionResult ShowAllPlayers()
        {var playerService=new BaseService<Player>(new FootballCompetitionsEntities());
            var playerList = playerService.GetAll();
            return View(playerList);
        }

    }
}
