using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyChy.Frame.Core.Common.Extensions;

namespace MyChy.Frame.Core.Common.Helper
{
    public static class WebHelper
    {
        private  const int TIME_OUT= 10000;

        #region 同步方法

        #region Get

        /// <summary>
        /// 使用Get方法获取字符串结果（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url, string formData, Encoding encoding = null)
        {
            if (!string.IsNullOrEmpty(formData))
            {
                url = url + "?" + formData;
            }
            return HttpGet(url, encoding);
        }

        /// <summary>
        /// 使用Get方法获取字符串结果（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Encoding encoding = null)
        {
            return HttpGet(url, null, encoding, null);
        }

        /// <summary>
        /// 使用Get方法获取字符串结果（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="encoding"></param>
        /// <param name="cer">证书，如果不需要则保留null</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static string HttpGet(string url, CookieContainer cookieContainer = null,
            Encoding encoding = null, X509Certificate cer = null, int timeOut = TIME_OUT)
        {
            var handler = new HttpClientHandler();
            if (cookieContainer != null)
            {
                handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true,
                };
            }
            if (cer != null)
            {
                handler.ClientCertificates.Add(cer);
            }
            var httpClient = new HttpClient(handler)
            {
                Timeout = DayTimeHelper.CalculatingDifferenceSecond(timeOut)
            };
            var t = httpClient.GetStringAsync(url);
            t.Wait();
            return t.Result;
        }

        #endregion

        #region Post

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null,
            string formData = null, Encoding encoding = null,
            X509Certificate cer = null, int timeOut = TIME_OUT)
        {
            var ms = new MemoryStream();
            FillFormDataStream(formData,ms);//填充formData
            return HttpPost(url, cookieContainer, ms, null, null, encoding, cer, timeOut);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> formData = null,
            CookieContainer cookieContainer = null, 
            Encoding encoding = null, 
            X509Certificate cer = null, int timeOut = TIME_OUT)
        {
            var ms = new MemoryStream();
            formData.FillFormDataStream(ms);//填充formData
            return HttpPost(url, cookieContainer, ms, null, null, encoding, cer, timeOut);
        }

        /// <summary>
        /// 发送Web请求返回结果
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="postDataStr">参数 例如:arg1=a&arg2=b</param>
        /// <returns></returns>
        public static string HttpPostJson(string url, string postDataStr)
        {
            var retString = string.Empty;
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                //上面的http头看情况而定，但是下面俩必须加  
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Timeout = 100000;

                var encoding = Encoding.UTF8;//根据网站的编码自定义  
                byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，格式还是和上次说的一样  
                request.ContentLength = postData.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(postData, 0, postData.Length);
                //requestStream.Close();  

                var response = (HttpWebResponse)request.GetResponse();

                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }
                    var streamReader = new StreamReader(responseStream, encoding);
                    retString = streamReader.ReadToEnd();

                    streamReader.Close();
                    responseStream.Close();
                }

            }
            catch (Exception e)
            {
                LogHelper.Log(e);
            }

            return retString;
        }




        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postStream"></param>
        /// <param name="fileDictionary">需要上传的文件，Key：对应要上传的Name，Value：本地文件名</param>
        /// <param name="encoding"></param>
        /// <param name="cer">证书，如果不需要则保留null</param>
        /// <param name="timeOut"></param>
        /// <param name="checkValidationResult">验证服务器证书回调自动验证</param>
        /// <param name="refererUrl"></param>
        /// <returns></returns>
        public static string HttpPost(string url, CookieContainer cookieContainer = null,
            Stream postStream = null, Dictionary<string, string> fileDictionary = null, 
            string refererUrl = null, Encoding encoding = null, X509Certificate cer = null,
            int timeOut = TIME_OUT, bool checkValidationResult = false)
        {
            if (cookieContainer == null)
                cookieContainer = new CookieContainer();

            var handler = new HttpClientHandler {CookieContainer = cookieContainer};

            if (cer != null)
            {
                handler.ClientCertificates.Add(cer);
            }

            //HttpClient client = new HttpClient(handler);
            var client = new HttpClient(handler)
            {
                Timeout = DayTimeHelper.CalculatingDifferenceSecond(timeOut)
            };
            HttpContent hc = new StreamContent(postStream);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
            //hc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            hc.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            hc.Headers.Add("Timeout", timeOut.ToString());
            hc.Headers.Add("KeepAlive", "true");

            #region 处理Form表单文件上传
            var formUploadFile = fileDictionary != null && fileDictionary.Count > 0;//是否用Form上传文件
            if (formUploadFile)
            {
                //通过表单上传文件
                postStream = postStream ?? new MemoryStream();

                string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                //byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                string fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                string dataFormdataTemplate = "\r\n--" + boundary +
                                              "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (var file in fileDictionary)
                {
                    try
                    {
                        var fileName = file.Value;
                        //准备文件流
                        using (var fileStream = FileHelper.GetFileStream(fileName))
                        {
                            string formdata = null;
                            formdata = fileStream != null ? 
                                string.Format(fileFormdataTemplate, file.Key, /*fileName*/ Path.GetFileName(fileName)) :
                                string.Format(dataFormdataTemplate, file.Key, file.Value);

                            //统一处理
                            var formdataBytes = Encoding.UTF8.GetBytes(postStream.Length == 0 ? 
                                formdata.Substring(2, formdata.Length - 2)
                                : formdata);//第一行不需要换行
                            postStream.Write(formdataBytes, 0, formdataBytes.Length);

                            //写入文件
                            if (fileStream == null) continue;
                            var buffer = new byte[1024];
                            int bytesRead = 0;
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                postStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(ex);
                       // throw ex;
                    }
                }
                //结尾
                var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

                hc.Headers.ContentType = new MediaTypeHeaderValue(string.Format("multipart/form-data; boundary={0}", boundary));
            }
            else
            {
                hc.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            }
            #endregion

            var t = client.PostAsync(url, hc);
            t.Wait();
            var t1 = t.Result.Content.ReadAsStringAsync();
            t1.Wait();
            return t1.Result;
        }

        #endregion

        /// <summary>
        /// 验证服务器证书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
        {
            var dataString = formData.ToQueryString();
            FillFormDataStream(dataString, stream);
        }

        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static void FillFormDataStream(string formData, Stream stream)
        {
             var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(formData);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }

        #endregion

        #region 异步方法

        /// <summary>
        /// 使用Get方法获取字符串结果（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formData"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, string formData, Encoding encoding = null)
        {
            if (!string.IsNullOrEmpty(formData))
            {
                url = url + "?" + formData;
            }
            return await HttpGetAsync(url, encoding);
        }


        /// <summary>
        /// 使用Get方法获取字符串结果（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Encoding encoding = null)
        {
            return await HttpGetAsync(url, null, encoding, null);
        }

        /// <summary>
        /// 使用Get方法获取字符串结果（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="encoding"></param>
        /// <param name="cer">证书，如果不需要则保留null</param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, CookieContainer cookieContainer = null,
            Encoding encoding = null, X509Certificate cer = null, int timeOut = TIME_OUT)
        {
            var handler = new HttpClientHandler();
            if (cookieContainer != null)
            {
                handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true,
                };
            }
            if (cer != null)
            {
                handler.ClientCertificates.Add(cer);
            }
            var httpClient = new HttpClient(handler)
            {
                Timeout = DayTimeHelper.CalculatingDifferenceSecond(timeOut)
            };
            return await httpClient.GetStringAsync(url);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, string formData = null, 
            CookieContainer cookieContainer = null,
           Encoding encoding = null,
            X509Certificate cer = null, int timeOut = TIME_OUT)
        {
            var ms = new MemoryStream();
            await FillFormDataStreamAsync(formData, ms);//填充formData
            return await HttpPostAsync(url, cookieContainer, ms, null, null, encoding, cer, timeOut);
        }


        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, Dictionary<string, string> formData = null, 
            CookieContainer cookieContainer = null,
           Encoding encoding = null,
            X509Certificate cer = null, int timeOut = TIME_OUT)
        {
            var ms = new MemoryStream();
            await formData.FillFormDataStreamAsync(ms);//填充formData
            return await HttpPostAsync(url, cookieContainer, ms, null, null, encoding, cer, timeOut);
        }


        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postStream"></param>
        /// <param name="fileDictionary">需要上传的文件，Key：对应要上传的Name，Value：本地文件名</param>
        /// <param name="encoding"></param>
        /// <param name="cer">证书，如果不需要则保留null</param>
        /// <param name="timeOut"></param>
        /// <param name="checkValidationResult">验证服务器证书回调自动验证</param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, CookieContainer cookieContainer = null,
            Stream postStream = null, Dictionary<string, string> fileDictionary = null, string refererUrl = null,
            Encoding encoding = null, X509Certificate cer = null, int timeOut = TIME_OUT, bool checkValidationResult = false)
        {
            if (cookieContainer == null)
                cookieContainer = new CookieContainer();

            var handler = new HttpClientHandler {CookieContainer = cookieContainer};
            if (cer != null)
            {
                handler.ClientCertificates.Add(cer);
            }

            var client = new HttpClient(handler)
            {
                Timeout = DayTimeHelper.CalculatingDifferenceSecond(timeOut)
            };
            HttpContent hc = new StreamContent(postStream);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
            //hc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            hc.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            hc.Headers.Add("Timeout", timeOut.ToString());
            hc.Headers.Add("KeepAlive", "true");

            #region 处理Form表单文件上传
            var formUploadFile = fileDictionary != null && fileDictionary.Count > 0;//是否用Form上传文件
            if (formUploadFile)
            {
                //通过表单上传文件
                postStream = postStream ?? new MemoryStream();

                var boundary = "----" + DateTime.Now.Ticks.ToString("x");
                //byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                var fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                var dataFormdataTemplate = "\r\n--" + boundary +
                                              "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (var file in fileDictionary)
                {
                    try
                    {
                        var fileName = file.Value;
                        //准备文件流
                        using (var fileStream = FileHelper.GetFileStream(fileName))
                        {
                            string formdata = null;
                            formdata = fileStream != null ? 
                                string.Format(fileFormdataTemplate, file.Key, /*fileName*/ Path.GetFileName(fileName)) : 
                                string.Format(dataFormdataTemplate, file.Key, file.Value);

                            //统一处理
                            var formdataBytes = Encoding.UTF8.GetBytes(postStream.Length == 0 ?
                                formdata.Substring(2, formdata.Length - 2) : 
                                formdata);//第一行不需要换行

                            postStream.Write(formdataBytes, 0, formdataBytes.Length);

                            //写入文件
                            if (fileStream == null) continue;
                            var buffer = new byte[1024];
                            var bytesRead = 0;
                            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                postStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log(ex);
                        //throw ex;
                    }
                }
                //结尾
                var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

                hc.Headers.ContentType = 
                    new MediaTypeHeaderValue(string.Format("multipart/form-data; boundary={0}", boundary));
            }
            else
            {
                hc.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            }
            #endregion

            var r = await client.PostAsync(url, hc);
            return await r.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static async Task FillFormDataStreamAsync(string formData, Stream stream)
        {
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(formData);
            await stream.WriteAsync(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置

        }

        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static async Task FillFormDataStreamAsync(this Dictionary<string, string> formData, Stream stream)
        {
            var dataString = formData.ToQueryString();
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            await stream.WriteAsync(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }


        public static string HttpPostFile(
    string url, CookieContainer cookieContainer = null,
    Dictionary<string, string> formData = null, Encoding encoding = null, int timeOut = 10000
    )
        {
            MemoryStream memoryStream = new MemoryStream();
            formData.FillFormDataStream(memoryStream);
            return HttpPostFile(url, cookieContainer, memoryStream, null, null, encoding, timeOut);
        }

        //
        // 摘要:
        //     使用Post方法获取字符串结果
        //
        // 参数:
        //   url:
        //
        //   cookieContainer:
        //
        //   postStream:
        //
        //   fileDictionary:
        //     需要上传的文件，Key：对应要上传的Name，Value：本地文件名
        //
        //   timeOut:
        public static string HttpPostFile
            (string url, CookieContainer cookieContainer = null,
            Stream postStream = null, Dictionary<string, string> fileDictionary = null,
            string refererUrl = null, Encoding encoding = null, int timeOut = 10000)

        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = timeOut;
            if (fileDictionary != null && fileDictionary.Count > 0)
            {
                postStream = postStream ?? new MemoryStream();
                string text = "----" + DateTime.Now.Ticks.ToString("x");
                string format = "\r\n--" + text + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                string format2 = "\r\n--" + text + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (KeyValuePair<string, string> item in fileDictionary)
                {
                    try
                    {
                        string value = item.Value;
                        using FileStream fileStream = FileHelper.GetFileStream(value);
                        string text2 = null;
                        text2 = ((fileStream == null) ? string.Format(format2, item.Key, item.Value) : string.Format(format, item.Key, value));
                        byte[] bytes = Encoding.ASCII.GetBytes((postStream.Length == 0) ? text2.Substring(2, text2.Length - 2) : text2);
                        postStream.Write(bytes, 0, bytes.Length);
                        if (fileStream != null)
                        {
                            byte[] array = new byte[1024];
                            int num = 0;
                            while ((num = fileStream.Read(array, 0, array.Length)) != 0)
                            {
                                postStream.Write(array, 0, num);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                byte[] bytes2 = Encoding.ASCII.GetBytes("\r\n--" + text + "--\r\n");
                postStream.Write(bytes2, 0, bytes2.Length);
                httpWebRequest.ContentType = $"multipart/form-data; boundary={text}";
            }
            else
            {
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            }

            httpWebRequest.ContentLength = postStream?.Length ?? 0;
            httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            httpWebRequest.KeepAlive = true;
            if (!string.IsNullOrEmpty(refererUrl))
            {
                httpWebRequest.Referer = refererUrl;
            }

            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            if (cookieContainer != null)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            }

            if (postStream != null)
            {
                postStream.Position = 0L;
                Stream requestStream = httpWebRequest.GetRequestStream();
                byte[] array = new byte[1024];
                int num = 0;
                while ((num = postStream.Read(array, 0, array.Length)) != 0)
                {
                    requestStream.Write(array, 0, num);
                }

                postStream.Seek(0L, SeekOrigin.Begin);
                StreamReader streamReader = new StreamReader(postStream);
                string text3 = streamReader.ReadToEnd();
                postStream.Close();
            }

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (cookieContainer != null)
            {
                httpWebResponse.Cookies = cookieContainer.GetCookies(httpWebResponse.ResponseUri);
            }

            using Stream stream = httpWebResponse.GetResponseStream();
            using StreamReader streamReader2 = new StreamReader(stream, encoding ?? Encoding.GetEncoding("utf-8"));
            return streamReader2.ReadToEnd();
        }

        #endregion




        ///// <summary>
        ///// 获取当前用户IP
        ///// </summary>
        ///// <returns></returns>
        //public static string GetIp()
        //{
        //    string result;
        //    try
        //    {
        //        // 如果使用代理，获取真实IP 
        //        result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "" ?
        //            System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] :
        //            System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //        if (string.IsNullOrEmpty(result))
        //            result = System.Web.HttpContext.Current.Request.UserHostAddress;
        //        return result;

        //    }
        //    catch (Exception)
        //    {
        //        result = "0.0.0.0";
        //        //throw;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 判断IP是否合法 真合法
        ///// </summary>
        ///// <param name="listip"></param>
        ///// <returns></returns>
        //public static bool CheckIp(string listip)
        //{
        //    if (string.IsNullOrEmpty(listip)) return true;
        //    var list = listip.Split(',');
        //    var ip = GetIp();
        //    return list.Any(s => s == ip);

        //}

        ///// <summary>
        ///// 获取当前URL
        ///// </summary>
        ///// <returns></returns>
        //public static string GetUrl()
        //{
        //    string result;
        //    try
        //    {
        //        result = System.Web.HttpContext.Current.Request.Url.ToString();

        //    }
        //    catch (Exception exception)
        //    {
        //        LogHelper.Log(exception);
        //        result = "0.0.0.0";
        //        //throw;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 获取上一级URL
        ///// </summary>
        ///// <returns></returns>
        //public static string GetUrlReferrer()
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        if (System.Web.HttpContext.Current.Request.UrlReferrer != null)
        //            result = System.Web.HttpContext.Current.Request.UrlReferrer.AbsoluteUri.ToString();
        //    }
        //    catch (Exception exception)
        //    {
        //        LogHelper.Log(exception);
        //        result = "0.0.0.0";
        //        //throw;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 获取固定位置参数值
        /// </summary>
        /// <param name="length"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetParam(int length, string url)
        {
            var result = string.Empty;
            var arr = url.Split('&');
            if (arr.Length > 0 && arr.Length > length)
            {
                result = arr[length].Split('=')[1];
            }
            return result;
        }

        ///// <summary>
        ///// 读取图片
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //public static byte[] RedFile(string file)
        //{
        //    var filePath = IoFiles.GetFileMapPath(file);
        //    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //    var buffer = new byte[fileStream.Length];
        //    fileStream.Read(buffer, 0, buffer.Length);
        //    fileStream.Close();
        //    return buffer;
        //}


        ///// <summary>
        ///// 判断是否邮箱
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public static bool IsEmail(string email)
        //{
        //    const string strExp = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        //    var r = new Regex(strExp);
        //    var m = r.Match(email);
        //    return m.Success;
        //}
    }
}
