using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.Models;

namespace uhrenWelt.Controllers
{
    public class CartController : Controller
    {
        private uhrenWeltEntities db = new uhrenWeltEntities();

        // GET: Cart
        public ActionResult AddToCart(int? productId)
        {
            var userEmail = User.Identity.Name;
            var getUser = db.Customer.Single(x => x.Email == userEmail);

            uhrenWelt.Services.CartService.AddItemToCart(productId, getUser.Id, 69);
            var temopProductList = GetList().Where(x => x.Id == productId);
            return View(temopProductList);
        }

        public List<OrderDetail> GetList()
        {
            List<OrderDetail> meineListe = new List<OrderDetail>();
            foreach (var item in GetListFromDB())
            {
                meineListe.Add(Mapping(item));
            }
            return meineListe;
        }

        public List<OrderLine> GetListFromDB()
        {
            using (var db = new uhrenWeltEntities())
            {
                return db.OrderLine.ToList();
            }
        }

        public string GetManufacturerFromDB(int? id)
        {
            using (var db = new uhrenWeltEntities())
            {
                var manuId = db.Manufacturer.Find(id);
                return manuId.Name;
            }
        }

        public int GetAmountFromDB(int? id)
        {
            using (var db = new uhrenWeltEntities())
            {
                var manuId = db.OrderLine.Where(x => x.ProductId == id).FirstOrDefault();
                return manuId.Amount;
            }
        }

        public OrderDetail Mapping(OrderLine databaseData)
        {
            OrderDetail vm = new OrderDetail();

            vm.Id = databaseData.Id;
            vm.NetUnitPrice = databaseData.NetUnitPrice;
            vm.ImagePath = databaseData.Product.ImagePath;
            vm.ManufacturerId = databaseData.Product.ManufacturerId;
            vm.Amount = databaseData.Amount;
            vm.ManufacturerName = GetManufacturerFromDB(databaseData.Product.ManufacturerId);
            vm.ProductId = databaseData.ProductId;
            vm.ProductName = databaseData.Product.ProductName;
            vm.NetUnitPrice = databaseData.NetUnitPrice;

            return vm;
        }
    }
}


