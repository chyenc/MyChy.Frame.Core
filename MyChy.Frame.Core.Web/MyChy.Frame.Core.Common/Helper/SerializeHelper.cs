using System;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace MyChy.Frame.Core.Common.Helper
{
    public class SerializeHelper
    {
        /// <summary>
        /// 类序列化成字符
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjToString(object obj)
        {
            var result = string.Empty;

            if (obj == null) return result;
            var ty = obj.GetType();
            var ty1 = ty.GetTypeInfo().BaseType;
            if (ty1 != null && ty1.Name == "ValueType")
            {
                result = JsonConvert.SerializeObject(obj);
            }
            else
            {
                if ((ty.Name == "String") || (ty.Name == "StringBuilder"))
                {
                    result = obj.ToString();
                }
                else
                {
                    result = JsonConvert.SerializeObject(obj);
                }

            }

            return result;

        }

        /// <summary>
        /// 字符转化成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T StringToObj<T>(string value)
        {
            return StringToObj<T>(value, default(T));
        }
        


        /// <summary>
        /// 字符转化成类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static T StringToObj<T>(string value, T def)
        {
            object result;
            if (string.IsNullOrEmpty(value))
            {
                return def;
            }
            try
            {
                var ty = typeof(T);
                var ty1 = ty.GetTypeInfo().BaseType;
                if ((ty1 != null) && (ty1.Name == "ValueType"))
                {
                    result = JsonConvert.DeserializeObject(value, ty);
                }
                else
                {
                    if ((ty.Name == "String") || (ty.Name == "StringBuilder"))
                    {
                        if (ty.Name == "String")
                        {
                            result = value.ToString();
                        }
                        else
                        {
                            var sb = new StringBuilder();
                            sb.Append(value);
                            result = sb;
                        }
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<T>(value);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
                result = null;
            }
            if (result != null)
            {
                return (T)result;
            }
            return def;
        }
    }
}
