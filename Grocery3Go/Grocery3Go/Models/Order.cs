using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Grocery3Go.Models
{
    public class Order
    {
        public int OrderId { get; set; }


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrderItems> OrderItems { get; set; }

    }
}