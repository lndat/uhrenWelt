using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uhrenWelt.Data;

namespace uhrenWelt.Services
{
    public partial class CartService
    {
        private static uhrenWeltEntities db = new uhrenWeltEntities();

           public static void AddItemToCart(int? productId, int userId, int amount)
            {
            var newOrderLine = new uhrenWelt.Data.OrderLine();
            var getItem = db.Product.Single(x => x.Id == productId);
            var getOrderId = db.OrderLine.SingleOrDefault(x => x.Order.CustomerId == userId);

            if (getItem != null)
            {
                if (getOrderId == null)
                {
                    newOrderLine.Order.CustomerId = userId;
                    newOrderLine.Product.ImagePath = getItem.ImagePath;
                    newOrderLine.Product.ManufacturerId = getItem.ManufacturerId;
                    newOrderLine.ProductId = getItem.Id;
                    newOrderLine.Product.ProductName = getItem.ProductName;
                    newOrderLine.NetUnitPrice = getItem.NetUnitPrice;
                    newOrderLine.Amount = amount;
 
                    db.OrderLine.Add(newOrderLine);
                }
            }

            db.SaveChanges();
            }
        //public int RemoveFromCart(int id)
        //{
        //    // Get the cart
        //    var cartItem = storeDB.Carts.Single(
        //        cart => cart.CartId == ShoppingCartId
        //        && cart.RecordId == id);

        //    int itemCount = 0;

        //    if (cartItem != null)
        //    {
        //        if (cartItem.Count > 1)
        //        {
        //            cartItem.Count--;
        //            itemCount = cartItem.Count;
        //        }
        //        else
        //        {
        //            storeDB.Carts.Remove(cartItem);
        //        }
        //        // Save changes
        //        storeDB.SaveChanges();
        //    }
        //    return itemCount;
        //}
        //public void EmptyCart()
        //{
        //    var cartItems = storeDB.Carts.Where(
        //        cart => cart.CartId == ShoppingCartId);

        //    foreach (var cartItem in cartItems)
        //    {
        //        storeDB.Carts.Remove(cartItem);
        //    }
        //    // Save changes
        //    storeDB.SaveChanges();
        //}
        //public List<Cart> GetCartItems()
        //{
        //    return storeDB.Carts.Where(
        //        cart => cart.CartId == ShoppingCartId).ToList();
        //}
        //public int GetCount()
        //{
        //    // Get the count of each item in the cart and sum them up
        //    int? count = (from cartItems in storeDB.Carts
        //                  where cartItems.CartId == ShoppingCartId
        //                  select (int?)cartItems.Count).Sum();
        //    // Return 0 if all entries are null
        //    return count ?? 0;
        //}
        //public decimal GetTotal()
        //{
        //    // Multiply album price by count of that album to get 
        //    // the current price for each of those albums in the cart
        //    // sum all album price totals to get the cart total
        //    decimal? total = (from cartItems in storeDB.Carts
        //                      where cartItems.CartId == ShoppingCartId
        //                      select (int?)cartItems.Count *
        //                      cartItems.Album.Price).Sum();

        //    return total ?? decimal.Zero;
        //}
        //public int CreateOrder(Order order)
        //{
        //    decimal orderTotal = 0;

        //    var cartItems = GetCartItems();
        //    // Iterate over the items in the cart, 
        //    // adding the order details for each
        //    foreach (var item in cartItems)
        //    {
        //        var orderDetail = new OrderDetail
        //        {
        //            AlbumId = item.AlbumId,
        //            OrderId = order.OrderId,
        //            UnitPrice = item.Album.Price,
        //            Quantity = item.Count
        //        };
        //        // Set the order total of the shopping cart
        //        orderTotal += (item.Count * item.Album.Price);

        //        storeDB.OrderDetails.Add(orderDetail);

        //    }
        //    // Set the order's total to the orderTotal count
        //    order.Total = orderTotal;

        //    // Save the order
        //    storeDB.SaveChanges();
        //    // Empty the shopping cart
        //    EmptyCart();
        //    // Return the OrderId as the confirmation number
        //    return order.OrderId;
        //}
        //// We're using HttpContextBase to allow access to cookies.
        //public string GetCartId(HttpContextBase context)
        //{
        //    if (context.Session[CartSessionKey] == null)
        //    {
        //        if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
        //        {
        //            context.Session[CartSessionKey] =
        //                context.User.Identity.Name;
        //        }
        //        else
        //        {
        //            // Generate a new random GUID using System.Guid class
        //            Guid tempCartId = Guid.NewGuid();
        //            // Send tempCartId back to client as a cookie
        //            context.Session[CartSessionKey] = tempCartId.ToString();
        //        }
        //    }
        //    return context.Session[CartSessionKey].ToString();
        //}
        //// When a user has logged in, migrate their shopping cart to
        //// be associated with their username
        //public void MigrateCart(string userName)
        //{
        //    var shoppingCart = storeDB.Carts.Where(
        //        c => c.CartId == ShoppingCartId);

        //    foreach (Cart item in shoppingCart)
        //    {
        //        item.CartId = userName;
        //    }
        //    storeDB.SaveChanges();
        //}
    }
}
