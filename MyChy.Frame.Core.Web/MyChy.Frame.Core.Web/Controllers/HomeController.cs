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
using MyChy.Frame.Core.EFCore;
using System.Data.SqlClient;
using MyChy.Frame.Core.EFCore.AutoHistorys;

namespace MyChy.Frame.Core.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICompetencesWorkArea _competencesService;
        private readonly IBaseUnitOfWork baseUnitOfWork;
        private readonly ILogger _logger;

        public HomeController(
            ICompetencesWorkArea competencesService,
            ILoggerFactory loggerFactory,
            IBaseUnitOfWork _baseUnitOfWork)
        {
            _competencesService = competencesService;
            _logger = loggerFactory.CreateLogger<HomeController>();
            baseUnitOfWork = _baseUnitOfWork;

        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            LogHelper.LogError("asdfasdf");

            var model = _competencesService.CompUserR.GetById(1);
            if (model?.Id > 0)
            {
                var m = _competencesService.CompUserRoleR.GetById(1);
                if (m?.Id > 0)
                {
                    m.UserId = 1;
                    m.RoleId = m.RoleId + 2;
                    _competencesService.CompUserRoleR.Update(m);
                }
                else
                {
                    m = new  CompUserRole
                    {
                        UserId=1,
                        RoleId=1,
                    };

                    _competencesService.CompUserRoleR.Add(m);
                    //_competencesService.CompUserRoleR.Context.SaveChanges();
                }

                //  _competencesService.CompUserR.Context.SaveChanges();
                var models1 = _competencesService.CompUserR.GetById(3);

                var models = _competencesService.CompUserR.GetById(2);
                models.NickName = DateTime.Now.Ticks.ToString();
                models.UpdatedBy = "234";
                models.UpdatedOn = DateTime.Now;
                _competencesService.CompUserR.Update(model);


                model.NickName = DateTime.Now.Ticks.ToString();
                model.UpdatedBy = "123";
                model.UpdatedOn = DateTime.Now;
                _competencesService.CompUserR.Update(model);

                _competencesService.CompUserR.Context.EnsureAutoHistory("MyChy");

                _competencesService.CompUserR.Context.SaveChanges();

                //_competencesService.CompUserRoleR.Context.SaveChanges();

            }
            else
            {
                model = new CompUser
                {
                    NickName = DateTime.Now.Ticks.ToString(),
                    PassWord = "123",
                    UserName = "123",
                    CreatedBy = "123",
                    CreatedOn = DateTime.Now
                };

                _competencesService.CompUserR.Add(model);
            }


            var predicate = PredicateBuilder.New<CompUser>();

            //var x1 = baseUnitOfWork.Context.Database.ExecuteSqlCommand("update [CompUser] set [NickName]=@name where id=@id",
            //new SqlParameter[] {
            //      new SqlParameter("@name","1234"),
            //      new SqlParameter("@id",1),
            //});

            var xlist = baseUnitOfWork.Context.Set<CompUser>().AsTracking().FromSql("select * from [CompUser] where id<@id", new SqlParameter[] {
                  new SqlParameter("@id",10),
            }).ToList();





            var id = 5;
            xlist = baseUnitOfWork.Context.Set<CompUser>().AsTracking().FromSql($"select * from [CompUser] where id<{id}").ToList();

            // var city = "Redmond";
            //  context.Customers.FromSql($"SELECT * FROM Customers WHERE City = {city}");


            var xlist2 = baseUnitOfWork.Context.Set<CompUser>().Select(x => new CompUserOther()
            {
                NickName = x.NickName,
                UserName = x.UserName,

            }).FromSql("select NickName,UserName from [CompUser] where id<@id", new SqlParameter[] {
                  new SqlParameter("@id",10),
            }).ToList();


            //  baseUnitOfWork.Context.Set<CompUser>

            var ss = predicate.Body.ToString();

            predicate.And(x => x.State == 0);

            ss = predicate.Body.ToString();

            var list = _competencesService.CompUserR.QueryPage(predicate, page: 1, pageSize: 10);

            list = _competencesService.CompUserR.QueryPage(predicate, page: 2, pageSize: 10);

            var comp = new CompUser
            {
                NickName = "123",
                PassWord = "123",
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };
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
