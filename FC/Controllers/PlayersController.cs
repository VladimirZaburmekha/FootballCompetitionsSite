﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
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
            var playerService = new BaseService<Player>(new FootballCompetitionsEntities());
            var playerList = playerService.GetAll();
            return View(playerList);
        }
        public ActionResult ShowPlayers(string template )
       {
           var playerService = new BaseService<Player>(new FootballCompetitionsEntities());
           
            if (string.IsNullOrEmpty(template))
            {
                var playerList = playerService.GetAll().ToList();
                return PartialView("PlayersTable", playerList);
            }
            else
            {
                var playerList = playerService.GetAll().ToList();
                var selectedPlayers = (from p in playerList
                    where
                        p.PlayerName.ToLower().Contains(template.ToLower()) ||
                        p.PlayerSurname.ToLower().Contains(template.ToLower()) ||
                       p.Team.TeamName.ToLower().Contains(template.ToLower())
                    select p).ToList();
                return PartialView("PlayersTable",selectedPlayers);
            }
       }
        [Authorize]
        [HttpGet]
        public ActionResult CreatePlayer()
        {
            var items = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
            var teams=new SelectList(items,"TeamId","TeamName");
            ViewBag.Teams = teams;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreatePlayer(Player player)
        {
            var playerService = new BaseService<Player>(new FootballCompetitionsEntities());
            playerService.Add(player);
           // var items = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
           // var teams = new SelectList(items, "TeamId", "TeamName");
           // ViewBag.Teams = teams;
            return RedirectToAction("ShowAllPlayers");
        }
        [Authorize]
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
        [Authorize]
        [HttpPost]
        public ActionResult EditPlayer(Player player)
        {
            var playerService = new BaseService<Player>(new FootballCompetitionsEntities()).Update(player,player.PlayerId);
            return RedirectToAction("ShowAllPlayers");
        }
        [Authorize]
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
        [Authorize]
        [HttpGet]
        public ActionResult DetailsOfPlayer(int? id)
        {
            if (id != null)
            {
                var player = new BaseService<Player>(new FootballCompetitionsEntities()).Find(p => p.PlayerId == id);
                if (player != null)
                {
                    ViewBag.TotalGoals =
                        new BaseService<Goal>(new FootballCompetitionsEntities()).FindAll(g => g.PlayerId == id).Count;
                    return View(player);
                }
                else return HttpNotFound();
            }
            else return HttpNotFound();
        }
        [HttpGet]
        public JsonResult GetPlayersToAutoComplete(string term, int team1Id, int team2Id)
        {
            var players = new BaseService<Player>(new FootballCompetitionsEntities()).FindAll(p=>p.TeamId==team1Id|| p.TeamId==team2Id);
            var selectedPlayers = (from p in players
                where
                    (p.PlayerName.ToLower().Contains(term.ToLower())) ||
                    (p.PlayerSurname.ToLower().Contains(term.ToLower())) ||
                    (p.Team != null && p.Team.TeamName.ToLower().Contains(term.ToLower()))
                select new{id=p.PlayerId,value=p.ToString()}
                ).ToList();
            return Json(selectedPlayers, JsonRequestBehavior.AllowGet);
        }

    }
}
