using BookStore.Models.DBInteractive;
using BookStore.Models.Entities;
using BookStore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        BookStoreDB db = new BookStoreDB();
        public ActionResult Index()
        {
            if (Session[Note.SESSION.CheckOutSuccess] != null)
            {
                TempData[Note.TEMDATA.Message] = "Đặt hàng thành công";
                Session[Note.SESSION.CheckOutSuccess] = null;
            }
            return View(db.GetHomePage());
        }

        public ActionResult List(string text = "", int page = 1, int cate = 0, int type = 0, int language = 0, int cost = 0)
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

            ListBook list = db.GetListBook(text, page, cate, type, language, cost);
            ViewBag.ListPage = SupportFuntions.getNumberPage(page, list.page);
            ViewBag.MaxPage = list.page;

            return View(list.books);
        }
        public ActionResult Detail(int id)
        {
            ViewBag.RecommendBook = db.GetBookRecommned();

            return View(db.GetDetailBook(id));
        }

        public ActionResult Login(string returnUrl = "")
        {
            if (Session[Note.SESSION.UserInfor] != null)
                return RedirectToAction("Index", "Home");
            FormsAuthentication.SignOut();
            //ViewBag.TotalCart = 0;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string phone, string password, string returnUrl)
        {
            User user = db.Login(phone, password);
            if (user != null)
            {
                if (user.is_ban == 1)
                {
                    TempData[Note.TEMDATA.Message] = "Tài khoản của bạn bị khóa, vui lòng liên hệ để biết thêm chi tiết";
                    return RedirectToAction("Login", "Home", new { returnUrl });
                }
                FormsAuthentication.SetAuthCookie("" + user.ID, true);
                Session[Note.SESSION.UserInfor] = user;

            }
            else
            {
                TempData[Note.TEMDATA.Message] = "Sai tài khoản hoặc mật khẩu";
                return RedirectToAction("Login", "Home", new { returnUrl });
            }

            if (user.ID.Equals(Note.id_Admin))
                return RedirectToAction("Index", "Admin");
            if (returnUrl.Equals(""))
                return RedirectToAction("Index", "Home");

            return Redirect(returnUrl);
        }

        public ActionResult Logout()
        {
            Session[Note.SESSION.UserInfor] = null;
            Session[Note.SESSION.Cart] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignUp(string returnUrl, string phone="", string fullName="", string email="", string gender="", string birthday="")
        {
            ViewBag.Phone = phone;
            ViewBag.FullName = fullName;
            ViewBag.Email = email;
            ViewBag.Gender = gender;
            ViewBag.Birthday = birthday;
            if (Session[Note.SESSION.UserInfor] != null)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string phone, string password, string confirmPassword, string fullName, string email, string gender, string birthday, string returnUrl)
        {
            
            if (!password.Equals(confirmPassword))
            {
                TempData[Note.TEMDATA.Message] = "Xác nhận mật khẩu không đúng";
                TempData["SignUp"] = true;
                return RedirectToAction("SignUp", "Home", new { returnUrl,phone,fullName,email,gender,birthday});
            }

            User user = db.SignUp(phone, password, fullName, email, gender, birthday);
            if (user != null)
            {
                Session[Note.SESSION.UserInfor] = user;
            }
            else
            {
                TempData[Note.TEMDATA.Message] = "Số điện thoại đã đăng kí";
                TempData["SignUp"] = true;
                return RedirectToAction("SignUp", "Home", new { returnUrl });
            }

            if (returnUrl.Equals(""))
                return RedirectToAction("Index", "Home");

            return Redirect(returnUrl);
        }
    }
}