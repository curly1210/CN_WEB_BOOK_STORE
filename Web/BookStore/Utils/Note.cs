using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Utils
{
    public class Note
    {
        public class TEMDATA
        {
            public static string Message = "Message";
            public static string RequireLogin = "RequireLogin";
        }

        public class SESSION
        {
            public static string UserInfor = "UserInfor";
            public static string Cart = "Cart";
            public static string CheckOutSuccess = "CheckOutSuccess";
        }

        public static int id_Admin = 4;
    }
}