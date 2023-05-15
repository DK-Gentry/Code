using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.Md5Module
{
    public static class Md5Helper
    {
        /// <summary>
        /// 对string的扩展方法，
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.Default.GetBytes(str+"@HolyDK"));
            var md5Str = BitConverter.ToString(bytes).Replace("-","");
            return md5Str;
        }
    }
}
