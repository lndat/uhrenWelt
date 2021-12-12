using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.ViewModel;

namespace uhrenWelt.Controllers
{
    public class ShopController : Controller
    {
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        //[Authorize]
        public ActionResult Shop()
        {
            var tempProductList = GetList();
            return View(tempProductList);
        }

        public ActionResult Search(string search, int? categories, int? manufacturer)
        {
            if (categories == null && manufacturer == null)
            {
                var searchList = GetList()
                    .Where(x => x.ProductName.ToLower()
                        .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                        .Contains(search.ToLower()) || x.Description.ToLower()
                        .Contains(search.ToLower()));

                return View(searchList);
            }

            if (categories != null && manufacturer == null)
            {
                var searchList = GetList()
                    .Where(x => x.ProductName.ToLower()
                        .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                        .Contains(search.ToLower()) || x.Description.ToLower()
                        .Contains(search.ToLower()))
                    .Where(x => x.CategoryId == categories);

                return View(searchList);
            }

            if (categories == null && manufacturer != null)
            {
                var searchList = GetList()
                    .Where(x => x.ProductName.ToLower()
                        .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                        .Contains(search.ToLower()) || x.Description.ToLower()
                        .Contains(search.ToLower()))
                    .Where(x => x.ManufacturerId == manufacturer);

                return View(searchList);
            }

            if (categories != null && manufacturer != null)
            {
                var searchList = GetList()
                    .Where(x => x.ProductName.ToLower()
                        .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                        .Contains(search.ToLower()) || x.Description.ToLower()
                        .Contains(search.ToLower()))
                    .Where(x => x.ManufacturerId == manufacturer && x.CategoryId == categories);

                return View(searchList);
            }

            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tempProductList = GetList().Single(x => x.Id == id);

            Product viewCounter = db.Product.Where(x => x.Id == id).FirstOrDefault();

            viewCounter.ViewCounter = ++viewCounter.ViewCounter;


            if (ModelState.IsValid)
            {
                db.Entry(viewCounter).State = EntityState.Modified;
                db.SaveChanges();
            }

            return View(tempProductList);
        }

        public ActionResult RecommendedMan(int? id)
        {
            var tempProductList = GetList().Single(x => x.Id == id);
            var getRecommended = GetList().Where(x => x.ManufacturerId == tempProductList.ManufacturerId && x.Id != tempProductList.Id).Take(2);
            return PartialView("_Man", getRecommended);
        }

        public ActionResult RecommendedCat(int? id)
        {
            var tempProductList = GetList().Single(x => x.Id == id);
            var getRecommended = GetList().Where(x => x.CategoryId == tempProductList.CategoryId && x.ManufacturerId != tempProductList.ManufacturerId).Take(2);

            return PartialView("_Cat", getRecommended);
        }

        public List<ProductVM> GetList()
        {
            var tempProductList = new List<ProductVM>();
            foreach (var item in GetListFromDB()) tempProductList.Add(Mapping(item));
            return tempProductList;
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
            var vm = new ProductVM();

            vm.Id = databaseData.Id;
            vm.ProductName = databaseData.ProductName;
            vm.NetUnitPrice = databaseData.NetUnitPrice;
            vm.ImagePath = databaseData.ImagePath;
            vm.Description = databaseData.Description;
            vm.ManufacturerId = databaseData.ManufacturerId;
            vm.CategoryId = databaseData.CategoryId;
            vm.ViewCounter = (int)databaseData.ViewCounter;
            vm.ManufacturerName = GetManufacturerFromDB(databaseData.ManufacturerId);
            //vm.Ratings = GetRatingFromDB();

            return vm;
        }

        public ActionResult AddRating(int prodId, string comment,)
        {
            var tempProductList = GetList().Single(x => x.Id == id);
            var getRecommended = GetList().Where(x => x.CategoryId == tempProductList.CategoryId && x.ManufacturerId != tempProductList.ManufacturerId).Take(2);

            return PartialView("_Cat", getRecommended);
        }
        //private int GetRatingFromDB(int prodId, string comment)
        //{

        //}
    }
}