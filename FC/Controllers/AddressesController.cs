using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using FC.Models;
using FC.Services;

namespace FC.Controllers
{
    public class AddressesController : Controller
    {
        //
        // GET: /Addresses/

        public ActionResult ShowAllAddresses()
        {
            var addresses=new BaseService<Address>(new FootballCompetitionsEntities()).GetAll().ToList();
            return View(addresses);
        }
        public ActionResult CreateAddress()
        {
            return PartialView("CreateAddress");
        }

        public JsonResult GetAddressesToAutocomplete(string term)
        {

            var projects = new BaseService<Address>(new FootballCompetitionsEntities()).GetAll();
            var result = (from p in projects
                          where p.Street.ToLower().Contains(term.ToLower()) 
                          select new
                          {
                              id = p.AddressId,
                              value = p.Region+", "+p.City+", "+p.Street+", "+p.BuildingNumber,
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult SelectAddress()
        {
            return View();
        }

    }
}
