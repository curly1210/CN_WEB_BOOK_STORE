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

        public ListBook GetListBook(string text = "", int page = 1, int cate = 0, int type = 0, int language = 0, int publisher = -1)
        {
            int pageSize = 12;
            var removeUnicode = SupportFuntions.RemoveUnicode(text);
            var listBook = StoreDB.Books.Where(x => x.isHidden != 1);
            listBook = listBook.Where(m => m.KeySearch.Contains(removeUnicode));

            if (cate != 0)
                listBook = listBook.Where(m => m.idCategory == cate);

            if (publisher != -1)
                listBook = listBook.Where(m => m.idPublisher == publisher);

            if (type != 0)
                listBook = listBook.Where(m => m.idType == type);

            if (language != 0)
                listBook = listBook.Where(m => m.idLanguage == language);

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
    }
}