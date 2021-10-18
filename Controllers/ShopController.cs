using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using uhrenWelt.Data;
using uhrenWelt.ViewModel;

namespace uhrenWelt.Controllers
{
    public class ShopController : Controller
    {
        [Authorize]
        public ActionResult Shop()
        {
            var temopProductList = GetList();
            return View(temopProductList);
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
            else if (categories != null && manufacturer == null)
            {
                    var searchList = GetList()
                    .Where(x => x.ProductName.ToLower()
                    .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                    .Contains(search.ToLower()) || x.Description.ToLower()
                    .Contains(search.ToLower()))
                    .Where(x => x.CategoryId == categories);

                    return View(searchList);
            }
            else if(categories == null && manufacturer != null)
            {
                    var searchList = GetList()
                    .Where(x => x.ProductName.ToLower()
                    .Contains(search.ToLower()) || x.ManufacturerName.ToLower()
                    .Contains(search.ToLower()) || x.Description.ToLower()
                    .Contains(search.ToLower()))
                    .Where(x => x.ManufacturerId == manufacturer);

                    return View(searchList);
            }
            else if(categories != null && manufacturer != null)
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
            var temopProductList = GetList().Single(x => x.Id == id);
            return View(temopProductList);
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