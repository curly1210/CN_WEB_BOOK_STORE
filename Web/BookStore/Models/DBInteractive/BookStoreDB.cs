using BookStore.Models.Entities;
using BookStore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.DBInteractive
{
    public class ListBook
    {
        public int page { get; set; }
        public Category cate { get; set; }
        public List<Book> books { get; set; }
        public ListBook(int page, List<Book> books)
        {
            this.page = page;
            this.books = books;
        }

    }

    public class HowMuch
    {
        public int ID { get; set; }
        public string NameCost { get; set; }

        public HowMuch(int ID, string NameCost)
        {
            this.ID = ID;
            this.NameCost = NameCost;
        }
    }

    public class BookStoreDB
    {
        BookStoreDBContext StoreDB;

        public BookStoreDB()
        {
            StoreDB = new BookStoreDBContext();
        }


        //Lấy 8 quyển sách theo thể loại để hiển thị lên menu
        public List<ListBook> GetHomePage()
        {
            int pageSize = 8;
            var listItem = StoreDB.Books.Where(x => x.isHidden != 1);
            List<ListBook> list = new List<ListBook>();
            foreach (var cate in StoreDB.Categories.ToList())
            {
                ListBook listBook = new ListBook(0, listItem.Where(x => x.idCategory == cate.ID).OrderBy(x => x.ID).Take(pageSize).ToList());
                listBook.cate = cate;
                list.Add(listBook);
            }
            return list;
        }

        public ListBook GetListBook(string text = "", int page = 1, int cate = 0, int type = 0, int language = 0, int cost = 0)
        {
            int pageSize = 12;
            var removeUnicode = SupportFuntions.RemoveUnicode(text);
            var listBook = StoreDB.Books.Where(x => x.isHidden != 1);
            listBook = listBook.Where(m => m.KeySearch.Contains(removeUnicode));

            if (cate != 0)
                listBook = listBook.Where(m => m.idCategory == cate);

            //if (publisher != -1)
            //    listBook = listBook.Where(m => m.idPublisher == publisher);

            if (type != 0)
                listBook = listBook.Where(m => m.idType == type);

            if (language != 0)
                listBook = listBook.Where(m => m.idLanguage == language);

            if (cost == 1)
            {
                listBook = listBook.Where(m => m.Price < 100000);
            }
            else if (cost == 2)
            {
                listBook = listBook.Where(m => m.Price >= 100000 && m.Price < 200000);
            }
            else if (cost == 3)
            {
                listBook = listBook.Where(m => m.Price >= 200000 && m.Price < 300000);
            }
            else if (cost == 4)
            {
                listBook = listBook.Where(m => m.Price >= 300000 && m.Price < 500000);
            }
            else if (cost == 5)
            {
                listBook = listBook.Where(m => m.Price >= 500000 && m.Price < 1000000);
            }
            else if (cost == 6)
            {
                listBook = listBook.Where(m => m.Price >= 1000000);
            }

            int maxPage;
            int countItem = listBook.Count();

            if (countItem / 12 == 0 && countItem == 0)
                maxPage = 0;
            else if (countItem / 12 == 0 && countItem != 0)
                maxPage = 1;
            else if (countItem % 12 == 0)
                maxPage = countItem / 12;
            else
                maxPage = (countItem / 12) + 1;

            listBook = listBook.OrderBy(x => x.ID).Skip((page - 1) * pageSize).Take(pageSize);

            ListBook list = new ListBook(maxPage, listBook.ToList());

            return list;
        }

        //Lấy 5 quyển sách để hiển thị sách đề xuất ở trang xem sách chi tiết của khách hàng (Detail)
        public List<Book> GetBookRecommned()
        {
            int pageSize = 5;
            List<Book> list = StoreDB.Books.Where(x => x.isHidden != 1).OrderBy(x => x.ID).Take(pageSize).ToList();
            for (int i = 0; i < pageSize; i++)
            {
                list[i].Page = list[i].Images.First().Url;
            }
            return list;
        }

        //Lấy 1 quyển sách, để hiển thị Detail quyển sách đó
        public Book GetDetailBook(int id)
        {
            var book = StoreDB.Books.Where(x => x.ID == id).First();
            return book;
        }

        //Lấy danh sách các thể loại sách
        public List<Category> GetCategories()
        {
            return StoreDB.Categories.ToList();
        }

        //Lấy danh sách hình thức 
        public List<Entities.Type> GetTypes()
        {
            return StoreDB.Types.ToList();
        }

        //Lấy danh sách ngôn ngữ
        public List<Language> GetLanguages()
        {
            return StoreDB.Languages.ToList();
        }

        //Lấy danh sách NXB
        public List<Publisher> GetPublishers()
        {
            return StoreDB.Publishers.ToList();
        }

        //Lấy khoảng giá
        public List<HowMuch> GetCost()
        {
            HowMuch howMuch1 = new HowMuch(1, "Giá dưới 100.000đ");
            HowMuch howMuch2 = new HowMuch(2, "100.000đ - 200.000đ");
            HowMuch howMuch3 = new HowMuch(3, "200.000đ - 300.000đ");
            HowMuch howMuch4 = new HowMuch(4, "300.000đ - 500.000đ");
            HowMuch howMuch5 = new HowMuch(5, "500.000đ - 1.000.000đ");
            HowMuch howMuch6 = new HowMuch(6, "Giá trên 1.000.000đ");

            List<HowMuch> list = new List<HowMuch>();
            list.Add(howMuch1);
            list.Add(howMuch2);
            list.Add(howMuch3);
            list.Add(howMuch4);
            list.Add(howMuch5);
            list.Add(howMuch6);

            return list;
        }

        public User Login(string phone, string password)
        {
            string hashPass = SupportFuntions.sha256(password);
            var user = StoreDB.Users.Where(x => x.Phone == phone && x.Password == hashPass);
            if (user.Count() >= 1)
                return user.First();
            return null;
        }

        public Book GetBook(int id)
        {
            return StoreDB.Books.Find(id);
        }

        public List<Address> GetListAdressByIdUser(int idUser)
        {
            List<Address> list = new List<Address>();
            list = StoreDB.Addresses.Where(x => x.idUser == idUser).ToList();
            return list;
        }

        public List<CartItem> AddCart(List<CartItem> list, int id, int quantity, int checkBuyNow)
        {
            var book = StoreDB.Books.Find(id);
            if (list != null)
            {
                if (list.Exists(x => x.book.ID == id))
                {
                    foreach (var item in list)
                    {
                        if (item.book.ID == id)
                        {
                            if (checkBuyNow == 0)
                            {
                                item.quantity += quantity;
                            }
                            else
                            {
                                item.quantity = 1;
                            }
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
            }
            else
            {
                var item = new CartItem();
                item.book = book;
                item.quantity = quantity;
                list = new List<CartItem>();
                list.Add(item);
            }
            return list;
        }

        public void CheckOut(List<CartItem> listCart, int idUser, int idAddress, string fullName, string address, string phone, string shipper, string message, int shipFee, int totalPrice)
        {
            Order order = new Order();
            order.idUser = idUser;
            order.CreateDate = DateTime.Now;
            if (idAddress != -1)
            {
                string HadName = StoreDB.Addresses.Where(x => x.ID == idAddress).First().Name;
                order.ClientName = HadName;
                order.IDAddress = idAddress;
            }
            else
            {
                Address newAddress = new Address();
                newAddress.Name = fullName;
                newAddress.Phone = phone;
                newAddress.Address1 = address;
                newAddress.idUser = idUser;
                StoreDB.Addresses.Add(newAddress);
                StoreDB.SaveChanges();
                order.IDAddress = newAddress.ID;
                order.ClientName = fullName;
            }
            order.TotalPrice = totalPrice;
            order.OrderStatus = 0;
            order.Shipper = shipper;
            order.Notes = message;
            order.ShipFee = shipFee;
            StoreDB.Orders.Add(order);
            StoreDB.SaveChanges();


            int idOrder = StoreDB.Orders.OrderByDescending(x => x.ID).First().ID;
            foreach (var item in listCart)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.idOrder = idOrder;
                orderDetail.idBook = item.book.ID;
                orderDetail.Quantity = item.quantity;
                StoreDB.OrderDetails.Add(orderDetail);
            }
            StoreDB.SaveChanges();
        }

        public List<Order> GetListOderByIdUser(int idUser)
        {
            var listOrder = StoreDB.Orders.Where(x => x.idUser == idUser).OrderByDescending(x => x.ID).ToList();
            return listOrder;
        }

        public Order GetOrderByIdOrder(int idOrder)
        {
            var order = StoreDB.Orders.Where(x => x.ID == idOrder).First();
            return order;
        }

        public List<OrderDetail> GetListOrderDetailByIdOrder(int idOrder)
        {
            return StoreDB.OrderDetails.Where(x => x.idOrder == idOrder).ToList();
        }

        public Address GetAddressOrderByIdUser(int idUser, int idAddress)
        {
            return StoreDB.Addresses.Where(x => x.idUser == idUser).Where(x => x.ID == idAddress).First();
        }

        public User SignUp(string phone, string password, string fullName, string email = "", string gender = "", string birthday = "")
        {
            if (StoreDB.Users.Where(x => x.Phone == phone).Count() > 0)
                return null;
            User user = new User();
            user.Phone = phone;
            user.Password = SupportFuntions.sha256(password);
            user.Fullname = fullName;
            user.Email = email;
            user.Gender = gender;
            if (birthday.Length == 0)
                user.Birthday = DateTime.Now.ToShortDateString();
            else
                user.Birthday = birthday;
            StoreDB.Users.Add(user);
            StoreDB.SaveChanges();
            return user;
        }

        public User UpdateInfoUser(string phone, string fullName, string birthday, string email, string gender )
        {
            User user = StoreDB.Users.Where(x => x.Phone == phone).First();
            if (user == null)
                return null;
            user.Fullname = fullName;
            user.Email = email;
            user.Gender = gender;
            if (birthday.Length == 0)
                user.Birthday = DateTime.Now.ToShortDateString();
            else
                user.Birthday = birthday;
            
            StoreDB.SaveChanges();
            return user;

        }
    }
}