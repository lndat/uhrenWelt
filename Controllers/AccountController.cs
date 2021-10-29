using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using uhrenWelt.Data;
using uhrenWelt.Services;
using uhrenWelt.ViewModels;

namespace uhrenWelt.Controllers
{
    public class AccountController : Controller
    {
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerVM customerVm)
        {
            var newCustomer = new Customer();
            var salt = UserService.CreateSalt(customerVm.PwHash.Length);

            newCustomer.Title = customerVm.Title;
            newCustomer.FirstName = customerVm.FirstName;
            newCustomer.LastName = customerVm.LastName;
            newCustomer.Email = customerVm.Email;
            newCustomer.Street = customerVm.Street;
            newCustomer.Zip = customerVm.Zip;
            newCustomer.City = customerVm.City;
            newCustomer.PwHash = UserService.HashPassword(customerVm.PwHash + salt);
            newCustomer.Salt = salt;

            if (ModelState.IsValid)
            {
                if (UserService.EmailCheck(customerVm.Email))
                {
                    ViewBag.Message = "EmailDupe";
                    return View();
                }

                db.Customer.Add(newCustomer);
                db.SaveChanges();
                AuthenticateUser(newCustomer.Email);
                return RedirectToAction("Index", "Home");
            }

            return View(customerVm);
        }

        public List<CustomerVM> GetList()
        {
            var meineListe = new List<CustomerVM>();
            foreach (var item in GetListFromDB()) meineListe.Add(Mapping(item));
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
            var vm = new CustomerVM();

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

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (email == null || password == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (UserService.LoginCheck(email, password))
            {
                AuthenticateUser(email);
                return RedirectToAction("Shop", "Shop");
            }
            ViewBag.Message = "WrongLoginData";
            return View();
        }

        [NonAction]
        private void AuthenticateUser(string email)
        {
            var timeNow = DateTime.Now;
            var name = email;
            var rememberMe = true;
            var userData = "";

            var ticket = new FormsAuthenticationTicket(
                0,
                name,
                timeNow,
                timeNow.AddMinutes(30),
                rememberMe,
                userData
            );

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName, // default framework cookiename
                encryptedTicket
            );

            // per http response, cookie user übergeben
            Response.Cookies.Add(cookie);
        }
    }
}