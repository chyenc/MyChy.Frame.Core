using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using MyChy.Frame.Core.Common.Extensions;
using System;
using Microsoft.AspNetCore.Http.Authentication;
using MyChy.Frame.Core.Common.Helper;
using Microsoft.AspNetCore.Session;


namespace MyChy.Frame.Core.Services
{
    public class ClaimsIdentityServer
    {
        private const string _Name = "AdminTicket";
        private const int _Minutes =60;

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Claims"></param>
        /// <param name="Name"></param>
        public static async void UserLogin(IList<Claim> Claims, string Name = _Name)
        {
            var claimsIdentity = new ClaimsIdentity(Claims, Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.Current.Authentication.SignInAsync(Name, claimsPrincipal);

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Claims"></param>
        /// <param name="Name"></param>
        /// <param name="Minutes"></param>
        public static async void UserLogin(FrontIdentity Front, string Name = _Name, int Minutes= _Minutes)
        {
           // UserOut(_Name);
            SessionServer.Set(Front, Name, Minutes - 10);

            IList<Claim> Claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,Front.UserId.ToString()),
                new Claim(ClaimTypes.Name,Front.UserIds),
                new Claim(ClaimTypes.MobilePhone,Front.Mobile),
             //   new Claim(ClaimTypes.PrimarySid,Front.UserId.ToString()),
                new Claim(ClaimTypes.Actor,Front.OpenId),
                new Claim(ClaimTypes.Role,Front.RoleId.ToString()),
                new Claim(ClaimTypes.Authentication,Front.Authority),
                new Claim(ClaimTypes.Expired,Front.EndTime.ToString()),

            };
            var claimsIdentity = new ClaimsIdentity(Claims, Name);
        
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            var Authentication = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Minutes),
                IsPersistent = false,
                AllowRefresh = false
            };

            await HttpContext.Current.Authentication.SignInAsync(Name, claimsPrincipal, Authentication);
        }



        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="Name"></param>
        public static async void UserOut(string Name = _Name)
        {
            SessionServer.Remove(Name);
            await HttpContext.Current.Authentication.SignOutAsync(Name);
        }


        /// <summary>
        /// 返回登录用户信息
        /// </summary>
        /// <returns></returns>
        public static FrontIdentity AccountUserid() 
        {
            var userinfo = SessionServer.Get<FrontIdentity>(_Name);
            if (userinfo != null && userinfo.Success)
            {
                return userinfo;
            }

            var result = new FrontIdentity();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                result.Success = true;
                var ss = HttpContext.Current.User.Claims;
                result.UserId = ShowClaimValue<int>(ss, ClaimTypes.NameIdentifier);
                result.UserIds = ShowClaimValue<string>(ss, ClaimTypes.Name);
                result.Mobile = ShowClaimValue<string>(ss, ClaimTypes.MobilePhone);
                result.OpenId = ShowClaimValue<string>(ss, ClaimTypes.Actor);
                result.RoleId = ShowClaimValue<int>(ss, ClaimTypes.Role);
                result.Authority = ShowClaimValue<string>(ss, ClaimTypes.Authentication);
                result.EndTime = ShowClaimValue<DateTime>(ss, ClaimTypes.Expired);
                CheckEndTime(result);
                // SessionServer.Set(result, _Name, _Minutes - 10);
            }
            return result;
        }

        private static void CheckEndTime(FrontIdentity Front)
        {
            if (Front.Success)
            {
                if (Front.EndTime < DateTime.Now)
                {
                    UserOut(_Name);
                }
                else if (DateTime.Now.AddMinutes(20) > Front.EndTime )
                {
                    Front.EndTime = DateTime.Now.AddMinutes(_Minutes);
                    UserLogin(Front);
                }
            }
            else
            {
                UserOut(_Name);
            }
        }


        private static T ShowClaimValue<T>(IEnumerable<Claim> ss, string ClaimTypes)
        {
            var result = string.Empty;

            var yy = ss.FirstOrDefault(x => x.Type == ClaimTypes);
            if (yy != null)
            {
                result = yy.Value;
            }
            return result.To<T>();
        }

    }

    public class FrontIdentity
    {
        public bool Success { get; set; }

        public int UserId { get; set; }

        public string UserIds { get; set; }

        public string OpenId { get; set; }

        public string Mobile { get; set; }

        public int RoleId { get; set; }

        public string Authority { get; set; }

        public DateTime EndTime { get; set; }

    }
}
