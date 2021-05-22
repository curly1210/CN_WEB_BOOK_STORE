using BookStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models.DBInteractive
{
    public class ListBook
    {
        public int pages { get; set; }
        public Category cate { get; set; }
        public List<Book> books { get; set; }
        public ListBook(int pages, List<Book> books)
        {
            this.pages = pages;
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

        public List<ListBook> GetHomePage()
        {
            int pageSize = 8;
            var listItem = StoreDB.Books.Where(x => x.isHidden != 1);
            List<ListBook> list = new List<ListBook>();
            foreach(var cate in StoreDB.Categories.ToList())
            {
                ListBook listBook = new ListBook(0, listItem.Where(x => x.idCategory == cate.ID).OrderBy(x => x.ID).Take(pageSize).ToList());
                listBook.cate = cate;
                list.Add(listBook);
            }
            return list;
        }
    }
}