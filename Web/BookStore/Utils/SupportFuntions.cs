using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BookStore.Utils
{
    public class SupportFuntions
    {
        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[]
            {
                "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ",
                "í", "ì", "ỉ", "ĩ", "ị",
                "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ",
                "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự",
                "ý", "ỳ", "ỷ", "ỹ", "ỵ",
            };
            string[] arr2 = new string[]
            {
                "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e",
                "i", "i", "i", "i", "i",
                "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o",
                "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u",
                "y", "y", "y", "y", "y",
            };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i]);
            }

            return text.ToLower().Trim();
        }

        public static int[] getNumberPage(int presentPage, int maxPage)
        {
            var list = new int[] { };

            if (maxPage - presentPage <= 2 && maxPage >= 6)
                list = new int[] { maxPage - 4, maxPage - 3, maxPage - 2, maxPage - 1, maxPage };
            else if (presentPage > 4 && maxPage >= 7)
                list = new int[] { presentPage - 2, presentPage - 1, presentPage, presentPage + 1, presentPage + 2 };
            else
                list = new int[] { 1, 2, 3, 4, 5 };

            if(maxPage < 5 )
            {
                List<int> temp = new List<int>(list);
                temp.RemoveRange(maxPage, 5 - maxPage);
                list = temp.ToArray();
            }

            return list;
        }

        public static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }

            return hash;
        }
    }
}