using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Grocery3Go.Models.Views
{
    public class ProductsViewModel
    {
        //View Model - Data transfer Object (container for the view)
        public IList<Product> Products { get; set; }

        //Vessel for Shopping Cart List 
        public ICollection<ShoppingCartItem> ShoppingCartList { get; set; }
        public ApplicationUser User { get; set; }
    }
}