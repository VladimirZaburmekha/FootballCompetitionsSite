using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class CoachesController : Controller
    {
        //
        // GET: /Coaches/

        public ActionResult ShowAllCoaches()    
        {
            var coaches = new BaseService<Coach>(new FootballCompetitionsEntities()).GetAll().ToList().OrderBy(c=>c.TeamId);
            return View(coaches);
        }

        [HttpGet]
        public ActionResult CreateCoach()
        {
            var items = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll().ToList();
            var teams = new SelectList(items, "TeamId", "TeamName");
            ViewBag.Teams = teams;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCoach(Coach coach)
        {
            var coachService = new BaseService<Coach>(new FootballCompetitionsEntities());
            coachService.Add(coach);
            return RedirectToAction("ShowAllCoaches");
        }
        [HttpGet]
        public ActionResult EditCoach(int? id)
        {
            if (id != null)
            {
                var coach = new BaseService<Coach>(new FootballCompetitionsEntities()).Find(c => c.CoachId == id);
                if (coach != null)
                {
                    var items = new BaseService<Team>(new FootballCompetitionsEntities()).GetAll();
                    var teams = new SelectList(items, "TeamId", "TeamName");
                    ViewBag.Teams = teams;
                    if (coach.Address != null)
                    {
                        var cAddress =
                            new BaseService<Address>(new FootballCompetitionsEntities()).Find(
                                a => a.AddressId == coach.AddressId);
                        ViewBag.currentAddress = cAddress.Region + ", " + cAddress.City + ", " + cAddress.Street + ", " +
                                                 cAddress.BuildingNumber;
                    }
                    else
                    {
                        ViewBag.currentAddress = null;
                    }
                    return View(coach);
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
        [HttpPost]
        public ActionResult EditCoach(Coach coach)
        {
            var coachService = new BaseService<Coach>(new FootballCompetitionsEntities());
            coachService.Update(coach, coach.CoachId);
            return RedirectToAction("ShowAllCoaches");
        }
        [HttpGet]
        public ActionResult DetailsOfCoach(int? id)
        {
            if (id != null)
            {
                var coach = new BaseService<Coach>(new FootballCompetitionsEntities()).Find(c => c.CoachId == id);
                if (coach != null)
                {
                    return View(coach);
                }
                else return HttpNotFound();
            }
            return HttpNotFound();
        }

        public ActionResult DeleteCoach(int? id)
        {
            if (id != null)
            {
                var coachService = new BaseService<Coach>(new FootballCompetitionsEntities());
                var coach = coachService.Find(c => c.CoachId == id);
                if (coach != null)
                {
                    coachService.Delete(coach);
                   return RedirectToAction("ShowAllCoaches");
                }
                else return HttpNotFound();
            }
            else return HttpNotFound();
        }
    }
}
