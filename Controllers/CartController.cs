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

        public List<CartVM> GetList()
        {
            List<CartVM> meineListe = new List<CartVM>();
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

        public CartVM Mapping(OrderLine databaseData)
        {
            CartVM vm = new CartVM();

            vm.Id = databaseData.Id;
            vm.NetUnitPrice = databaseData.NetUnitPrice;
            vm.Product.ImagePath = databaseData.Product.ImagePath;
            vm.Description = databaseData.Product.Description;
            vm.ManufacturerId = databaseData.Product.ManufacturerId;
            vm.CategoryId = databaseData.Product.CategoryId;
            vm.Amount = databaseData.Amount;
            vm.ManufacturerName = GetManufacturerFromDB(databaseData.Product.ManufacturerId);

            return vm;
        }
    }
}


