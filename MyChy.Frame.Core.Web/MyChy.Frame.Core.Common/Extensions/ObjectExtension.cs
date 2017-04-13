using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyChy.Frame.Core.Common.Extensions
{
    public static class ObjectExtension
    {
        #region To T

        /// <summary>
        /// 将字符转换成自己的类型
        /// </summary>
        /// <param name="val">System.String</param>
        /// <returns>如果转换失败将返回 T 的默认值</returns>
        public static T To<T>(this object val)
        {
            return val != null ? val.To<T>(default(T)) : default(T);
        }

        /// <summary>
        /// 将字符转换成自己的类型
        /// </summary>
        /// <param name="val">System.Object</param>
        /// <param name="defVal">在转换成 T 失败时，返回的默认值</param>
        /// <returns>类型 T 的值</returns>
        public static T To<T>(this object val, T defVal)
        {
            if (val == null)
                return (T)defVal;
            if (val is T)
                return (T)val;

            var type = typeof(T);
            try
            {
                if (type.GetTypeInfo().BaseType == typeof(Enum))
                {
                    return (T)Enum.Parse(type, val.ToString(), true);
                }
                return (T)Convert.ChangeType(val, type);
            }
            catch
            {
                return defVal;
            }
        }

        #endregion

        //  private static readonly Type ValueTypeType = typeof(ValueType);

        /// <summary>
        /// 根据类型 名称 获取context值
        /// </summary>
        /// <param name="ty">类型</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object GetValueByType(Type ty, object value)
        {
            if (value == null) return null;
            try
            {
                object result;

                if (ty.Name == "String" || ty.Name == "StringBuilder")
                {
                    result = value.ToString();
                }
                else
                {
                    if (ty == typeof(decimal))
                    {
                        ty = typeof(int);
                    }
                    var objvalue = string.Format("\"{0}\"", value);
                    result = JsonConvert.DeserializeObject(objvalue, ty);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据属性名称获取指定对象中该属性的值
        /// </summary>
        /// <param name="obj">属性所在对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回获取到的属性值</returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName);
            var propertyValue = propertyInfo?.GetValue(obj, null);
            return propertyValue;
        }

    }
}
