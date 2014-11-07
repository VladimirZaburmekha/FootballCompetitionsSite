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

        public ActionResult ShowAllMatches()
        {
            var matches = new BaseService<Match>(new FootballCompetitionsEntities()).GetAll();
            return View(matches);
        }

        public ActionResult ShowMatches(string template)
        {
            var matches = new BaseService<Match>(new FootballCompetitionsEntities()).GetAll();
            if (string.IsNullOrEmpty(template))
            {
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
                    ).ToList();
                return PartialView("MatchesTable",selectedMatches);
            }
        }

    }
}
