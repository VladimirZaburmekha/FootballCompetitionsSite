using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class GoalController : Controller
    {
        //
        // GET: /Goal/
        [Authorize]
        public ActionResult AddGoal(int? matchId,int? playerId,int? minute)
        {
            if (matchId == null || playerId == null)
            {
                return PartialView("Error");
            }
            else
            {
                var match = new BaseService<Match>(new FootballCompetitionsEntities()).Find(m => m.MatchId == matchId);
                var player = new BaseService<Player>(new FootballCompetitionsEntities()).Find(p => p.PlayerId == playerId);
                if (match != null && player != null)
                {
                    var goalService = new BaseService<Goal>(new FootballCompetitionsEntities());
                   // var goal = new Goal((int) matchId, (int) playerId, minute);
                    var goal= new Goal();
                    goal.PlayerId = (int) playerId;
                    goal.MatchId = (int) matchId;
                    goal.Minute = minute;
                    goalService.Add(goal);
                }
                var goals = new BaseService<Goal>(new FootballCompetitionsEntities()).FindAll(g => g.MatchId == matchId);
                return PartialView("GoalsTable",goals);
            }
            
        }
        [Authorize]
        public ActionResult DeleteGoal(int? id)
        {
            if (id == null)
            {
                return PartialView("Error");
                //return Json(new { error = true });
            }
            else
            {
                var goalService = new BaseService<Goal>(new FootballCompetitionsEntities());
                var goal = goalService.Find(g => g.GoalId == id);
                if (goal != null)
                {
                    int matchId = goal.MatchId;
                    goalService.Delete(goal);
                    var allGoals = goalService.FindAll(g => g.MatchId == matchId);
                    return PartialView("GoalsTable", allGoals);
                    //return Json(new { success = true });
                }
                else
                {
                    return PartialView("Error");
                   // return Json(new { error = true });
                }
            }
        }
        [Authorize]
        public ActionResult GetGoalsPerMatch(int? matchId)
        {
            if (matchId != null)
            {
                var goals = new BaseService<Goal>(new FootballCompetitionsEntities()).FindAll(g => g.MatchId == matchId);
                return PartialView("GoalsTable", goals);
            }
            else
            {
                return PartialView("Error");
            }
        }

    }
}
