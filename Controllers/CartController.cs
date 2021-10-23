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
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        // GET: Cart
        [Authorize]
        public ActionResult ShowCart()
        {
            var tempCarttList = GetList();

            if (tempCarttList.Count() <= 0)
            {
                ViewBag.Message = "EmptyCart";
                return View();
            }

            return View(tempCarttList);
        }

        [Authorize]
        public ActionResult AddToCart(int? id, int? amount)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var getCustomer = db.Customer.Single(x => x.Email == User.Identity.Name);
            var getNoOrderDateCart = db.Order.Where(x => x.CustomerId == getCustomer.Id && x.DateOrdered == null);

            // if we dont find an order with date = null -> new order is created
            if (getNoOrderDateCart.Count() == 0)
            {
                // create new order
                Order newOrder = new Order();
                newOrder.Id = newOrder.Id++;
                newOrder.CustomerId = getCustomer.Id;
                newOrder.PriceTotal = CalculateTotalPrice(22, 22); // TODO totalprice
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

                    if (ModelState.IsValid)
                    {
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

                    if (ModelState.IsValid)
                    {
                        db.OrderLine.Add(newOrderLine);
                        db.SaveChanges();
                    }
                }
            }

            var tempCarttList = GetList();
            return View(tempCarttList);
        }

        [Authorize]
        public ActionResult Order(int? id)
        {
            // TODO Create Order
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        private decimal CalculateTotalPrice(decimal unitPrice, int quantity)
        {
            return (unitPrice * quantity) * 1.2m;
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
            vm.OrderId = databaseData.Id;
            vm.ProductId = databaseData.ProductId;
            vm.Amount = databaseData.Amount;
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

        public Customer GetCustomerByEmail(string email)
        {
            var c = db.Customer.Single(x => x.Email == email);
            return c;
        }
    }
}