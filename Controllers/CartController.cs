using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.ViewModels;

namespace uhrenWelt.Controllers
{
    public class CartController : Controller
    {
        private uhrenWeltEntities db = new uhrenWeltEntities();

        // GET: Cart
        public ActionResult ShowCart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShowCart(CartVM product)
        {
            return View();
        }

        public List<CartVM> GetList()
        {
            List<CartVM> meineListe = new List<CartVM>();
            foreach (var item in GetListFromDB())
            {
                meineListe.Add(Mapping(item));
            }
            return meineListe;
        }

        public List<Product> GetListFromDB()
        {
            using (var db = new uhrenWeltEntities())
            {
                return db.Product.ToList();
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

        public CartVM Mapping(Product databaseData)
        {
            CartVM vm = new CartVM();

            vm.Id = databaseData.Id;
            vm.ProductName = databaseData.ProductName;
            vm.NetUnitPrice = databaseData.NetUnitPrice;
            vm.ImagePath = databaseData.ImagePath;
            vm.Description = databaseData.Description;
            vm.ManufacturerId = databaseData.ManufacturerId;
            vm.CategoryId = databaseData.CategoryId;
            vm.Amount = databaseData.am;
            vm.ManufacturerName = GetManufacturerFromDB(databaseData.ManufacturerId);

            return vm;
        }
    }
}


