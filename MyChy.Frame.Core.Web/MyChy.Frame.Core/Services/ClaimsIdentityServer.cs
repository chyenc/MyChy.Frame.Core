using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using MyChy.Frame.Core.Common.Extensions;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MyChy.Frame.Core.Services
{
    public class ClaimsIdentityServer
    {
        private const string _Name = "AdminTicket";
        private const int _Minutes = 60;

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Claims"></param>
        /// <param name="Name"></param>
        public static async void UserLogin(IList<Claim> Claims, string Name = _Name)
        {
            var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Claims"></param>
        /// <param name="Name"></param>
        /// <param name="Minutes"></param>
        public static async void UserLogin(FrontIdentity Front, string Name = _Name, int Minutes = _Minutes)
        {
            // UserOut(_Name);
            SessionServer.Set(Name, Front);

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Front.UserId.To("")),
                new Claim(ClaimTypes.Name, Front.UserIds.To("")),
                new Claim(ClaimTypes.Gender, Front.UserNick.To("")),
                new Claim(ClaimTypes.MobilePhone, Front.Mobile.To("")),
                new Claim(ClaimTypes.PrimarySid, Front.OrganizationId.To("")),
                new Claim(ClaimTypes.Actor, Front.OpenId.To("")),
                new Claim(ClaimTypes.Role, Front.RoleId.To("")),
                new Claim(ClaimTypes.Authentication, Front.Authority.To("")),
                new Claim(ClaimTypes.Expired, Front.EndTime.To("")),
                new Claim(ClaimTypes.GroupSid, Front.RoleType.To(""))

            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            var Authentication = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Minutes),
                IsPersistent = false,
                AllowRefresh = false,
                 
            };

            await HttpContext.Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            //  await HttpContext.Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, Authentication);
        }



        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="Name"></param>
        public static async void UserOut(string Name = _Name)
        {
            SessionServer.Remove(Name);
            await HttpContext.Current.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        /// <summary>
        /// 返回登录用户信息
        /// </summary>
        /// <returns></returns>
        public static FrontIdentity AccountUserid()
        {

            var result = new FrontIdentity();
            if (HttpContext.Current == null)
            {
                return result;
            }
            var userinfo = SessionServer.Get<FrontIdentity>(_Name);
            if (userinfo != null && userinfo.Success)
            {
                return userinfo;
            }
            if (HttpContext.Current.User == null)
            {
                return result;
            }
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                result.Success = true;
                var ss = HttpContext.Current.User.Claims;
                result.UserId = ShowClaimValue<int>(ss, ClaimTypes.NameIdentifier);
                result.UserIds = ShowClaimValue<string>(ss, ClaimTypes.Name);
                result.UserNick = ShowClaimValue<string>(ss, ClaimTypes.Gender);
                result.Mobile = ShowClaimValue<string>(ss, ClaimTypes.MobilePhone);
                result.OpenId = ShowClaimValue<string>(ss, ClaimTypes.Actor);

                result.RoleId = ShowClaimValue<int>(ss, ClaimTypes.Role);
                result.RoleType= ShowClaimValue<int>(ss, ClaimTypes.GroupSid);
                result.Authority = ShowClaimValue<string>(ss, ClaimTypes.Authentication);
                result.EndTime = ShowClaimValue<DateTime>(ss, ClaimTypes.Expired);
                result.OrganizationId = ShowClaimValue<string>(ss, ClaimTypes.PrimarySid);

                if (!CheckEndTime(result))
                {
                    result = new FrontIdentity();
                }
                else
                {
                    SessionServer.Set(_Name, result);
                }
            }
            if (result.UserId == 0) { result.UserNick = ""; }
            return result;
        }

        private static bool CheckEndTime(FrontIdentity Front)
        {
            var result = true;
            if (Front.Success)
            {
                if (Front.EndTime < DateTime.Now)
                {
                    result = false;
                    UserOut(_Name);
                }
                else if (DateTime.Now.AddMinutes(20) > Front.EndTime)
                {
                    Front.EndTime = DateTime.Now.AddMinutes(_Minutes);
                    UserLogin(Front);
                    result = false;
                }
            }
            else
            {
                result = false;
                UserOut(_Name);
            }
            return result;
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

        public int RoleType { get; set; }

        public int RoleId { get; set; }

        public string OrganizationId { get; set; }

        public string UserIds { get; set; }

        public string UserNick { get; set; }

        public string OpenId { get; set; }

        public string Mobile { get; set; }

        public string Authority { get; set; }

        public DateTime EndTime { get; set; }

    }
}
