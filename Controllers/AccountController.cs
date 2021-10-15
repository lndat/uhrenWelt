﻿using System.Collections.Generic;
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
        public ActionResult Register(CustomerVM customerVm)
        {
            var newCustomer = new Customer();
            var salter = uhrenWelt.Services.UserService.CreateSalt(customerVm.PwHash.Length);

            newCustomer.Title = customerVm.Title;
            newCustomer.FirstName = customerVm.FirstName;
            newCustomer.LastName = customerVm.LastName;
            newCustomer.Email = customerVm.Email;
            newCustomer.Street = customerVm.Street;
            newCustomer.Zip = customerVm.Zip;
            newCustomer.City = customerVm.City;
            newCustomer.PwHash = uhrenWelt.Services.UserService.HashPassword(customerVm.PwHash + salter);
            newCustomer.Salt = salter;

            if (ModelState.IsValid)
            {
                db.Customer.Add(newCustomer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(); //TODO
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