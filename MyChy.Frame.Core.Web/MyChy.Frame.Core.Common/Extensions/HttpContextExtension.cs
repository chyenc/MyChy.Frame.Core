using Microsoft.AspNetCore.Http;
using MyChy.Frame.Core.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChy.Frame.Core.Common.Extensions
{
    public static class HttpContextExtension
    {
        public static string GetUserIp(this HttpContext context)
        {
            var ip = "0.0.0.0";
            try
            {
                ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(ip))
                {
                    ip = context.Connection.RemoteIpAddress.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                ip = "0.0.0.1";
            }
            return ip;
        }

        /// <summary>
        /// 获取请求的完整URL
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            var host = request.Host.ToString();
            var prot = request.Host.Port;
            if (prot != null && prot.Value == 80)
            {
                host = request.Host.Host;
            }

           // host = host.Replace(":80", "");
                var url=new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
            return url;
        }


        /// <summary>
        /// <para>将 URL 中的参数名称/值编码为合法的格式。</para>
        /// <para>可以解决类似这样的问题：假设参数名为 tvshow, 参数值为 Tom&Jerry，如果不编码，
        /// 可能得到的网址： http://a.com/?tvshow=Tom&Jerry&year=1965 编码后则为：http://a.com/?tvshow=Tom%26Jerry&year=1965 </para>
        /// <para>实践中经常导致问题的字符有：'&', '?', '=' 等</para>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AsUrlData(this string data)
        {
            if (data == null)
            {
                return null;
            }
            return Uri.EscapeDataString(data);
        }




    }
}
