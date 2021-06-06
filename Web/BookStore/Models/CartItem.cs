using BookStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class CartItem
    {
        public Book book { get; set; }
        public int quantity { get; set; }
    }
}