using BookStore.Models.DBInteractive;
using BookStore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        BookStoreDB db = new BookStoreDB();
        public ActionResult Index()
        {
            return View(db.GetHomePage());
        }

        public ActionResult List(string text = "", int page = 1)
        {
            ViewBag.TextSearch = text;
            ViewBag.PresentPage = page;
            ListBook list = db.GetListBook(text, page);
            ViewBag.Count = list.books.Count();
            ViewBag.ListPage = SupportFuntions.getNumberPage(page, list.page);
            ViewBag.MaxPage = list.page;
            return View(list.books);
        }
        public ActionResult Detail(int id)
        {
            ViewBag.RecommendBook = db.GetBookRecommned();

            return View(db.GetDetailBook(id));
        }
    }
}