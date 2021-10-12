using System.Linq;
using System.Net;
using System.Web.Mvc;
using uhrenWelt.Data;

namespace uhrenWelt.Controllers
{
    public class HomeController : Controller
    {
        private uhrenWeltEntities db = new uhrenWeltEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Shop()
        {
            return View(db.Product);
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

        public ActionResult Details(int? id)
        {
            ViewBag.ProductId = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Data.Product customer = db.Product.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

    }
}