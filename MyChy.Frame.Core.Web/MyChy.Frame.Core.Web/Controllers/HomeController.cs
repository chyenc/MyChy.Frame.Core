using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyChy.Frame.Core.Services;
using MyChy.Frame.Core.Common.Helper;

namespace MyChy.Frame.Core.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {

            var sss = MyChy.Frame.Core.HttpContext.Current;

          var sfa=SerializeHelper.ObjToString("234");


            //SessionServer.Set(234, "ss", 10);
            //var ss = SessionServer.Get<int>("ss");

            HttpContext.Response.Cookies.Append("1", "1235");

            HttpContext.Request.Cookies.TryGetValue("1", out string ss1);

            var s = ss1;
            var Email = CookiesServer.Get<string>("LoginName");
            CookiesServer.Set("LoginName", "admin");
            Email = CookiesServer.Get<string>("LoginName");



            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
