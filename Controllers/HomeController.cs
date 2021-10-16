using System.Web.Mvc;

namespace uhrenWelt.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Geschaeftsbedingungen()
        {
            ViewBag.Message = "Geschäftsbedingungen";

            return View();
        }

        public ActionResult Impressum()
        {
            ViewBag.Message = "Impressum";

            return View();
        }

        public ActionResult Datenschutz()
        {
            ViewBag.Message = "Datenschutz";

            return View();
        }
    }
}