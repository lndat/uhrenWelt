using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uhrenWelt.ViewModel;
using uhrenWelt.Data;

namespace uhrenWelt.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        public CustomerTblVM Mapping(Customer databaseData)
        {
            CustomerTblVM vm = new CustomerTblVM();

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