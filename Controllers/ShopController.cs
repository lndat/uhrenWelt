using System.Collections.Generic;
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

        [Authorize]
        public ActionResult RateProduct(int prodId, int rating, string comment)
        {
            var getUser = User.Identity.Name;
            var getCustId = db.Customer.Single(x => x.Email == getUser);

            Rating newRating = new Rating();
            newRating.Comment = comment;
            newRating.Rating1 = rating;
            newRating.ProductId = prodId;
            newRating.CustomerId = getCustId.Id;

            if (ModelState.IsValid)
            {
                db.Rating.Add(newRating);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { Id = prodId });
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tempProductList = GetList().Single(x => x.Id == id);
            return View(tempProductList);
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

        //public static int GetRatingFromDB(int? id)
        //{
        //    using (var db = new uhrenWeltEntities())
        //    {
        //        return db.Rating.Single(x => x.ProductId == id).Rating1;
        //    }
        //}

        //public static string GetCommentFromDB(int? id)
        //{
        //    using (var db = new uhrenWeltEntities())
        //    {
        //        return db.Rating.Single(x => x.ProductId == id).Comment;
        //    }
        //}


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
            vm.ManufacturerName = GetManufacturerFromDB(databaseData.ManufacturerId);
            //vm.Rating = GetRatingFromDB(databaseData.ManufacturerId);
            //vm.Comment = GetCommentFromDB(databaseData.ManufacturerId);

            return vm;
        }


    }
}