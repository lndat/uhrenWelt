using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public const string SESSION_NAME = "CartSession";

        // GET: Cart
        public ActionResult Index()
        {

            return View();
        }

        [Authorize]
        public ActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (Session[SESSION_NAME] == null)
            {
                Debug.WriteLine("NO SESSION FOUND :( NOOOO");
                List<Cart> cartList = new List<Cart>
                {
                    new Cart(db.Product.Find(id), 1)
                };
                Session[SESSION_NAME] = cartList;
            }
            else
            {
                Debug.WriteLine("SESSION FOUND!! SESSION FOUND!! SESSION FOUND!! SESSION FOUND!!");

                List<Cart> cartList = (List<Cart>)Session[SESSION_NAME];
                int check = CartItemAmount(id);
                if (check == -1)
                {
                    cartList.Add(new Cart(db.Product.Find(id), 1));
                }
                else
                {
                    cartList[check].Amount++;
                }
                Session[SESSION_NAME] = cartList;
            }
            return View("Cart");
        }

        public int CartItemAmount(int? id)
        {
            List<Cart> cartList = (List<Cart>)Session[SESSION_NAME];

            for (int i = 0; i < cartList.Count; i++)
            {
                if (cartList[i].Product.Id == id)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}


