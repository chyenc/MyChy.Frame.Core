using MyChy.Frame.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Services
{
    public class SessionServer
    {
        /// <summary>
        /// Session 保存
        /// </summary>
        /// <param name="Front"></param>
        public static void Set(string Name,object Front)
        {
            if (HttpContext.Current!=null&& HttpContext.Current.Session != null)
            {
                var val = SerializeHelper.ObjToString(Front);
                byte[] serializedResult = Encoding.UTF8.GetBytes(val);
                HttpContext.Current.Session.Set(Name, serializedResult);
            }

        }

        ///// <summary>
        ///// Session 保存
        ///// </summary>
        ///// <param name="Front"></param>
        //public static void Set(object Front, string Name, int Minutes)
        //{
        //    var val = SerializeHelper.ObjToString(Front);
        //    byte[] serializedResult = Encoding.UTF8.GetBytes(val);
        //    HttpContext.Current.Session.Set(Name, serializedResult);
        //}


        public static T Get<T>(string Name)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.TryGetValue(Name, out byte[] serializedResult);
                if (serializedResult != null && serializedResult.Length > 0)
                {
                    var str = Encoding.UTF8.GetString(serializedResult);
                    return SerializeHelper.StringToObj<T>(str);
                }
            }
            return default(T);
        }

        public static void Remove(string Name)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Remove(Name);
            }
        }
    }
}
