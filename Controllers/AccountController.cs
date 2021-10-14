using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.ViewModels;

namespace uhrenWelt.Controllers
{
    public class AccountController : Controller
    {
        private uhrenWeltEntities db = new uhrenWeltEntities();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Title,FirstName,LastName,Email,Street,Zip,City,PwHash,Salt")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(customer);
        }

        public ActionResult Show()
        {
            return View(GetList());
        }

        public List<CustomerVM> GetList()
        {
            List<CustomerVM> meineListe = new List<CustomerVM>();
            foreach (var item in GetListFromDB())
            {
                meineListe.Add(Mapping(item));
            }
            return meineListe;
        }

        public List<Customer> GetListFromDB()
        {
            using (var db = new uhrenWeltEntities())
            {
                return db.Customer.ToList();
            }
        }

        public CustomerVM Mapping(Customer databaseData)
        {
            CustomerVM vm = new CustomerVM();

            vm.Id = databaseData.Id;
            vm.Title = databaseData.Title;
            vm.FirstName = databaseData.FirstName;
            vm.LastName = databaseData.LastName;
            vm.Email = databaseData.Email;
            vm.Street = databaseData.Street;
            vm.Zip = databaseData.Zip;
            vm.City = databaseData.City;
            vm.PwHash = databaseData.PwHash;
            vm.Salt = databaseData.Salt;

            return vm;
        }
    }
}