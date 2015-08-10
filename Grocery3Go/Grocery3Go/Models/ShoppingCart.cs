using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grocery3Go.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }


        public ICollection<ShoppingCartItem> ShoppingCartList { get; set; }



    }
}