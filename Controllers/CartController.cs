using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.Models;

namespace uhrenWelt.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        // GET: Cart
        public ActionResult ShowCart()
        {
            var getCustomer = db.Customer.Single(x => x.Email == User.Identity.Name);
            var getNoOrderDateCart = db.Order.Where(x => x.CustomerId == getCustomer.Id && x.DateOrdered == null);
            var tempCarttList = GetList();

            if (getNoOrderDateCart.Count() > 0)
                ViewBag.Total = CalculateTotalPrice(getCustomer.Email);

            if (tempCarttList.Count() <= 0)
            {
                ViewBag.Message = "EmptyCart";
                return View();
            }

            return View(tempCarttList);
        }

        public ActionResult AddToCart(int? id, int? amount)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            #region qry
            var getCustomer = db.Customer.Single(x => x.Email == User.Identity.Name);
            var getNoOrderDateCart = db.Order.Where(x => x.CustomerId == getCustomer.Id && x.DateOrdered == null);
            var getProductPrice = db.Product.Single(x => x.Id == id);
            #endregion qry

            if (getNoOrderDateCart.Count() > 0)
                ViewBag.Total = (CalculateTotalPrice(getCustomer.Email) + ((getProductPrice.NetUnitPrice * 1.2m) * amount));
            else
                ViewBag.Total = getProductPrice.NetUnitPrice * 1.2m * amount;

            // check if order with date=null exists -> if not -> a new order is created
            if (getNoOrderDateCart.Count() == 0)
            {
                // create new order
                Order newOrder = new Order();
                newOrder.Id = newOrder.Id++;
                newOrder.CustomerId = getCustomer.Id;
                newOrder.PriceTotal = (GetNetUnitPrice((int)id) * (int)amount) * 1.2m;
                newOrder.Street = getCustomer.Street;
                newOrder.Zip = getCustomer.Zip;
                newOrder.City = getCustomer.City;
                newOrder.DateOrdered = null;

                // create new OrderLine
                OrderLine newOrderLine = new OrderLine();
                newOrderLine.Id = newOrderLine.Id++;
                newOrderLine.OrderId = newOrder.Id;
                newOrderLine.ProductId = (int)id;
                newOrderLine.Amount = (int)amount;
                newOrderLine.NetUnitPrice = GetNetUnitPrice((int)id);
                newOrderLine.TaxRate = GetTaxRate(GetProductCategoryId((int)id));

                if (ModelState.IsValid)
                {
                    db.Order.Add(newOrder);
                    db.OrderLine.Add(newOrderLine);
                    db.SaveChanges();
                }
            }
            // else we find an order where date is null
            else
            {
                var getOrderId = db.Order.Single(x => x.CustomerId == getCustomer.Id && x.DateOrdered == null);
                var checkProductCount = db.OrderLine.Where(x => x.OrderId == getOrderId.Id).Where(x => x.ProductId == (int)id);

                // checkinfo for productId in Order, if it exists edit orderLine (changing product amount)
                if (checkProductCount.Count() > 0)
                {
                    OrderLine orderLine = db.OrderLine.Where(x => x.OrderId == getOrderId.Id && x.ProductId == (int)id).FirstOrDefault();
                    orderLine.Amount += (int)amount;

                    var totalPriceGen = CalculateTotalPrice(getCustomer.Email) + ((getProductPrice.NetUnitPrice * 1.2m) * amount);
                    Order order = db.Order.Where(x => x.Id == getOrderId.Id && x.CustomerId == getCustomer.Id).FirstOrDefault();
                    order.PriceTotal = (decimal)totalPriceGen;

                    if (ModelState.IsValid)
                    {
                        db.Entry(order).State = EntityState.Modified;
                        db.Entry(orderLine).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    // productid not in orderLine, create new OrderLine
                    OrderLine newOrderLine = new OrderLine();
                    newOrderLine.Id = newOrderLine.Id++;
                    newOrderLine.OrderId = getOrderId.Id;
                    newOrderLine.ProductId = (int)id;
                    newOrderLine.Amount = (int)amount;
                    newOrderLine.NetUnitPrice = GetNetUnitPrice((int)id);
                    newOrderLine.TaxRate = GetTaxRate(GetProductCategoryId((int)id));

                    var totalPriceGen = CalculateTotalPrice(getCustomer.Email) + ((getProductPrice.NetUnitPrice * 1.2m) * amount);
                    Order order = db.Order.Where(x => x.Id == getOrderId.Id && x.CustomerId == getCustomer.Id).FirstOrDefault();
                    order.PriceTotal = (decimal)totalPriceGen;

                    if (ModelState.IsValid)
                    {
                        db.OrderLine.Add(newOrderLine);
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
            var tempCarttList = GetList();
            return View(tempCarttList);
        }

        public ActionResult IncrementAmount(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            OrderLine orderLine = db.OrderLine.Find(id);
            orderLine.Amount += 1;

            #region qry
            var getProductPrice = db.Product.Single(x => x.Id == orderLine.ProductId);
            var getCustomer = db.Customer.Single(x => x.Email == User.Identity.Name);
            var totalPriceGen = CalculateTotalPrice(getCustomer.Email) + ((getProductPrice.NetUnitPrice * 1.2m));
            var getOrderId = db.Order.Single(x => x.CustomerId == getCustomer.Id && x.DateOrdered == null);
            Order order = db.Order.Where(x => x.Id == getOrderId.Id && x.CustomerId == getCustomer.Id).FirstOrDefault();
            order.PriceTotal = (decimal)totalPriceGen;
            #endregion qry

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.Entry(orderLine).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        public ActionResult DecrementAmount(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            OrderLine orderLine = db.OrderLine.Find((int)id);
            if (orderLine.Amount > 0)
                orderLine.Amount -= 1;

            if (orderLine.Amount <= 0)
                return RedirectToAction("Delete", new { Id = id });

            #region qry

            var getProductPrice = db.Product.Single(x => x.Id == orderLine.ProductId);
            var getCustomer = db.Customer.Single(x => x.Email == User.Identity.Name);
            var totalPriceGen = CalculateTotalPrice(getCustomer.Email) - ((getProductPrice.NetUnitPrice * 1.2m));
            var getOrderId = db.Order.Single(x => x.CustomerId == getCustomer.Id && x.DateOrdered == null);
            Order order = db.Order.Where(x => x.Id == getOrderId.Id && x.CustomerId == getCustomer.Id).FirstOrDefault();
            order.PriceTotal = (decimal)totalPriceGen;

            #endregion qry

            if (ModelState.IsValid)
            {
                db.Entry(orderLine).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orderLine = db.OrderLine.Find(id);
            db.OrderLine.Remove(orderLine);
            db.SaveChanges();

            return RedirectToAction("ShowCart");
        }

        private decimal CalculateTotalPrice(string email)
        {
            var getCustomer = db.Customer.Single(x => x.Email == User.Identity.Name);
            var getNoOrderDateCart = db.Order.Single(x => x.CustomerId == getCustomer.Id && x.DateOrdered == null);
            var getSum = db.OrderLine.Where(c => c.OrderId == getNoOrderDateCart.Id).Sum(x => (decimal?)x.NetUnitPrice * (int?)x.Amount);

            if (getSum <= 0 || getSum == null) return -1m;

            return (decimal)getSum * 1.2m;
        }

        public List<Cart> GetList()
        {
            var tempProductList = new List<Cart>();
            foreach (var item in GetListFromDB()) tempProductList.Add(Mapping(item));
            return tempProductList;
        }

        public List<OrderLine> GetListFromDB()
        {
            using (var db = new uhrenWeltEntities())
            {
                var cusId = GetCustomerByEmail(User.Identity.Name).Id;
                return db.OrderLine.Where(x => x.Order.CustomerId == cusId).ToList();
            }
        }

        public Cart Mapping(OrderLine databaseData)
        {
            var vm = new Cart();
            vm.Id = databaseData.Id;
            vm.OrderId = databaseData.OrderId;
            vm.ProductId = databaseData.ProductId;
            vm.Amount = databaseData.Amount;
            vm.PriceTotal = GetTotalPrice(databaseData.OrderId);
            vm.NetUnitPrice = GetNetUnitPrice(databaseData.ProductId);
            vm.TaxRate = databaseData.TaxRate;
            vm.ManufacturerName = GetManufacturerName(GetManufacturerId(databaseData.ProductId));
            vm.ProductName = GetProductName(databaseData.ProductId);
            vm.ImagePath = GetImagePath(databaseData.ProductId);

            return vm;
        }

        private int GetManufacturerId(int id)
        {
            var getId = db.Product.Single(x => x.Id == id);
            return getId.ManufacturerId;
        }

        private decimal GetTaxRate(int id)
        {
            var getTax = db.Category.Single(x => x.Id == id);
            return getTax.TaxRate;
        }

        private int GetProductCategoryId(int id)
        {
            var getId = db.Product.Single(x => x.Id == id);
            return getId.CategoryId;
        }

        private string GetManufacturerName(int id)
        {
            var getName = db.Manufacturer.Single(x => x.Id == id);
            return getName.Name;
        }

        private string GetImagePath(int id)
        {
            var getName = db.Product.Single(x => x.Id == id);

            return getName.ImagePath;
        }

        private string GetProductName(int id)
        {
            var getName = db.Product.Single(x => x.Id == id);

            return getName.ProductName;
        }

        private decimal GetNetUnitPrice(int id)
        {
            var getPrice = db.Product.Single(x => x.Id == id);
            return getPrice.NetUnitPrice;
        }

        private decimal GetTotalPrice(int id)
        {
            var getTotalPrice = db.Order.Single(x => x.Id == id);
            return getTotalPrice.PriceTotal;
        }

        public Customer GetCustomerByEmail(string email)
        {
            var c = db.Customer.Single(x => x.Email == email);
            return c;
        }
    }
}