using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyChy.Frame.Core.Common.Extensions
{
    public static class DictionaryExtensions
    {
        #region 转换成URL参数
        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<object, object> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(key + "=" + Uri.EscapeDataString(value.ToString()) + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToQueryString(this IDictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.Append(key + "=" + Uri.EscapeDataString(value) + "&");
                }
            }
            return sb.ToString().TrimEnd('&');
        }


        /// <summary>
        /// 转换成参数 按照字符排序
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToOrderString(this IDictionary<object, object> dictionary)
        {
            var sb = new StringBuilder();
            var dictionarys = dictionary.OrderBy(x => x.Key).ToList();
            foreach (var key in dictionarys.Where(key => key.Key != null))
            {
                sb.Append(key.Key + "=" + key.Value + "&");
            }
            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 转换成参数 按照字符排序
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToOrderString(this IDictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            var dictionarys = dictionary.OrderBy(x => x.Key).ToList();
            foreach (var key in dictionarys.Where(key => key.Key != null))
            {
                sb.Append(key.Key + "=" + key.Value + "&");
            }
            return sb.ToString().TrimEnd('&');
        }


        #endregion

        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static void ToStreamData(this Dictionary<string, string> formData, Stream stream)
        {
            var dataString = formData.ToQueryString();
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置

        }


        #region 转换成XML

        /// <summary>
        /// 转换成URL参数
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ToXmlString(this IDictionary<object, object> dictionary)
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                if (value != null)
                {
                    sb.AppendFormat("<{0}>{1}</{0}>", key, value);
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }

        #endregion

    }

}
