using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
              (t.Address != null && t.Address.ToString().ToLower().Contains(template))||
              (t.League != null && t.League.LeagueName.ToLower().Contains(template))
                
                select t).ToList();
                return PartialView("TeamsTable", selectedTeams);
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult CreateTeam()
        {
            var leagues = new BaseService<League>(new FootballCompetitionsEntities()).GetAll();
            var Leagues = new SelectList(leagues, "LeagueId", "LeagueName");
            ViewBag.Leagues = Leagues;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateTeam(Team team)
        {
            var teamsService = new BaseService<Team>(new FootballCompetitionsEntities());
            teamsService.Add(team);
            return RedirectToAction("ShowAllTeams");
        }
        [Authorize]
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
                    var leagues = new BaseService<League>(new FootballCompetitionsEntities()).GetAll();
                    var Leagues = new SelectList(leagues, "LeagueId", "LeagueName");
                    ViewBag.Leagues = Leagues;
                    return View(team);
                }
                else return View("Error");
            }
            else return View("Error");
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditTeam(Team team)
        {
            var teamsService = new BaseService<Team>(new FootballCompetitionsEntities());
            teamsService.Update(team, team.TeamId);
           return RedirectToAction("ShowAllTeams");
        }

        [HttpGet]
        public ActionResult DetailsOfTeam(int? id)
        {
            if (id != null)
            {
                var team = new BaseService<Team>(new FootballCompetitionsEntities()).Find(t => t.TeamId == id);
                if (team != null)
                {
                    var players = team.Players;
                    var coaches = team.Coaches;
                    int totalWins = 0;
                    int totalLose = 0;
                    int totalDraws = 0;
                    var mathces = new BaseService<Match>(new FootballCompetitionsEntities()).FindAll(m=>m.Team1Id==team.TeamId||m.Team2Id==team.TeamId);
                    var playerService = new BaseService<Player>(new FootballCompetitionsEntities());
                    var playerIds = from p in
                        playerService.FindAll(p => p.TeamId == team.TeamId)
                        select p.PlayerId;
                    foreach (var item in mathces)
                    {var opponentPlayerIds = new List<int>();
                        if (item.Team1Id == team.TeamId)
                        {
                            opponentPlayerIds = (from p in playerService.FindAll(p => p.TeamId == item.Team2Id)
                                select p.PlayerId).ToList();
                        }
                        else
                        {
                            opponentPlayerIds = (from p in playerService.FindAll(p => p.TeamId == item.Team1Id)
                                                 select p.PlayerId).ToList();
                        }
                        int totalGoalsOfOpponentTeamOfMatch =
                               (from g in item.Goals where opponentPlayerIds.Contains(g.PlayerId) select g).Count();
                        int totalgoalsOfNeedTeamOfMatch =
                            (from g in item.Goals where playerIds.Contains(g.PlayerId) select g).Count();
                        if (totalGoalsOfOpponentTeamOfMatch == totalgoalsOfNeedTeamOfMatch) totalDraws++;
                        if (totalGoalsOfOpponentTeamOfMatch < totalgoalsOfNeedTeamOfMatch) totalWins++;
                        if (totalGoalsOfOpponentTeamOfMatch > totalgoalsOfNeedTeamOfMatch) totalLose++;
                    }
                    ViewBag.totalWins = totalWins;
                    ViewBag.totalDraws = totalDraws;
                    ViewBag.totalLose = totalLose;
                    ViewBag.Coaches = coaches;
                    ViewBag.Players = players;
                    return View(team);
                }
                else
                {
                    return View("Error");
                }
            }
            else return View("Error");
        }
        [Authorize]
        public ActionResult DeleteTeam(int? id)
        {
            if (id != null)
            {
                var teamService = new BaseService<Team>(new FootballCompetitionsEntities());
                var team = teamService.Find(t => t.TeamId == id);
                if (team != null)
                { //getting
                    var goalsService = new BaseService<Goal>(new FootballCompetitionsEntities());
                    var playersService = new BaseService<Player>(new FootballCompetitionsEntities());
                    var matchService = new BaseService<Match>(new FootballCompetitionsEntities());
                    var coachService = new BaseService<Coach>(new FootballCompetitionsEntities());

                    var coaches = coachService.FindAll(c => c.TeamId == id);

                    var players = playersService.FindAll(p => p.TeamId == id);

                    var matches = matchService.FindAll(m => m.Team1Id == id || m.Team2Id == id);
                    var matchesIds = from m in matches select m.MatchId;


                    var goals = from g in goalsService.GetAll() where matchesIds.Contains(g.MatchId) select g;
                    //Deleting
                    foreach (var item in goals)
                    {
                        goalsService.Delete(item);
                    }
                    foreach (var item in players)
                    {
                        playersService.Delete(item);
                    }
                    foreach (var item in matches)
                    {
                        matchService.Delete(item);
                    }
                    foreach (var item in coaches)
                    {
                        coachService.Delete(item);
                    }
                    
                    teamService.Delete(team);
                    return RedirectToAction("ShowAllTeams");
                }
                else
                {
                    return PartialView("Error");
                }
            }
            else
            {
                return PartialView("Error");
            }
        }

        public JsonResult GetTeamsToAutocomplete(string term, int? leagueId)
        {
            if (leagueId == null) leagueId = -1;
            var teams = new BaseService<Team>(new FootballCompetitionsEntities()).FindAll(t=>t.LeagueId==leagueId);
            var selectedTeams =
                (from t in teams
                    where t.TeamName.ToLower().Contains(term)
                    select new {id = t.TeamId, value = t.TeamName}).ToList();
            return Json(selectedTeams, JsonRequestBehavior.AllowGet);
        }

    }
}
