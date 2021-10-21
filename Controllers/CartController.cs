using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.Models;

namespace uhrenWelt.Controllers
{
    public class CartController : Controller
    {
        public const string SESSION_NAME = "CartSession";
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        // GET: Cart
        [Authorize]
        public ActionResult ShowCart()
        {
            var cartList = new List<Cart>();
            cartList = (List<Cart>)Session[SESSION_NAME];

            if (cartList == null)
            {
                ViewBag.Message = "EmptyCart";
                return View();
            }

            return View(cartList);
        }

        [Authorize]
        public ActionResult AddToCart(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var getCustomer = db.Customer.Single(x => x.Email == User.Identity.Name);
            var checkForOrder = db.Order.Where(x => x.CustomerId == getCustomer.Id);
            var cartList = new List<Cart> { new Cart(db.Product.Find(id), 1) };

            // Suche nach Order, wenn keine da, Session check und Session erstellen + CartListe bekommt das gewählte Produkt
            if (checkForOrder.Count() <= 0)
            {
                if (Session[SESSION_NAME] == null)
                {
                    Session[SESSION_NAME] = cartList;

                    // neue Order wird erstellt (ohne OrderDate erstmal - Warenkorb "speicherfunktion")
                    var createNewOrder = new Order();

                    foreach (var item in cartList)
                    {
                        createNewOrder.Id = createNewOrder.Id++;
                        createNewOrder.CustomerId = GetCustomerByEmail(User.Identity.Name).Id;
                        createNewOrder.PriceTotal = CalculateTotalPrice(item.Product.NetUnitPrice, item.Amount);
                        createNewOrder.DateOrdered = null;
                        createNewOrder.Street = GetCustomerByEmail(User.Identity.Name).Street;
                        createNewOrder.Zip = GetCustomerByEmail(User.Identity.Name).Zip;
                        createNewOrder.City = GetCustomerByEmail(User.Identity.Name).City;
                    }

                    // auch eine neue OrderLine wird erstellt
                    var createNewOrderLine = new OrderLine();

                    foreach (var item in cartList)
                    {
                        createNewOrderLine.Id = createNewOrderLine.Id++;
                        createNewOrderLine.OrderId = createNewOrder.Id;
                        createNewOrderLine.ProductId = (int)id;
                        createNewOrderLine.Amount = item.Amount;
                        createNewOrderLine.NetUnitPrice = item.Product.NetUnitPrice;
                        createNewOrderLine.TaxRate = createNewOrderLine.TaxRate;
                    }

                    if (ModelState.IsValid)
                    {
                        db.Order.Add(createNewOrder);

                        if (ModelState.IsValid)
                        {
                            db.OrderLine.Add(createNewOrderLine);
                        }
                        db.SaveChanges();
                    }
                }
                //else
                //{
                //    var cartList = (List<Cart>)Session[SESSION_NAME];
                //    var check = CartItemAmount(id);
                //    if (check == -1)
                //        cartList.Add(new Cart(db.Product.Find(id), 1));
                //    else
                //        cartList[check].Amount++;
                //    Session[SESSION_NAME] = cartList;
                //}

                //var newOrder = new Order();

                //foreach (var item in (List<Cart>)Session[SESSION_NAME])
                //{
                //    newOrder.Id = newOrder.Id++;
                //    newOrder.CustomerId = GetCustomerByEmail(User.Identity.Name).Id;
                //    newOrder.PriceTotal = CalculateTotalPrice(item.Product.NetUnitPrice, item.Amount);
                //    newOrder.DateOrdered = null;
                //    newOrder.Street = GetCustomerByEmail(User.Identity.Name).Street;
                //    newOrder.Zip = GetCustomerByEmail(User.Identity.Name).Zip;
                //    newOrder.City = GetCustomerByEmail(User.Identity.Name).City;
                //}

                //// auch eine neue OrderLine wird erstellt
                //var newOrderLine = new OrderLine();

                //foreach (var item in (List<Cart>)Session[SESSION_NAME])
                //{
                //    newOrderLine.Id = newOrderLine.Id++;
                //    newOrderLine.OrderId = newOrder.Id;
                //    newOrderLine.ProductId = (int)id;
                //    newOrderLine.Amount = item.Amount;
                //    newOrderLine.NetUnitPrice = item.Product.NetUnitPrice;
                //    newOrderLine.TaxRate = newOrderLine.TaxRate;
                //}

                //if (ModelState.IsValid)
                //{
                //    db.Entry(newOrder).State = EntityState.Modified;

                //    if (ModelState.IsValid)
                //    {
                //        db.Entry(newOrderLine).State = EntityState.Modified;
                //    }
                //    db.SaveChanges();
                //}
            }

            //if (Session[SESSION_NAME] == null)
            //{
            //    Debug.WriteLine("NO SESSION FOUND :( NOOOO");
            //    var cartList = new List<Cart>
            //    {
            //        new Cart(db.Product.Find(id), 1)
            //    };
            //    Session[SESSION_NAME] = cartList;
            //}
            //else
            //{
            //    Debug.WriteLine("SESSION FOUND!! SESSION FOUND!! SESSION FOUND!! SESSION FOUND!!");

            //    var cartList = (List<Cart>)Session[SESSION_NAME];
            //    var check = CartItemAmount(id);
            //    if (check == -1)
            //        cartList.Add(new Cart(db.Product.Find(id), 1));
            //    else
            //        cartList[check].Amount++;
            //    Session[SESSION_NAME] = cartList;
            //}

            return View(cartList);
        }

        private decimal CalculateTotalPrice(decimal unitPrice, int quantity)
        {
            return (unitPrice * quantity) * 1.2m;
        }

        public int CartItemAmount(int? id)
        {
            var cartList = (List<Cart>)Session[SESSION_NAME];

            for (var i = 0; i < cartList.Count; i++)
                if (cartList[i].Product.Id == id)
                    return i;
            return -1;
        }

        public Customer GetCustomerByEmail(string email)
        {
            var c = db.Customer.Single(x => x.Email == email);
            return c;
        }
    }
}