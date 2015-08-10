using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grocery3Go.Models.Views
{
    public class MyOrderViewModel
    {
        public ICollection<ShoppingCartItem> ShoppingCartList { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}