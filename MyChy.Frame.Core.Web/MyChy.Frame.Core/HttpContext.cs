using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace MyChy.Frame.Core
{
    public static class HttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }


        public static T GetService<T>()
        {
            return Current.RequestServices.GetService<T>();
        }

        public static string GetIp()
        {
            var ip = Current.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip)|| ip.Length<7)
            {
                ip = Current.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        //public static IServiceProvider ServiceProvider;

        //public static Microsoft.AspNetCore.Http.HttpContext Current
        //{
        //    get
        //    {
        //        object factory = ServiceProvider.GetService(typeof(IHttpContextAccessor));
        //        Microsoft.AspNetCore.Http.HttpContext context = ((HttpContextAccessor)factory).HttpContext;
        //        return context;
        //    }
        //}

    }
}
