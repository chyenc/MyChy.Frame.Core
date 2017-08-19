using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

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
