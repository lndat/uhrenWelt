using Rotativa;
using System.Collections.Generic;
using System.Diagnostics;
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
        public const string SESSION_NAME = "CartSession";
        private readonly uhrenWeltEntities db = new uhrenWeltEntities();

        // GET: Cart
        [Authorize]
        public ActionResult ShowCart()
        {
            var cartList = new List<Cart>();
            cartList = (List<Cart>) Session[SESSION_NAME];

            if (cartList == null)
            {
                ViewBag.Message = "EmptyCart";
                return View();
            }

            return View(cartList);
        }
  
        public ActionResult AddToCart(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (Session[SESSION_NAME] == null)
            {
                Debug.WriteLine("NO SESSION FOUND :( NOOOO");
                var cartList = new List<Cart>
                {
                    new Cart(db.Product.Find(id), 1)
                };
                Session[SESSION_NAME] = cartList;
            }
            else
            {
                Debug.WriteLine("SESSION FOUND!! SESSION FOUND!! SESSION FOUND!! SESSION FOUND!!");

                var cartList = (List<Cart>) Session[SESSION_NAME];
                var check = CartItemAmount(id);
                if (check == -1)
                    cartList.Add(new Cart(db.Product.Find(id), 1));
                else
                    cartList[check].Amount++;
                Session[SESSION_NAME] = cartList;
            }

            return View("Cart");
        }

        public int CartItemAmount(int? id)
        {
            var cartList = (List<Cart>) Session[SESSION_NAME];

            for (var i = 0; i < cartList.Count; i++)
                if (cartList[i].Product.Id == id)
                    return i;
            return -1;
        }

        //public ActionResult PrintIndex()
        //{
        //    return new ActionAsPdf("Index", new { name = "ö" }) { FileName = "Test.pdf" };
        //}

        //public ActionResult Index(string name)
        //{
        //    ViewBag.Message = string.Format("Hello {0} to ASP.NET MVC!", name);

        //    return View();
        //}
    }
}