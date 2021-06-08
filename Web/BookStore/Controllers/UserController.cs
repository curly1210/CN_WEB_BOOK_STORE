﻿using BookStore.Models;
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



        [HttpPost]
        public JsonResult AddToCart(int id, int quantity)
        {
            var list = (List<CartItem>)Session[Note.SESSION.Cart];
            int checkBuyNow = 0;
            list = db.AddCart(list, id, quantity,checkBuyNow);
            Session[Note.SESSION.Cart] = list;
            return Json(new { status = 1 });
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
                if (item.book.ID == id)
                {
                    item.quantity = quantity;
                    break;
                }
            }
            Session[Note.SESSION.Cart] = list;
            return Json(new { status = 1 });
        }

        public ActionResult DeleteCartItem(int id)
        {
            var list = (List<CartItem>)Session[Note.SESSION.Cart];
            foreach (var item in list)
            {
                if (item.book.ID == id)
                {
                    list.Remove(item);
                    break;
                }
            }
            Session[Note.SESSION.Cart] = list;
            return RedirectToAction("ListCart", "User");
        }

        public ActionResult Payment(int id = -1)
        {
            User user = (User)Session[Note.SESSION.UserInfor];
            var list = (List<CartItem>)Session[Note.SESSION.Cart];
            int checkBuyNow = 1;
            if (id != -1)
            {
                list = db.AddCart(list, id, 1,checkBuyNow);
                Session[Note.SESSION.Cart] = list;
            }

            ViewBag.ListAdress = db.GetListAdressByIdUser(user.ID);
            return View(list);
        }

        [HttpPost]
        public ActionResult CheckOut(int idAddress, string fullName, string address, string phone, string shipper, string message, int shipFee, int totalPrice)
        {
            return RedirectToAction("Index", "Home");
        }

    }
}