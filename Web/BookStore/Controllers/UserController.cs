using BookStore.Models;
using BookStore.Models.DBInteractive;
using BookStore.Models.Entities;
using BookStore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        BookStoreDB db = new BookStoreDB();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult AddToCart(int idBook, int quantity)
        //{
        //    if (Session[Note.SESSION.UserInfor] == null)
        //        return RedirectToAction("Login", "Home");
        //    var book = db.GetBook(idBook);
        //    var cart = Session[Note.SESSION.Cart];
        //    //Nếu đã có giỏ hàng
        //    if (cart != null)
        //    {
        //        var list = (List<CartItem>)Session[Note.SESSION.Cart];
        //        //Nếu item đã có trong giỏ hàng
        //        if (list.Exists(x => x.book.ID == idBook))
        //        {
        //            foreach (var item in list)
        //            {
        //                if (item.book.ID == idBook)
        //                {
        //                    item.quantity += quantity;
        //                }
        //            }
        //        }
        //        //Nếu item chưa có trong giỏ hàng
        //        else
        //        {
        //            var item = new Models.CartItem();
        //            item.book = book;
        //            item.quantity = quantity;
        //            list.Add(item);
        //        }
        //        Session[Note.SESSION.Cart] = list;
        //    }
        //    //Nếu chưa có giỏ hàng
        //    else
        //    {
        //        var item = new CartItem();
        //        item.book = book;
        //        item.quantity = quantity;
        //        var list = new List<CartItem>();
        //        list.Add(item);
        //        Session[Note.SESSION.Cart] = list;
        //    }
        //    TempData[Note.TEMDATA.Message] = "Thêm vào giỏ hàng thành công";
        //    return RedirectToAction("Detail", "Home", new { id = idBook });
        //}

        [HttpPost]
        public JsonResult AddToCart(int id, int quantity)
        {
            //if (Session[Note.SESSION.UserInfor] == null)
            //    return RedirectToAction("Login", "Home");
            var book = db.GetBook(id);
            var cart = Session[Note.SESSION.Cart];
            //Nếu đã có giỏ hàng
            if (cart != null)
            {
                var list = (List<CartItem>)Session[Note.SESSION.Cart];
                //Nếu item đã có trong giỏ hàng
                if (list.Exists(x => x.book.ID == id))
                {
                    foreach (var item in list)
                    {
                        if (item.book.ID == id)
                        {
                            item.quantity += quantity;
                        }
                    }
                }
                //Nếu item chưa có trong giỏ hàng
                else
                {
                    var item = new Models.CartItem();
                    item.book = book;
                    item.quantity = quantity;
                    list.Add(item);
                }
                Session[Note.SESSION.Cart] = list;
            }
            //Nếu chưa có giỏ hàng
            else
            {
                var item = new CartItem();
                item.book = book;
                item.quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[Note.SESSION.Cart] = list;
            }
            //TempData[Note.TEMDATA.Message] = "Thêm vào giỏ hàng thành công";
            return Json(new { status = 1 });        
            //return RedirectToAction("Detail", "Home", new { id = idBook });
        }

        public ActionResult ListCart()
        {
            var cart = Session[Note.SESSION.Cart];
            var list = new List<Models.CartItem>();
            if (cart != null)
                list = (List<Models.CartItem>)Session[Note.SESSION.Cart];
            return View(list);
        }

        [HttpPost]
        public JsonResult ChangeQuantity(int id, int quantity)
        {
            var list = (List<CartItem>)Session[Note.SESSION.Cart];
            foreach (var item in list)
            {
                if(item.book.ID == id)
                {
                    item.quantity = quantity ;
                    break;
                }
            }
            Session[Note.SESSION.Cart] = list;
            return Json(new { status = 1 });
        }

        public ActionResult DeleteCartItem(int id)
        {
            var list = (List<CartItem>)Session[Note.SESSION.Cart];
            foreach(var item in list)
            {
                if(item.book.ID == id)
                {
                    list.Remove(item);
                    break;
                }
            }
            Session[Note.SESSION.Cart] = list;
            return RedirectToAction("ListCart", "User");
        }

    }
}