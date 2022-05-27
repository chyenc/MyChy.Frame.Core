using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Common.Helper
{
    public class WebNewHelper
    {
        /// <summary>
        /// 发送Web请求返回结果
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postDataStr">参数 例如:arg1=a&arg2=b</param>
        /// <returns></returns>
        public static T HttpClientPost<T>(string url, string postDataStr
            , IDictionary<string, string> HeaderDictionary = null, string ContentType = "application/json")
        {
            string result = string.Empty;
            try
            {
                
                Uri postUrl = new Uri(url);

                using (HttpContent httpContent = new StringContent(postDataStr))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                    if (HeaderDictionary != null && HeaderDictionary.Count > 0)
                    {
                        foreach (var i in HeaderDictionary)
                        {
                            httpContent.Headers.Add(i.Key, i.Value);
                        }
                    }
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = new TimeSpan(0, 0, 600);
                        result = httpClient.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync().Result;

                    }

                }
 
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
            }
            if (string.IsNullOrEmpty(result))
            { 
                return default(T);
            }
            var t = SerializeHelper.StringToObj<T>(result);

            return t;
        }


        /// <summary>
        /// 发送Web请求返回结果
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postDataStr">参数 例如:arg1=a&arg2=b</param>
        /// <returns></returns>
        public static async Task<T> HttpClientPostAsync<T>(string url, string postDataStr
            , IDictionary<string, string> HeaderDictionary = null, string ContentType = "application/json")
        {
            string result = string.Empty;
            try
            {
                Uri postUrl = new Uri(url);

                using (HttpContent httpContent = new StringContent(postDataStr))
                {
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                    if (HeaderDictionary != null && HeaderDictionary.Count > 0)
                    {
                        foreach (var i in HeaderDictionary)
                        {
                            httpContent.Headers.Add(i.Key, i.Value);
                        }
                    }
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = new TimeSpan(0, 0, 600);
                        var post = await httpClient.PostAsync(url, httpContent);
                        result =await post.Content.ReadAsStringAsync();
                    }

                }

            }
            catch (Exception e)
            {
                LogHelper.Log(e);
            }
            if (string.IsNullOrEmpty(result))
            {
                return default(T);
            }
            var t = SerializeHelper.StringToObj<T>(result);

            return t;
        }
    }
}
