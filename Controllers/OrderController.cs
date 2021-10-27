﻿using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.ViewModels;

namespace uhrenWelt.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        public ActionResult Order()
        {
            var tempCarttList = GetList();
            return View(tempCarttList);
        }

        // confirming with orderdate & email
        public ActionResult ConfirmOrder(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // TODO customer can change shipping address (in Order)
            SendEmail(GetCustomerByEmail(User.Identity.Name).Email, id);

            Order order = db.Order.Where(x => x.Id == id && x.DateOrdered == null).FirstOrDefault();
            order.DateOrdered = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }

            var tempCarttList = GetList();

            return RedirectToAction("OrderConfirmed", "Order", new { Id = id });
        }

        [HttpPost]
        public ActionResult ChangeAddress(int? id, string street, string city, string zip)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!String.IsNullOrWhiteSpace(street) && !String.IsNullOrWhiteSpace(city) && !String.IsNullOrWhiteSpace(zip))
            {
                Order order = db.Order.Where(x => x.Id == id).FirstOrDefault();
                order.Street = street;
                order.City = city;
                order.Zip = zip;

                if (ModelState.IsValid)
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Order");
        }

        public bool SendEmail(string customerEmail, int? orderId)
        {
            var customer = GetCustomerByEmail(customerEmail);
            var tempCarttList = GetList();

            #region otherwayofcreatingpdf
            //var actionPDF = new Rotativa.ActionAsPdf("OrderPdf")
            //{
            //    FileName = "Rechnung.pdf",
            //    PageSize = Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Landscape,
            //    PageMargins = { Left = 1, Right = 1 }
            //};

            //public ActionResult OrderPdf()
            //{
            //    var tempCarttList = GetList();
            //    return View();
            //}
            #endregion

            var report = new Rotativa.PartialViewAsPdf("_OrderPdf", tempCarttList);
            byte[] applicationPDFData = report.BuildFile(ControllerContext);
            string path = Server.MapPath(@"~/InvoicePdf/Rechnung" + "-" + orderId + ".pdf");
            System.IO.File.WriteAllBytes(path, applicationPDFData);

            var message = new MailMessage(@"testmailuhrenwelt@gmail.com", customerEmail);
            message.Subject = "Vielen Dank für Ihre Bestellung bei uhrenwelt.at!";
            message.Body = $"Ihre Rechnung {customer.FirstName} {customer.LastName}! \n Danke und bis zum nächsten mal :)";
            SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587);
            message.Attachments.Add(new Attachment(path));
            mailer.Credentials = new NetworkCredential("testmailuhrenwelt@gmail.com", "User123!");
            mailer.EnableSsl = true;
            mailer.Send(message);

            return true;
        }

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
            vm.FirstName = GetCustomerByEmail(User.Identity.Name).FirstName;
            vm.LastName = GetCustomerByEmail(User.Identity.Name).LastName;

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