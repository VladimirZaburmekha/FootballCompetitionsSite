using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class TeamsController : Controller
    {
        //
        // GET: /Teams/

        public ActionResult ShowAllTeams()
        {
            var teamsSevice = new BaseService<Team>(new FootballCompetitionsEntities());
            var teams = teamsSevice.GetAll();
            return View(teams);
        }
        public ActionResult ShowTeams(string template)
        {
            var teamsService = new BaseService<Team>(new FootballCompetitionsEntities());
            if (string.IsNullOrEmpty(template))
            {
                var teams = teamsService.GetAll();
                return PartialView("TeamsTable", teams);
            }
            else
            {
                var teams = teamsService.GetAll().ToList();
                var selectedTeams =
                    (from t in teams where 
                              t.TeamName.ToLower().Contains(template.ToLower())||
              (t.Address != null && t.Address.ToString().ToLower().Contains(template))
                
                select t).ToList();
                return PartialView("TeamsTable", selectedTeams);
            }
        }

        [HttpGet]
        public ActionResult CreateTeam()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTeam(Team team)
        {
            var teamsService = new BaseService<Team>(new FootballCompetitionsEntities());
            teamsService.Add(team);
            return RedirectToAction("ShowAllTeams");
        }

        [HttpGet]
        public ActionResult EditTeam(int? id)
        {
            if (id != null)
            {
                var team = new BaseService<Team>(new FootballCompetitionsEntities()).Find(t => t.TeamId == id);
                if (team != null)
                {
                    if (team.Address != null)
                    {
                        var cAddress =new BaseService<Address>(new FootballCompetitionsEntities()).Find(a => a.AddressId == team.AddressId);
                        ViewBag.currentAddress = cAddress.ToString();
                    }
                    else
                    {
                        ViewBag.currentAddress = null;
                    }
                    return View(team);
                }
                else return View("Error");
            }
            else return View("Error");
        }

        [HttpPost]
        public ActionResult EditTeam(Team team)
        {
            var teamsService = new BaseService<Team>(new FootballCompetitionsEntities());
            teamsService.Update(team, team.TeamId);
           return RedirectToAction("ShowAllTeams");
        }

    }
}
