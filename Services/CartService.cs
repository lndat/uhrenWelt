using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uhrenWelt.Data;
using uhrenWelt.Models;

namespace uhrenWelt.Services
{
    public class CartService
    {
        public static int CartItemAmount(int? id, List<Cart> sessionList)
        {
            List<Cart> cartList = sessionList;

            for (int i = 0; i < cartList.Count; i++)
            {
                if (cartList[i].Product.Id == id)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}