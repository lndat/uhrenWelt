using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace uhrenWelt.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Order(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // TODO Create Order
            return new HttpStatusCodeResult(HttpStatusCode.NotImplemented);
        }
    }
}