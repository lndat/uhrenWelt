using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.ViewModels;
using Rotativa;
using System.IO;
using System.Web.Routing;
using uhrenWelt.Services;
using System.Net.Mail;
using System.Net.Mime;

namespace uhrenWelt.Controllers
{
    public class OrderController : Controller
    {
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        public ActionResult Order(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tempCarttList = GetList();
            return View(tempCarttList);
        }

        // confirming with orderdate & email
        public ActionResult ConfirmOrder(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SendEmail(GetCustomerByEmail(User.Identity.Name).Email);
            
            Order order = db.Order.Where(x => x.Id == id && x.DateOrdered == null).FirstOrDefault();
            order.DateOrdered = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }

            var tempCarttList = GetList();

            // TODO email & pdf creation to end ordering process
            return RedirectToAction("OrderConfirmed", "Order", new { Id = id });
        }

        public bool SendEmail(string customerEmail)
        {
            var customer = GetCustomerByEmail(User.Identity.Name);
            OrderVM emailBody = new OrderVM();
            var tempCarttList = GetList();
            foreach (var item in tempCarttList)
            {
                emailBody.OrderId = item.OrderId;
                emailBody.ProductId = item.ProductId;
                emailBody.Amount = item.Amount;
                emailBody.NetUnitPrice = item.NetUnitPrice;
                emailBody.PriceTotal = item.PriceTotal;
                emailBody.FirstName = customer.FirstName;
                emailBody.LastName = customer.LastName;
            }

            var message = new MailMessage(@"testmailuhrenwelt@gmail.com", customerEmail);
            message.Subject = "Vielen Dank für Ihre Bestellung bei uhrenwelt.at!";
            message.Body = $"Ihre Rechnung {emailBody.FirstName} {emailBody.LastName}! \nGesamtpreis inkl. MwSt.: {emailBody.PriceTotal}";
            SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587);
            mailer.Credentials = new NetworkCredential("testmailuhrenwelt@gmail.com", "User123!");
            mailer.EnableSsl = true;
            mailer.Send(message);
            return true;
        }

        // thank you page
        public ActionResult OrderConfirmed(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.OrderId = id;

            return View(id);
        }

        public List<OrderVM> GetList()
        {
            var tempProductList = new List<OrderVM>();
            foreach (var item in GetListFromDB()) tempProductList.Add(Mapping(item));
            return tempProductList;
        }

        public List<OrderLine> GetListFromDB()
        {
            using (var db = new uhrenWeltEntities())
            {
                var cusId = GetCustomerByEmail(User.Identity.Name).Id;
                return db.OrderLine.Where(x => x.Order.CustomerId == cusId).Include("Order").Where(x => x.Order.DateOrdered == null).ToList();
            }
        }

        public OrderVM Mapping(OrderLine databaseData)
        {
            var orderData = db.Order.Single(x => x.Id == databaseData.OrderId);

            var vm = new OrderVM();
            vm.ProductId = databaseData.ProductId;
            vm.OrderId = databaseData.OrderId;
            vm.ProductName = GetProductName(databaseData.ProductId);
            vm.Amount = databaseData.Amount;
            vm.NetUnitPrice = databaseData.NetUnitPrice;
            vm.PriceTotal = GetTotalPrice(databaseData.OrderId);
            vm.Street = orderData.Street;
            vm.Zip = orderData.Zip;
            vm.City = orderData.City;

            return vm;
        }

        public Customer GetCustomerByEmail(string email)
        {
            var c = db.Customer.Single(x => x.Email == email);
            return c;
        }

        private decimal GetTotalPrice(int id)
        {
            var getTotalPrice = db.Order.Single(x => x.Id == id);
            return getTotalPrice.PriceTotal;
        }

        private string GetProductName(int id)
        {
            var getName = db.Product.Single(x => x.Id == id);

            return getName.ProductName;
        }
    }
}