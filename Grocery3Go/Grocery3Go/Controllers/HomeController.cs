using Grocery3Go.Models;
using System.Net;
using System.Net.Mail;
using Grocery3Go.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;



namespace Grocery3Go.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Groceries
           [Authorize]
        public ActionResult Index()
        {
            var vm = new ProductsViewModel
            {
                Products = _db.Products.ToList(),

                ShoppingCartList = _db.Users.Where(m => m.UserName == User.Identity.Name).Include(m => m.ShoppingCart.ShoppingCartList).FirstOrDefault().ShoppingCart.ShoppingCartList,
                User = _db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault()

            };

            return View(vm);
        }

           public ActionResult MyCart()
           {
               ProductsViewModel pVm = new ProductsViewModel()
               {
                   ShoppingCartList = _db.Users.Where(m => m.UserName == User.Identity.Name).Include(m => m.ShoppingCart.ShoppingCartList).FirstOrDefault().ShoppingCart.ShoppingCartList,
                   User = _db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault()
               };
               return View(pVm);
           }

        [Authorize]
        public ActionResult ShoppingCart()
        {
            return View();
        }


        public ActionResult AddToCart(int productId)
        {

            var user = _db.Users.Where(m => m.UserName == User.Identity.Name).Include(m => m.ShoppingCart.ShoppingCartList).FirstOrDefault();


            user.ShoppingCart.ShoppingCartList.Add(new ShoppingCartItem
            {
                ProductId = productId,
                Quantity = 1
            });

            //ShoppingCart userCart = new ShoppingCart();
            //if (userCart == null)
            //{
            //    userCart.UserId = _db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Id;


            //}
            //else
            //{
            //    userCart = _db.ShoppingCarts.FirstOrDefault();
            //}

            _db.SaveChanges();

            CartCacheService.UpdateCartCount(user.Id, 1);

            return Redirect("Index");

            //return View();
        }

        // GET: Groceries/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Groceries/Create Order
        public ActionResult CreateOrder(int shoppingCartId)
        {
            var user = _db.Users.Where(m => m.UserName == User.Identity.Name).Include(m => m.ShoppingCart).FirstOrDefault();
            //var sCId = user.ShoppingCart.ShoppingCartId;
            var aBC = user.ShoppingCart.ShoppingCartList;
            var sC1 = _db.Users.FirstOrDefault(m => m.ShoppingCartId == shoppingCartId);
            var user1 = _db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Id;

            var order1 = _db.Orders.Where(m => m.UserId == user1).FirstOrDefault();

            _db.Orders.Add(new Order
            {
                UserId = user1

            });
            _db.SaveChanges();
            _db.OrderItems.Add(new OrderItems
            {
                OrderId = order1.OrderId,
                ShoppingCartList = aBC
                

            });
            _db.SaveChanges();
            return Redirect("MyOrder");
        }

        public ActionResult MyOrder()
        {

            //var user = _db.Users.Where(m => m.UserName == User.Identity.Name).Include(m => m.ShoppingCart).FirstOrDefault();
            ////var sCId = user.ShoppingCart.ShoppingCartId;
            //var aBC = user.ShoppingCart.ShoppingCartList;
            /////////////////////////////////////////////////////
            var user = _db.Users.Where(m => m.UserName == User.Identity.Name).Include(m => m.ShoppingCart.ShoppingCartList).FirstOrDefault();
            var user1 = _db.Users.Where(m => m.UserName == User.Identity.Name).FirstOrDefault().Id;

            var order1 = _db.Orders.Where(m => m.UserId == user1).FirstOrDefault();

            var orderItem1 = _db.OrderItems.Where(m => m.OrderId == order1.OrderId).Include(m => m.ShoppingCartList).FirstOrDefault();
            decimal total = 0.00m;
            var Ttotal = new decimal();
            foreach (var item in user.ShoppingCart.ShoppingCartList)
	{
            Ttotal += item.Product.Price;
	};

            var vm = new MyOrderViewModel
            {
                ShoppingCartList = user.ShoppingCart.ShoppingCartList,
                Total = Ttotal
            };
            return View(vm);
        }
        public ActionResult AboutUs() 
        
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactUs(EmailFormModel model)
        {
            if (ModelState.IsValid) 
            {
                model.ToEmail = "jasmathi86@gmail.com";
                model.FromEmail = "support@something.com";
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>";
                var message = new MailMessage();
                message.Bcc.Add(new MailAddress("one@gmail.com"));
                message.Bcc.Add(new MailAddress("two@gmail.com"));
                message.Bcc.Add(new MailAddress("three@gmail.com"));
                //message.Attachments.Add(new Attachment(HttpContext.Server.MapPath("~/App_Data/Test.docx")));
                message.To.Add(new MailAddress(model.ToEmail));
                message.From = new MailAddress(model.FromEmail);
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;
                if (model.Upload != null && model.Upload.ContentLength > 0)
                {
                    message.Attachments.Add(new Attachment(model.Upload.InputStream, Path.GetFileName(model.Upload.FileName)));
                }

                using (var smtp = new SmtpClient())
                {
                    
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
        }
    }
    return View(model);
}
        public ActionResult Sent()
        {
            return View();
        }

        // GET: Groceries/Delete/5
        public ActionResult DeleteFromCart(int shoppingCartItemid)
        {
            //Removing item from shopping cart list(local) (wasn't reflected in the database)
            //Removing item from database
            var user = _db.Users.Where(m => m.UserName == User.Identity.Name).Include(m => m.ShoppingCart.ShoppingCartList).FirstOrDefault();
            var sCl = user.ShoppingCart.ShoppingCartList;
            var sC2 = sCl.Where(m => m.ShoppingCartItemId == shoppingCartItemid).FirstOrDefault();
            sCl.Remove(sC2);
            var dBI = _db.ShoppingCartItems.Where(m => m.ShoppingCartItemId == shoppingCartItemid).FirstOrDefault();
            _db.ShoppingCartItems.Remove(dBI);

            _db.SaveChanges();

            CartCacheService.UpdateCartCount(user.Id, -1);

            return Redirect("Index");

        }
        

        // GET: Groceries/Edit/5
        public ActionResult EditFromCart(int shoppingCartItemid)


        {
            return View();
        }

        // POST: Groceries/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Home()
        {
            return View();
        }
        
    }
}