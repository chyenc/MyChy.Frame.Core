﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyChy.Frame.Core.EFCore;
using MyChy.Frame.Core.Web.Domains;
using MyChy.Frame.Core.Web.Work;
using MyChy.Frame.Core.Redis;
using MyChy.Frame.Core.Services;
using MyChy.Frame.Core.Common.Helper;

namespace MyChy.Frame.Core.Web3.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly ICompetencesWorkArea _competencesService;
        private readonly IBaseUnitOfWork baseUnitOfWork;
        private readonly ILogger _logger;

        public IndexModel(
            ICompetencesWorkArea competencesService,
            ILoggerFactory loggerFactory,
            IBaseUnitOfWork _baseUnitOfWork)
        {
            _competencesService = competencesService;
            _logger = loggerFactory.CreateLogger<IndexModel>();
            baseUnitOfWork = _baseUnitOfWork;

        }


        public void OnGet()
        {
            _logger.LogTrace("跟踪日志-----------");

            //var _competences = Core.HttpContext.GetService<ICompetencesWorkArea>();
            var model = _competencesService.CompUserR.GetById(3);

           // SqlData();

            Redis();

            CookiesSession();

            var userinfo = UserIdentity(1).Result;

            userinfo = UserIdentity(1).Result;
        }

        public void SqlData()
        {
            _logger.LogTrace("引用从 System.Data.SqlClient 修改成 Microsoft.Data.SqlClient");


            var parameters = new object[] { 1, "MyChy" };
            var sqltxt = @"update [CompUser] set IsDeleted={0},DeletedBy={1},DeletedOn=GETDATE()";

            var Idres = baseUnitOfWork.Context.Database.ExecuteSqlRaw(sqltxt, parameters);

            //执行存储过程 带返回值
            sqltxt = @"DECLARE	@return_value int
                        EXEC	@return_value = [dbo].[OrderDetails_TB]
                        SELECT	'Id' = @return_value";


            baseUnitOfWork.Context.Database.SetCommandTimeout(10 * 60 * 1000);

            var list1 = baseUnitOfWork.Context.Set<CompUser>().FromSqlRaw
                (sqltxt).
                Select(x => new CompUser()
                {
                    Id = x.Id,
                }).ToList();


            baseUnitOfWork.Context.Database.SetCommandTimeout(60 * 1000);


            var sql = new string(@"update [CompUser] set IsDeleted=@is,DeletedBy=@user,DeletedOn=GETDATE()
                                        where id = @id");
            var parameter = new SqlParameter[] {
                new SqlParameter("@id", 1),
                new SqlParameter("@user", "MyChy"),
                new SqlParameter("@is", 1),
            };

            //var sql = new string(@"update [CompUser] set IsDeleted=@is,DeletedOn=GETDATE()
            //                            where id = @id");
            //var parameter = new SqlParameter[] {
            //    new SqlParameter("@id", 1),
            //    new SqlParameter("@user", "MyChy"),
            //    new SqlParameter("@is", 1),
            //};

            //var user = new SqlParameter("user", "johndoe");

            //The SqlParameterCollection only accepts non-null SqlParameter type objects, not SqlParameter objects.”
            var x1 = baseUnitOfWork.Context.Database.ExecuteSqlRaw(sql, parameter);


            //var x1 = baseUnitOfWork.Context.Database.ExecuteSqlCommand("update [CompUser] set [NickName]=@name where id=@id",
            //new SqlParameter[] {
            //      new SqlParameter("@name","1234"),
            //      new SqlParameter("@id",1),
            //});

            var xlist = baseUnitOfWork.Context.Set<CompUser>().
                FromSqlRaw("select * from [CompUser] where id<@id", new SqlParameter[] {
                  new SqlParameter("@id",10),
            }).ToList();



            var predicate = PredicateBuilder.New<CompUser>();

            var id = 5;
            xlist = baseUnitOfWork.Context.Set<CompUser>().FromSqlRaw($"select * from [CompUser] where id<{id}").ToList();



            // var city = "Redmond";
            //  context.Customers.FromSql($"SELECT * FROM Customers WHERE City = {city}");


            var xlist2 = baseUnitOfWork.Context.Set<CompUser>()
                .FromSqlRaw("select NickName,UserName from [CompUser] where id<@id",
                new SqlParameter[] {
                  new SqlParameter("@id",10),
            })
                .Select(x => new CompUserOther()
                {
                    NickName = x.NickName,
                    UserName = x.UserName,

                })
            .ToList();


            //  baseUnitOfWork.Context.Set<CompUser>

            var ss = predicate.Body.ToString();

            predicate.And(x => x.State == true);

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

            for (int i = 0; i < 100; i++)
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


            var code = SessionServer.Get<long>("userid");

            SessionServer.Set("userid", DateTime.Now.Ticks);
            code = SessionServer.Get<long>("userid");

            var codes = SessionServer.Get<string>("userid");
            SessionServer.Set("userid", DateTime.Now.Ticks.ToString());
            codes = SessionServer.Get<string>("userid");


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

            var UserIdentity = ClaimsIdentityServer.AccountUserid();

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
            UserIdentity.Success = true;

            return UserIdentity;


        }
    }
}