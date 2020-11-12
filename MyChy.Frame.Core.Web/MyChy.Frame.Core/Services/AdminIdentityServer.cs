using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyChy.Frame.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MyChy.Frame.Core.Services
{
    public class AdminIdentityServer
    {
        private const string _Name = "AdminsTicket";
        private const int _Minutes = 60;

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Claims"></param>
        /// <param name="Name"></param>
        private static async void UserLogin(IList<Claim> Claims, string Name = _Name)
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
        public static async void UserLogin(AdminIdentity Front, string Name = _Name, int Minutes = _Minutes)
        {
            // UserOut(_Name);
            SessionServer.Set(Name, Front);

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Front.UserId.To("")),
                new Claim(ClaimTypes.Gender, Front.UserNick.To("")),
                new Claim(ClaimTypes.PrimarySid,  string.Join(",", Front.OrganizationList)),
                new Claim(ClaimTypes.Role, string.Join(",", Front.RoleList)),
                new Claim(ClaimTypes.Authentication,  string.Join(",",Front.AuthorityList)),
                new Claim(ClaimTypes.Expired, Front.EndTime.To("")),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            var Authentication = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Minutes),
                IsPersistent = false,
                AllowRefresh = false,
            };

            await HttpContext.Current.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, Authentication);
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
        public static AdminIdentity AccountUserid()
        {

            var result = new AdminIdentity();
            if (HttpContext.Current == null)
            {
                return result;
            }
            var userinfo = SessionServer.Get<AdminIdentity>(_Name);
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
                result.UserNick = ShowClaimValue<string>(ss, ClaimTypes.Gender);
                result.RoleList = ShowClaimValue<string>(ss, ClaimTypes.Role).ToList<int>();
                result.OrganizationList = ShowClaimValue<string>(ss, ClaimTypes.PrimarySid).ToList<int>();
                //result.RoleType = ShowClaimValue<int>(ss, ClaimTypes.GroupSid);
                result.AuthorityList = ShowClaimValue<string>(ss, ClaimTypes.Authentication).ToList<string>();
                result.EndTime = ShowClaimValue<DateTime>(ss, ClaimTypes.Expired);


                if (!CheckEndTime(result))
                {
                    result = new AdminIdentity();
                }
                else
                {
                    SessionServer.Set(_Name, result);
                }
            }
            if (result.UserId == 0) { result.UserNick = ""; }
            return result;
        }

        private static bool CheckEndTime(AdminIdentity Front)
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

    public class AdminIdentity
    {
        public bool Success { get; set; }

        public int UserId { get; set; }

        public string UserNick { get; set; }

        public IList<int> RoleList { get; set; } = new List<int>();

        public IList<int> OrganizationList { get; set; } = new List<int>();

        public IList<string> AuthorityList { get; set; } = new List<string>();

        public DateTime EndTime { get; set; }

    }
}