using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyChy.Frame.Core.Services;
using MyChy.Frame.Core.Common.Helper;
using MyChy.Frame.Core.Web.Work;
using Microsoft.Extensions.Logging;
using LinqKit;
using MyChy.Frame.Core.Web.Domains;

namespace MyChy.Frame.Core.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICompetencesWorkArea _competencesService;
        private readonly ILogger _logger;

        public HomeController(
            ICompetencesWorkArea competencesService,
            ILoggerFactory loggerFactory)
        {
            _competencesService = competencesService;
            _logger = loggerFactory.CreateLogger<HomeController>();

        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            var predicate = PredicateBuilder.New<CompUser>();

            //predicate.And(x => x.NickName == "1");

            var list = _competencesService.CompUserR.QueryPage(predicate, page: 1, pageSize: 10,IsNoTracking: true);

            var comp = new CompUser();
            comp.NickName = "123";
            comp.PassWord = "123";
            comp.CreatedOn = DateTime.Now;
            comp.UpdatedOn = DateTime.Now;
            var xx=_competencesService.CompUserR.AddAsync(comp);
            _competencesService.CompUserR.Context.SaveChanges();


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
