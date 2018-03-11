using Aramex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aramex.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report2()
        {
            return View();
        }

        public JsonResult GetSites()
        {
            try
            {
                DAL siteDAL = new DAL();
                List<Site> sites = new List<Site>();
                sites = siteDAL.GetAllSites().ToList();
                return Json(sites, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


    }
}