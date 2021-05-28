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

        public ActionResult List(string text = "", int page = 1, int cate = 0, int type = 0, int language = 0, int cost=0)
        {
            ViewBag.ListCate = db.GetCategories();
            ViewBag.ListCost = db.GetCost();
            //ViewBag.ListPublisher = db.GetPublishers();
            ViewBag.ListType = db.GetTypes();
            ViewBag.ListLanguage = db.GetLanguages();

            ViewBag.TextSearch = text;
            ViewBag.PresentPage = page;
            ViewBag.Cate = cate;
            ViewBag.Cost = cost;
            //ViewBag.Publisher = publisher;
            ViewBag.Type = type;
            ViewBag.Language = language;

            ListBook list = db.GetListBook(text, page, cate, type,language,cost);
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