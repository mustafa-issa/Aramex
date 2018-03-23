using Aramex.Models;
using Newtonsoft.Json;
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

        public ActionResult GetFcuSites()
        {
            try
            {
                DAL siteDAL = new DAL();
                List<FcuSite> sites = new List<FcuSite>();
                sites = siteDAL.GetAllFcuSites().ToList();
                foreach (var site in sites)
                {
                    site.RunHoursWork = siteDAL.GetPreventiveMaintainanceRun(site.RunName);
                    site.PreventiveMaintainanceOverdue = siteDAL.GetPreventiveMaintainanceRun(site.OverdueName);
                }
                var list = JsonConvert.SerializeObject(sites, Formatting.None, new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            });

                return Content(list, "application/json");
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMechanicalSites()
        {
            try
            {
                DAL siteDAL = new DAL();
                List<MechnicalSite> sites = new List<MechnicalSite>();
                sites = siteDAL.GetAllMechnicalSites().ToList();
                var list = JsonConvert.SerializeObject(sites, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });

                return Content(list, "application/json");
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

    }
}