using Microsoft.AspNetCore.Http;
using MyChy.Frame.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Services
{
    public class CookiesServer
    {
        /// <summary>
        ///  Cookie 保存
        /// </summary>
        /// <param name="Front"></param>
        public static void Set(string Name, object Front)
        {
            var val = SerializeHelper.ObjToString(Front);
            HttpContext.Current.Response.Cookies.Append(Name, val);
        }

        /// <summary>
        /// Cookie 保存
        /// </summary>
        /// <param name="Front"></param>
        public static void Set(string Name, object Front, int Minutes=0,string Domain="",bool HttpOnly=false)
        {
            var val = SerializeHelper.ObjToString(Front);
            //  byte[] serializedResult = Encoding.UTF8.GetBytes(val);
            CookieOptions options = new CookieOptions();
            if (Minutes > 0)
            {
                options.Expires = DateTime.Now.AddMinutes(Minutes);
            }
            else
            {
                options.Expires = DateTime.Now.AddMinutes(60);
            }
            if (!string.IsNullOrEmpty(Domain))
            {
                options.Domain = Domain;
            }
            options.HttpOnly = HttpOnly;
            HttpContext.Current.Response.Cookies.Append(Name, val, options);
        }


        public static T Get<T>(string Name)
        {
            HttpContext.Current.Request.Cookies.TryGetValue(Name, out string serializedResult);
            if (!string.IsNullOrEmpty(serializedResult))
            {
                return SerializeHelper.StringToObj<T>(serializedResult);
            }
            return default(T);
        }

        public static void Remove(string Name)
        {
            HttpContext.Current.Response.Cookies.Delete(Name);

        }
    }
}
