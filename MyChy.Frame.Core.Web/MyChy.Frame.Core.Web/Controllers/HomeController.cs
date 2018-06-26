using System;
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
using MyChy.Frame.Core.EFCore;
using System.Data.SqlClient;
using MyChy.Frame.Core.EFCore.AutoHistorys;
using MyChy.Frame.Core.Common.Extensions;
using MyChy.Frame.Core.Redis;
using Microsoft.Extensions.DependencyInjection;


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
            _logger.LogTrace("跟踪日志-----------");

            var _competences = MyChy.Frame.Core.HttpContext.GetService<ICompetencesWorkArea>();
            var model = _competences.CompUserR.GetById(3);


            //_logger.LogDebug("调试日志-----------");
            //_logger.LogInformation("普通信息日志-----------");


            //_logger.LogWarning("警告日志-----------");
            //_logger.LogError("错误日志-----------");
            //_logger.LogCritical("系统崩溃或灾难性-----------");



            //LogHelper.LogError("跟踪日志-----------2");
            //LogHelper.LogInfo("跟踪日志-----------3");
            // LogHelper.LogError("跟踪日志-----------2");
            //var url = Request.GetAbsoluteUri();

            //url = "http://material.huiyuanjuice.cn:80/Product/MobileTicket";
            //url = url.Replace(":80/", "/");


            return View();
        }

        public IActionResult About()
        {
            LogHelper.LogError("asdfasdf");

           // Common();

           // SqlData();

            // CookiesSession();

            //Redis();



            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<IActionResult> Contact()
        {
            await Common();

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

            //var info = ClaimsIdentityServer.AccountUserid();

            return Json(userinfo);
        }

        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Common
        /// </summary>
        public async Task<int> Common()
        {
            // _competencesService.CompUserR.Set.
            //  var model = _competencesService.CompUserR.q
            var model = _competencesService.CompUserR.GetById(3);
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
                    m = new CompUserRole
                    {
                        UserId = 1,
                        RoleId = 1,
                    };

                    _competencesService.CompUserRoleR.Add(m);
                    //_competencesService.CompUserRoleR.Context.SaveChanges();
                }

                var models1 = _competencesService.CompUserR.GetById(3);

                var models = _competencesService.CompUserR.GetById(2);
                models.NickName = DateTime.Now.Ticks.ToString();
                models.UpdatedBy = "234";
                models.UpdatedOn = DateTime.Now;
                _competencesService.CompUserR.Update(model);


                _competencesService.CompUserRoleR.CommitAutoHistory();
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

                await _competencesService.CompUserR.AddAsync(model);
                await _competencesService.CompUserR.CommitAutoHistoryAsync();
            }

            return 1;
        }

        /// <summary>
        /// 日志DEMO
        /// </summary>
        public void AutoHistory()
        {
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
                    m = new CompUserRole
                    {
                        UserId = 1,
                        RoleId = 1,
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
                //  _competencesService.CompUserR.c
                // _competencesService.CompUserR.c

                model.NickName = DateTime.Now.Ticks.ToString();
                model.UpdatedBy = "123";
                model.UpdatedOn = DateTime.Now;
                _competencesService.CompUserR.Update(model);
                var fullname = _competencesService.CompUserR.Context.GetType().FullName;

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
        }


        public void SqlData()
        {
            var sql = new RawSqlString(@"update [CompUser] set IsDeleted=@is,DeletedBy=@user,DeletedOn=GETDATE()
                                        where id = @id");
            var parameter = new SqlParameter[] {
                new SqlParameter("@id", 1),
                 new SqlParameter("@user", "MyChy"),
                  new SqlParameter("@is", 1),
            };
       
         
            var x1 = baseUnitOfWork.Context.Database.ExecuteSqlCommand(sql, parameter);


            //var x1 = baseUnitOfWork.Context.Database.ExecuteSqlCommand("update [CompUser] set [NickName]=@name where id=@id",
            //new SqlParameter[] {
            //      new SqlParameter("@name","1234"),
            //      new SqlParameter("@id",1),
            //});

            var xlist = baseUnitOfWork.Context.Set<CompUser>().AsTracking().FromSql("select * from [CompUser] where id<@id", new SqlParameter[] {
                  new SqlParameter("@id",10),
            }).ToList();



            var predicate = PredicateBuilder.New<CompUser>();

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



        }


        public void Redis()
        {
            // CoreDbContext
            // RedisConfig

            // MyChy.Frame.Core.Redis.

            var key = "RegistrationServer_ShowPresentcount_" + DateTime.Now.Date.ToString("yyyy-MM-dd");
            var xx1 = RedisServer.StringGetCache<long>(key);

            long xx = 0;
            RedisServer.StringSetCache("11", "asdfasdf");
            var ss = RedisServer.StringGetCache<string>("11");
            //RedisServer.Remove("11");
            // ss = RedisServer.StringGetCache<string>("11");
            var time = DateTime.Now;

            var s = RedisServer.ExistsKey("11");


            RedisServer.StringDaySetCache("21", time);
            time = RedisServer.StringGetCache<DateTime>("21");

            var ts = new CompUser
            {
                Id = 1,
                NickName = "1231",
            };
            RedisServer.StringSetCache("31", ts);
            var ts1 = RedisServer.StringGetCache<CompUser>("31");
            ts1.Id = 2;
            RedisServer.StringSetCache("31", ts1);
            var ts2 = RedisServer.StringGetCache<CompUser>("31");

            //long xx = 0;

            // xx = RedisServer.StringIncrement("asdf1", 10);
            xx = RedisServer.Increment("asdf11");
            xx = RedisServer.StringGetCache<long>("asdf11");
            xx = RedisServer.Increment("asdf11", 100);
            xx = RedisServer.Increment("asdf11");
            xx = RedisServer.StringGetCache<long>("asdf11");
            xx = RedisServer.Increment("asdf11");


            RedisSetTest();
            RedisHashTest();
            RedisSortedTest();
        }



        public void RedisSetTest()
        {
            const string key = "RedisTest_SetTest";
            long ss = 13810565157;
            string mobile = "13810565156";

            RedisServer.SetDayAddCache(key, mobile);
            var ss1 = RedisServer.SetDayContainsCache(key, mobile);
            RedisServer.SetDayDelete(key, mobile);


            ss1 = RedisServer.SetContainsCache(key, mobile);
            RedisServer.SetAddCache(key, mobile);
            ss1 = RedisServer.SetContainsCache(key, mobile);
            RedisServer.SetDelete(key, mobile);
            ss1 = RedisServer.SetContainsCache(key, mobile);

            if (!ss1)
            {
                RedisServer.SetDayAddCache(key, mobile, true);
                ss1 = RedisServer.SetDayContainsCache(key, mobile);

            }

            RedisServer.SetDayAddCache(key, mobile, true);

            for (int i = 0; i < 100000; i++)
            {
                mobile = ss.ToString();
                ss1 = RedisServer.SetDayContainsCache(key, mobile);
                if (!ss1)
                {
                    RedisServer.SetDayAddCache(key, mobile);
                }
                ss = ss + 1;
            }



        }



        public void RedisHashTest()
        {

            var key1 = "ProductServer_ShowProductStocksCount-";
            RedisServer.HashDayAddCache(key1, "123", 100);
            var ss13 = RedisServer.HashDayGetCache<long>(key1, "1231", -999);

            var ss = RedisServer.HashDayGetCache<long>(key1, "123", 0);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            RedisServer.HashDayAddCache(key1, "123", 15);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDecrementDayCache(key1, "123", 1);
            ss = RedisServer.HashDayGetCache<long>(key1, "123", 0);

            var key = "Hash";
            var ss1 = RedisServer.HashGetCache<long>(key, "1", -1);
            if (ss1 == -1)
            {
                RedisServer.HashAddCache(key, "1", 0);
            }
            ss1 = RedisServer.HashGetCache<long>(key, "1", -1);
            ss1 = RedisServer.HashIncrementCache(key, "1", 1);

            ss1 = RedisServer.HashGetCache<long>(key, "1", -1);

            ss1 = RedisServer.HashIncrementCache(key, "1", 1);

            ss1 = RedisServer.HashGetCache<long>(key, "1", -1);


        }


        public void RedisSortedTest()
        {
            string key = "SortedTest";
            for (var i = 0; i < 10; i++)
            {
                RedisServer.SortedAddCache(key, i.ToString(), i);
            }
            var list = RedisServer.SortedSetRangeByRankDescendingCache(key, 0, 5);
            var ss = RedisServer.SortedSetIncrementCache(key, "1", 100);
            ss = RedisServer.SortedSetIncrementCache(key, "2", 10);

            ss = RedisServer.SortedSetIncrementCache(key, "5", 2);

            list = RedisServer.SortedSetRangeByRankDescendingCache(key, 0, 5);
        }


        public void CookiesSession()
        {

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

            //ClaimsIdentityServer.UserLogin(UserIdentity);
            //// UserIdentity.Success = true;

            return UserIdentity;


        }
    }
}
