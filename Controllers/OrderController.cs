using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using uhrenWelt.Data;
using uhrenWelt.ViewModels;

namespace uhrenWelt.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        public ActionResult Order(int? id)
        {
            if (CheckNewCustomer(GetCustomerByEmail(User.Identity.Name).Id) && CountOrders(id) < 10)
            {
                ViewBag.Message = "NewCustomer";
            }
            var tempCarttList = GetList();
            return View(tempCarttList);
        }

        // change address in current order
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

            return RedirectToAction("Order", new { Id = id });
        }

        public ActionResult OrderPdf(int? id)
        {
            if (CheckNewCustomer(GetCustomerByEmail(User.Identity.Name).Id) && CountOrders(id) < 10)
            {
                ViewBag.Message = "ShowDiscount";
            }
            var tempCarttList = GetList();
            return View(tempCarttList);
        }

        public bool SendMail(string customerEmail, int? orderId)
        {
            var customer = GetCustomerByEmail(customerEmail);
            var tempCarttList = GetList();

            #region otherwayofcreatingpdf

            //var actionPDF = new Rotativa.ActionAsPdf("OrderPdf")
            //{
            //    PageSize = Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 },
            //    FormsAuthenticationCookieName = FormsAuthentication.FormsCookieName
            //};


            //var partialPdf = new Rotativa.PartialViewAsPdf("_OrderPdf", tempCarttList)
            //{
            //    PageSize = Size.A4,
            //    PageOrientation = Rotativa.Options.Orientation.Portrait,
            //    PageMargins = { Left = 1, Right = 1 },
            //    FormsAuthenticationCookieName = FormsAuthentication.FormsCookieName
            //};

            #endregion otherwayofcreatingpdf


            var partialPdf = new Rotativa.ActionAsPdf("OrderPdf", new {Id = orderId });
            if (CheckNewCustomer(GetCustomerByEmail(User.Identity.Name).Id) && CountOrders(orderId) < 10)
            {
                ViewBag.Message = "ShowDiscount";
            }
            byte[] invoicePdfData = partialPdf.BuildFile(ControllerContext);
            string path = Server.MapPath(@"~/InvoicePdf/Rechnung" + "-" + orderId + ".pdf");
            System.IO.File.WriteAllBytes(path, invoicePdfData);

            //var message = new MailMessage(@"ITN241552@qualifizierung.at", customerEmail);
            //message.Subject = $"Deine Bestellung bei uhrenwelt.at (Nr. {orderId})";
            //message.Body = $"Hallo {customer.FirstName} {customer.LastName}! " +
            //               "\nVielen Dank für deine Bestellung bei uhrenwelt.at." +
            //               "\nIm Anhang findest du deine Rechnung," +
            //               "\nbis zum nächsten mal!" +
            //               "\n" +
            //               "\nDein uhrenwelt.at Team :)";
            //SmtpClient mailer = new SmtpClient("smtp.office365.com", 587);
            //message.Attachments.Add(new Attachment(path));
            //mailer.Credentials = new NetworkCredential("emailadresse@email.at","passwort");
            //mailer.EnableSsl = true;
            //mailer.Send(message);

            return true;
        }

        // confirming order
        public ActionResult ConfirmOrder(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var custId = GetCustomerByEmail(User.Identity.Name).Id;
            Order order = db.Order.Where(x => x.Id == id && x.DateOrdered == null).FirstOrDefault();
            OrderLine orderLine = db.OrderLine.Where(x => x.OrderId == order.Id).FirstOrDefault();
            OrderLine orderLineDiscountCheck = db.OrderLine.Where(x => x.OrderId == order.Id && x.CustomerId == custId).FirstOrDefault();


            if (CheckNewCustomer(GetCustomerByEmail(User.Identity.Name).Id) && CountOrders(id) < 10)
            {
                if (CountOrders(id) < 10)
                {
                    ViewBag.Message = "ShowDiscount";
                    order.PriceTotal = order.PriceTotal - (order.PriceTotal / 100) * 3;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            SendMail(GetCustomerByEmail(User.Identity.Name).Email, id);
            order.DateOrdered = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }

            var tempCarttList = GetList();

            return RedirectToAction("OrderConfirmed", "Order", new { Id = id });
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
            vm.CStreet = GetCustomerByEmail(User.Identity.Name).Street;
            vm.CZip = GetCustomerByEmail(User.Identity.Name).Zip;
            vm.CCity = GetCustomerByEmail(User.Identity.Name).City;

            return vm;
        }
        public Customer GetCustomerByEmail(string email)
        {
            var c = db.Customer.Single(x => x.Email == email);
            return c;
        }

        
        public int CountOrders(int? id)
        {
            var custId = GetCustomerByEmail(User.Identity.Name).Id;
            Order order = db.Order.Where(x => x.Id == id && x.DateOrdered == null).FirstOrDefault();
            OrderLine orderLine = db.OrderLine.Where(x => x.OrderId == order.Id).FirstOrDefault();
            //OrderLine orderLineDiscountCheck = db.OrderLine.Where(x => x.OrderId == order.Id && x.CustomerId == custId).FirstOrDefault();
            var orderLineDiscountCheck = db.OrderLine.Where(x => x.OrderId == order.Id && x.CustomerId == custId).Sum(y => y.Amount);



            var orderLineDiscountChec2 = db.OrderLine.Select(g => new { Amount = g.CustomerId, TotalCount = g.Amount }).Count();


            return orderLineDiscountCheck;
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

        private bool CheckNewCustomer(int? id)
        {
            if (db.Order.Where(x => x.CustomerId == id && x.DateOrdered != null).Any())
            {
                return false;
            }

            return true;
        }
    }
}