using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uhrenWelt.Data;

namespace uhrenWelt.Controllers
{
    public class HomeController : Controller
    {
        private uhrenWeltEntities db = new uhrenWeltEntities();
        public ActionResult Index()
        {
            return View(db.Product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Impressum()
        {
            ViewBag.Message = "Impressum";

            return View();
        }
    }
}