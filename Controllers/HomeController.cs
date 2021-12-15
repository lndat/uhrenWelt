using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uhrenWelt.Data;

namespace uhrenWelt.Controllers
{
    public class HomeController : Controller
    {
    private readonly uhrenWeltEntities db = new uhrenWeltEntities();
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

        public ActionResult TopFive()
        {
            List<OrderLine> orders = db.OrderLine.ToList();

            // Daten grp, sum und als liste zurückgeben
            var sums = orders.GroupBy(f => f.ProductId)
               .Select(g => new StatsVM
               {
                   ProductId = g.Key,
                   Amount = g.Sum(f => f.Amount),
                   NetUnitPrice = g.Sum(f => f.NetUnitPrice * f.Amount) * 1.2m,
                   ProductName = GetProductName(g.Key)
               })
               .OrderByDescending(x => x.Amount)
               .ToList();

            ViewBag.Sums = sums;

            return View(sums);

        }

        private string GetProductName(int id)
        {
            var getName = db.Product.Single(x => x.Id == id);

            return getName.ProductName;
        }
    }
}