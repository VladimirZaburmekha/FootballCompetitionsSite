using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class MatchesController : Controller
    {
        //
        // GET: /Matches/
        private string GetMatchScore(Match match)
        {
            var playersService = new BaseService<Player>(new FootballCompetitionsEntities());
            var players = playersService.FindAll(p => p.TeamId == match.Team1Id || p.TeamId == match.Team2Id);
            
            var team1PlayersIds = (from p in players where p.TeamId == match.Team1Id select p.PlayerId).ToList();
            var team2PlayersIds = (from p in players where p.TeamId == match.Team2Id select p.PlayerId).ToList();

            int team1Score=(from g in match.Goals where team1PlayersIds.Contains(g.PlayerId) select g).Count();
            int team2Score=(from g in match.Goals where team2PlayersIds.Contains(g.PlayerId) select g).Count();

            return team1Score + " : " + team2Score;


        }
        //SHOW AND SEARCH
        [HttpGet]
        public ActionResult ShowAllMatches()
        {
            var matches = new BaseService<Match>(new FootballCompetitionsEntities()).GetAll().OrderByDescending(m=>m.Date);
            var matchScores= matches.Select(GetMatchScore).ToList();
            ViewBag.matchScores = matchScores;
            return View(matches);
        }
        [HttpGet]
        public ActionResult ShowMatches(string template)
        {
            var matches = new BaseService<Match>(new FootballCompetitionsEntities()).GetAll().OrderByDescending(m=>m.Date);
            if (string.IsNullOrEmpty(template))
            {
                var matchScores = matches.Select(GetMatchScore).ToList();
                ViewBag.matchScores = matchScores;
                return PartialView("MatchesTable", matches);
            }
            else
            {
                var selectedMatches = (from m in matches
                    where (m.Address != null && m.Address.ToString().ToLower().Contains(template.ToLower())) ||
                          (m.League != null && m.League.LeagueName.ToLower().Contains(template.ToLower())) ||
                          (m.MatchName != null && m.MatchName.ToLower().Contains(template.ToLower())) ||
                          (m.Team != null && m.Team.TeamName.ToLower().Contains(template.ToLower())) ||
                          (m.Team1 != null && m.Team1.TeamName.ToLower().Contains(template.ToLower()))
                    select m
                    ).OrderByDescending(m=>m.Date).ToList();
                var matchScores = matches.Select(GetMatchScore).ToList();
                ViewBag.matchScores = matchScores;
                return PartialView("MatchesTable",selectedMatches);
            }
        }
        //Creating
        [Authorize]
        [HttpGet]
        public ActionResult CreateMatch()
        {
            var teamItems = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
            var leaguesitems = new BaseService<League>(new FootballCompetitionsEntities()).GetAll();
            var leagues = new SelectList(leaguesitems, "LeagueId", "LeagueName");
            var teams = new SelectList(teamItems, "TeamId", "TeamName");
            ViewBag.Teams = teams;
            ViewBag.Leagues = leagues;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateMatch(Match match)
        {
            var matchService = new BaseService<Match>(new FootballCompetitionsEntities());
            matchService.Add(match);
            return RedirectToAction("ShowAllMatches");
        }
        [Authorize]
        //EDITING
        [Authorize]
        [HttpGet]
        public ActionResult EditMatch(int? id)
        {
            if (id != null)
            {
                var match = new BaseService<Match>(new FootballCompetitionsEntities()).Find(m => m.MatchId == id);
                if (match != null)
                {
                    var teamItems = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
                    var leaguesitems = new BaseService<League>(new FootballCompetitionsEntities()).GetAll();
                    var leagues = new SelectList(leaguesitems, "LeagueId", "LeagueName");
                    var teams = new SelectList(teamItems, "TeamId", "TeamName");
                    ViewBag.Goals = match.Goals;
                    ViewBag.Teams = teams;
                    ViewBag.Leagues = leagues;
                    if (match.Address != null)
                    {
                        ViewBag.currentAddress = match.Address.ToString();
                    }
                    else ViewBag.currentAddress = null;
                    return View(match);
                }
                else return PartialView("Error");
            }
            else return PartialView("Error");
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditMatch(Match match)
        {
            if (match != null){ 
                var matchService = new BaseService<Match>(new FootballCompetitionsEntities());
                matchService.Update(match, match.MatchId);
                return RedirectToAction("ShowAllMatches");
            }
            else return PartialView("Error");
        }
        [HttpGet]
        public ActionResult DetailsOfMatch(int? id)
        {
            if (id != null)
            {
                var match = new BaseService<Match>(new FootballCompetitionsEntities()).Find(m=>m.MatchId==id);
                if (match != null)
                {
                    var goals = match.Goals;
                    ViewBag.Goals = goals;
                    return View(match);
                }
                else
                {
                    return HttpNotFound();
                }

            }
            else
            {
                return HttpNotFound();
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult DeleteMatch(int? id)
        {
            if (id != null)
            {
                var matchService = new BaseService<Match>(new FootballCompetitionsEntities());
                var match = matchService.Find(m => m.MatchId == id);
                if (match != null)
                {
                    matchService.Delete(match);
                    return RedirectToAction("ShowAllMatches");
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}
