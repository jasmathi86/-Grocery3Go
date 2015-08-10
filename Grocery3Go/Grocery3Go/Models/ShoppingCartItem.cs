using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Grocery3Go.Models
{
    public class ShoppingCartItem
    {

        public int ShoppingCartItemId { get; set; }


        public int ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")]
        public virtual ShoppingCart ShoppingCart { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }








    }
}