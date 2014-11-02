using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class PlayersController : Controller
    {
        //
        // GET: /Players/

        public ActionResult ShowAllPlayers()
       {
            var playerService=new BaseService<Player>(new FootballCompetitionsEntities());
            var playerList = playerService.GetAll();
            return View(playerList);
       }
        [HttpGet]
        public ActionResult CreatePlayer()
        {
            var items = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
            var teams=new SelectList(items,"TeamId","TeamName");
            ViewBag.Teams = teams;
            return View();
        }
        [HttpPost]
        public ActionResult CreatePlayer(Player player)
        {
            var playerService = new BaseService<Player>(new FootballCompetitionsEntities());
            playerService.Add(player);
            var items = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
            var teams = new SelectList(items, "TeamId", "TeamName");
            ViewBag.Teams = teams;
            return RedirectToAction("ShowAllPlayers");
        }
        [HttpGet]
        public ActionResult EditPlayer(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Player player =new BaseService<Player>(new FootballCompetitionsEntities()).Find(p=>p.PlayerId==id);
            if (player != null)
            {
                var items = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
                var teams = new SelectList(items, "TeamId", "TeamName");
                ViewBag.Teams = teams;
                if (player.AddressId != null)
                {
                    var cAddress =
                        new BaseService<Address>(new FootballCompetitionsEntities()).Find(
                            a => a.AddressId == player.AddressId);
                    ViewBag.currentAddress = cAddress.Region + ", " + cAddress.City + ", " + cAddress.Street + ", " +
                                             cAddress.BuildingNumber;
                }
                else ViewBag.currentAddress = null;
                
                return View(player);
            }
            return RedirectToAction("ShowAllPlayers");
        }
        [HttpPost]
        public ActionResult EditPlayer(Player player,int id)
        {
            var playerService = new BaseService<Player>(new FootballCompetitionsEntities()).Update(player,id);
            return RedirectToAction("ShowAllPlayers");
        }

        public ActionResult DeletePlayer(int? id)
        {
            if (id != null)
            {
                var baseService = new BaseService<Player>(new FootballCompetitionsEntities());
                var playerToRemove =baseService.Find(p => p.PlayerId == id);
                baseService.Delete(playerToRemove);
                return RedirectToAction("ShowAllPlayers");
            }
            else
            {
                return PartialView("Error");
            }
        }

        [HttpGet]
        public ActionResult DetailsOfPlayer(int? id)
        {
            var player = new BaseService<Player>(new FootballCompetitionsEntities()).Find(p=>p.PlayerId==id);
            ViewBag.TotalGoals = new BaseService<Goal>(new FootballCompetitionsEntities()).FindAll(g => g.PlayerId == id).Count;
            return View(player);
        }

    }
}
