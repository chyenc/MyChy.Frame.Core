using MyChy.Frame.Core.Common.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace MyChy.Frame.Core.Common.Extensions
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this Enum obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string obj) where T : struct
        {
            if (string.IsNullOrEmpty(obj))
            {
                return default(T);
            }
            try
            {
                return (T)Enum.Parse(typeof(T), obj, true);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetDescription(this Type type, int? id)
        {
            var values = from Enum e in Enum.GetValues(type)
                         select new { id = e.ToInt(), name = e.ToDescription() };

            if (!id.HasValue) id = 0;

            return values.ToList().Find(c => c.id == id).name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IList<EnumListModel> GetDescriptionList(this Type type, int? id)
        {
            var values = from Enum e in Enum.GetValues(type)
                         select new EnumListModel() { Id = e.ToInt(), Title = e.ToDescription() };

            if (id.GetValueOrDefault() <= 0) return values.ToList();
            var idx = id.GetValueOrDefault();
            var enumListModels = values.ToList();
            foreach (var model in enumListModels.Where(model => model.Id == idx))
            {
                model.IsCheck = true;
                break;
            }
            return enumListModels;
        }


        /// <summary>
        ///     获取枚举项的Description特性的描述文字
        /// </summary>
        /// <param name="enumeration"> </param>
        /// <returns> </returns>
        public static string ToDescription(this Enum enumeration)
        {
            var type = enumeration.GetType();
            var members = type.GetMember(enumeration.To<string>());
            return members.Length > 0 ? members[0].ToDescription() : enumeration.To<string>();
        }
    }
}
