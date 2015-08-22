using Grocery3Go.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Grocery3Go
{
    public static class CartCacheService
    {
        private static CartCache _cache;
 
        static CartCacheService()
        {
            _cache = new CartCache();

            HttpRuntime.Cache["ShoppingCartCounts"] = _cache;
        }

        public static int GetUserCartCount(string userId)
        {

            //Catches Here
            //Issue's where no User is currently Logged In?

            if (_cache.UserShoppingCartCounts.ContainsKey(userId))
                return _cache.UserShoppingCartCounts[userId];
            else
            {
                int shoppingCartCount = 0;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    shoppingCartCount = db.Users.Where(u => u.Id == userId).Select(u => u.ShoppingCart.ShoppingCartList.Count).FirstOrDefault();
                    _cache.UserShoppingCartCounts.Add(userId, shoppingCartCount);
                }

                return shoppingCartCount;
            }
        }

        public static void UpdateCartCount(string userId, int count)
        {
            if (!_cache.UserShoppingCartCounts.ContainsKey(userId))
                return;

            _cache.UserShoppingCartCounts[userId] += count;
            HttpRuntime.Cache["ShoppingCartCount"] = _cache;
        }

        private class CartCache
        {
            public CartCache()
            {
                UserShoppingCartCounts = new Dictionary<string, int>();
            }

            public Dictionary<string, int> UserShoppingCartCounts { get; set; }
        }
    }
}
