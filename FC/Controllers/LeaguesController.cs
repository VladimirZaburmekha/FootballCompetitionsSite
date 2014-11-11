using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class LeaguesController : Controller
    {
        //
        // GET: /Leagues/
        //SHOW
        [HttpGet]
        public ActionResult ShowAllLeagues()
        {
            var leagues = new BaseService<League>(new FootballCompetitionsEntities()).GetAll();
            return View(leagues);
        }
        public ActionResult ShowLeagues(string template)
        {
            var leaguesService = new BaseService<League>(new FootballCompetitionsEntities());
            var leagues = leaguesService.GetAll();

            if (string.IsNullOrEmpty(template))
            {
                return PartialView("LeaguesTable",leagues);
            }
            else
            {
                var selectedLeagues = (from l in leagues
                    where (l.LeagueName != null && l.LeagueName.ToLower().Contains(template.ToLower())) ||
                          (l.LeagueDescription != null && l.LeagueDescription.ToLower().Contains(template.ToLower()))
                    select l);
                return PartialView("LeaguesTable", selectedLeagues);
            }
        }
        //CREATE
        [Authorize]
        [HttpGet]
        public ActionResult CreateLeague()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateLeague(League league)
        {
            var LeagueService = new BaseService<League>(new FootballCompetitionsEntities());
            LeagueService.Add(league);
            return RedirectToAction("ShowAllLeagues");
        }
        //EDIT
        [Authorize]
        [HttpGet]
        public ActionResult EditLeague(int? id)
        {
            if (id != null)
            {
                var league = new BaseService<League>(new FootballCompetitionsEntities()).Find(l => l.LeagueId == id);
                if (league != null)
                {
                    return View(league);
                }
                else
                {
                    return PartialView("error");
                }
            }
            else
            {
                return PartialView("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditLeague(League league)
        {
            var leaguesService = new BaseService<League>(new FootballCompetitionsEntities());
            leaguesService.Update(league,league.LeagueId);
            return RedirectToAction("ShowAllLeagues");
        }
        //Delete
        [Authorize]
        [HttpGet]
        public ActionResult DeleteLeague(int? id)
        {
            if (id != null)
            {
                var leagueService = new BaseService<League>(new FootballCompetitionsEntities());
                var league = leagueService.Find(l => l.LeagueId == id);
                if (league != null)
                {
                    leagueService.Delete(league);
                    return RedirectToAction("ShowAllLeagues");
                }
                else return PartialView("Error");

            }
            else return PartialView("Error");
        }
    }
}
