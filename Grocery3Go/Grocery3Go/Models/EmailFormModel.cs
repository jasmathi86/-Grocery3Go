using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grocery3Go.Models
{
    public class EmailFormModel
    {
        [Required, Display(Name = "Your name")]
        public string FromName { get; set; }

        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }

        [Required]
        public string Message { get; set; }

        [Display (Name = "Your email")]
        public string ToEmail { get; set; }

        
        public string subject { get; set; }

        public HttpPostedFileBase Upload { get; set; }
    }


}