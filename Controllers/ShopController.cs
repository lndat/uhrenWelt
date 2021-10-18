﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.ViewModel;

namespace uhrenWelt.Controllers
{
    public class ShopController : Controller
    {
        private uhrenWeltEntities db = new uhrenWeltEntities();

        //[Authorize]
        public ActionResult Shop()
        {
            ViewBag.SelectCat = CreateCategories();

            var temopProductList = GetList();
            return View(temopProductList);
        }

        [HttpGet, ActionName("Search")]
        public ActionResult SearchGet(string search)
        {
            var searchList = GetList()
                .Where(x => x.ProductName.ToLower()
                .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                .Contains(search.ToLower()) || x.Description.ToLower()
                .Contains(search.ToLower()));

            return View(searchList);
        }

        [HttpPost]
        [ActionName("Search")]
        public ActionResult SearchPost(string search)
        {
            var searchList = GetList()
                .Where(x => x.ProductName.ToLower()
                .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                .Contains(search.ToLower()) || x.Description.ToLower()
                .Contains(search.ToLower()));

            return View(searchList);
        }

        public ActionResult Details(int? id)
        {
            var temopProductList = GetList().Single(x => x.Id == id);
            return View(temopProductList);
        }


        private List<SelectListItem> CreateCategories()
        {
            var item1 = new SelectListItem();
            item1.Value = "1";
            item1.Text = "Automatik";

            var item2 = new SelectListItem();
            item2.Value = "1";
            item2.Text = "Smartwatch";

            var item3 = new SelectListItem();
            item3.Value = "1";
            item3.Text = "Mechanisch";

            var items = new List<SelectListItem> { item1, item2, item3 };

            return items;
        }

        public List<ProductVM> GetList()
        {
            List<ProductVM> meineListe = new List<ProductVM>();
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

        public ProductVM Mapping(Product databaseData)
        {
            ProductVM vm = new ProductVM();

            vm.Id = databaseData.Id;
            vm.ProductName = databaseData.ProductName;
            vm.NetUnitPrice = databaseData.NetUnitPrice;
            vm.ImagePath = databaseData.ImagePath;
            vm.Description = databaseData.Description;
            vm.ManufacturerId = databaseData.ManufacturerId;
            vm.CategoryId = databaseData.CategoryId;
            vm.ManufacturerName = GetManufacturerFromDB(databaseData.ManufacturerId);

            return vm;
        }
    }
}