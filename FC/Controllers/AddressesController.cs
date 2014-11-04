using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public ActionResult ShowAddresses(string template)
        {
            var addressService = new BaseService<Address>(new FootballCompetitionsEntities());
            if (string.IsNullOrEmpty(template))
            {
                var addresses = addressService.GetAll().ToList();
                return PartialView("AddressesTable", addresses);
            }
            else
            {
                var addresses = addressService.GetAll();
                var selectedAddresses = (from a in addresses
                    where
                       (a.BuildingNumber!=null && a.BuildingNumber.ToLower().Contains(template.ToLower())) ||
                       (a.Street!=null && a.Street.ToLower().Contains(template.ToLower())) ||
                       (a.City!=null && a.City.ToLower().Contains(template.ToLower())) ||
                       (a.Region!=null && a.Region.ToLower().Contains(template.ToLower()))
                    select a
                    ).ToList();
                return PartialView("AddressesTable", selectedAddresses);
            }
        }
        //creating
        [HttpGet]
        public ActionResult CreateAddress()
        {
            return View("CreateAddress");
        }
        [HttpPost]
        public ActionResult CreateAddress(Address address)
        {
            var addressService = new BaseService<Address>(new FootballCompetitionsEntities());
            addressService.Add(address);
            return RedirectToAction("ShowAllAddresses");
        }
        //Editing
        [HttpGet]
        public ActionResult EditAddress(int? id)
        {
            if (id != null)
            {
                var address = new BaseService<Address>(new FootballCompetitionsEntities()).Find(a => a.AddressId == id);
                if (address != null)
                {
                    return View(address);
                }
                else
                {
                    return HttpNotFound();
                }

            }
            else return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditAddress(Address address)
        {
            var addressService = new BaseService<Address>(new FootballCompetitionsEntities());
            addressService.Update(address, address.AddressId);
            return RedirectToAction("ShowAllAddresses");
        }
        public JsonResult GetAddressesToAutocomplete(string term)
        {

            var projects = new BaseService<Address>(new FootballCompetitionsEntities()).GetAll();
            var result = (from p in projects
                          where (
                          p.Region!=null && p.Region.ToLower().Contains(term.ToLower())||
                          p.City!=null && p.City.ToLower().Contains(term.ToLower())||
                          p.Street!=null && p.Street.ToLower().Contains(term.ToLower())||
                          p.BuildingNumber!=null && p.BuildingNumber.ToLower().Contains(term.ToLower())
                          )
                          select new
                          {
                              id = p.AddressId,
                              value = p.ToString()
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAddress(int? id)
        {
            if (id != null)
            {
                var addressService = new BaseService<Address>(new FootballCompetitionsEntities());
                var address = addressService.Find(a => a.AddressId == id);
                if (address != null)
                {
                    addressService.Delete(address);
                   return RedirectToAction("ShowAllAddresses");
                }
                else return HttpNotFound();
            }
            else
            {
                return HttpNotFound();
            }
        }
       

    }
}
