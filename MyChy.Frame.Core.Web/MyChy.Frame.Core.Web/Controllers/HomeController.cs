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
using Microsoft.EntityFrameworkCore;
using System.Text;

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

            var ss = predicate.Body.ToString();

            predicate.And(x => x.State == 0);

            ss = predicate.Body.ToString();

            var list = _competencesService.CompUserR.QueryPage(predicate, page: 1, pageSize: 10);

            list = _competencesService.CompUserR.QueryPage(predicate, page: 2, pageSize: 10);

            var comp = new CompUser();
            comp.NickName = "123";
            comp.PassWord = "123";
            comp.CreatedOn = DateTime.Now;
            comp.UpdatedOn = DateTime.Now;
            var xx = _competencesService.CompUserR.AddAsync(comp);
            _competencesService.CompUserR.Context.SaveChanges();


            var sss = MyChy.Frame.Core.HttpContext.Current;

            var sfa = SerializeHelper.ObjToString("234");


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

        public async Task<IActionResult> Contact()
        {
            //string val = "234243";

            //; byte[] serializedResult = Encoding.UTF8.GetBytes(val);
            //HttpContext.Session.Set("1", serializedResult);

            //HttpContext.Session.TryGetValue("1", out byte[] yy);
            //if (serializedResult != null && serializedResult.Length > 0)
            //{
            //    var str = Encoding.UTF8.GetString(serializedResult);
            //}

            //SessionServer.Set("1", val);
            //var ss = SessionServer.Get<string>("1");



            ViewData["Message"] = "Your contact page.";
            var userinfo = await UserIdentity(1);

            var info = ClaimsIdentityServer.AccountUserid();

            return Json(userinfo);
        }

        public IActionResult Error()
        {
            return View();
        }


        /// <summary>
        /// 增加用户登录信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<FrontIdentity> UserIdentity(int userid)
        {

            var UserIdentity = new FrontIdentity();
            var userinfo = await _competencesService.CompUserR.QueryNoTracking().Where(x => x.Id == userid).FirstOrDefaultAsync();
            if (userinfo?.Id > 0)
            {
                UserIdentity = new FrontIdentity()
                {
                    Success = true,
                    Mobile = "13810565156",
                    EndTime = DateTime.Now.AddHours(2),
                    UserId = userinfo.Id,
                    UserIds = userinfo.UserName,
                    UserNick = userinfo.NickName,
                };
                var list = await _competencesService.CompUserRoleR.QueryNoTracking().Where(x => x.UserId == userid).FirstOrDefaultAsync();
                if (list != null && list.Id > 0)
                {
                    UserIdentity.RoleId = list.RoleId;
                }

                UserIdentity.Authority = "";


                UserIdentity.OrganizationId = "1,2,3,4";

            }

            ClaimsIdentityServer.UserLogin(UserIdentity);
            // UserIdentity.Success = true;

            return UserIdentity;


        }
    }
}
