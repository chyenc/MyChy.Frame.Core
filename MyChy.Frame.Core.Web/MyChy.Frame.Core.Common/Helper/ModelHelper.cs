using MyChy.Frame.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Common.Helper
{
    public class ModelHelper
    {

        /// <summary>
        /// IDictionary 转 T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static T ModelByIDictionary<T>(IDictionary<string, string> dict)
        {
            var t = typeof(T);
            var model = Activator.CreateInstance(t);
            var col = TypeDescriptor.GetProperties(model);
            var list = new HashSet<string>();
            foreach (PropertyDescriptor item in col)
            {
                if (dict.ContainsKey(item.Name))
                {
                    list.Add(item.Name);
                }
            }
            foreach (PropertyDescriptor item in col)
            {
                if (!list.Contains(item.Name)) continue;
                var value = ObjectExtension.GetValueByType(item.PropertyType, dict[item.Name]);
                if (value != null)
                {
                    item.SetValue(model, value);
                }
            }
            return (T)model;
        }



        ///// <summary>
        ///// table 自动转换成类
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="da"></param>
        ///// <returns></returns>
        //public static T GetModelByTable<T>(DataTable da)
        //{
        //    if (da.Rows.Count == 0) return default(T);

        //    var result = GetListModelByTable<T>(da);
        //    if (result != null && result.Count > 0)
        //    {
        //        return result.ToList<T>()[0];
        //    }
        //    return default(T);
        //}


        ///// <summary>
        ///// table 自动转换成类
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="da"></param>
        ///// <returns></returns>
        //public static IList<T> GetListModelByTable<T>(DataTable da)
        //{
        //    IList<T> result = new List<T>();
        //    if (da == null)
        //    {
        //        return result;
        //    }

        //    var t = typeof(T);
        //    var model = Activator.CreateInstance(t);
        //    var col = TypeDescriptor.GetProperties(model);
        //    HashSet<string> list = null;
        //    var webcachename = string.Empty;
        //    //if (WebCache.IsCache)
        //    //{
        //    //    webcachename = t.FullName + "_" + SafeSecurity.Md5Encrypt(sqltxt);
        //    //    list = WebCache.GetCache<HashSet<string>>(webcachename);
        //    //}
        //    //  if (list == null)
        //    // {
        //    list = new HashSet<string>();
        //    foreach (PropertyDescriptor item in col)
        //    {
        //        if (da.Columns.Contains(item.Name))
        //        {
        //            list.Add(item.Name);
        //        }
        //    }
        //    //if (WebCache.IsCache)
        //    //{
        //    //    WebCache.SetCache(webcachename, list, 60);
        //    //}
        //    //  }


        //    foreach (DataRow dr in da.Rows)
        //    {
        //        model = Activator.CreateInstance(t);
        //        foreach (PropertyDescriptor item in col)
        //        {
        //            if (!list.Contains(item.Name)) continue;
        //            var value = ObjectExtension.GetValueByType(item.PropertyType, dr[item.Name]);
        //            if (value != null)
        //            {
        //                item.SetValue(model, value);
        //            }
        //        }
        //        if (model != null)
        //        {
        //            result.Add((T)model);
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 类 自动转换成 table
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static DataTable GetTableByListModel<T>(IList<T> list)
        //{
        //    var result = new DataTable();
        //    if (list == null)
        //    {
        //        return result;
        //    }
        //    var t = typeof(T);
        //    var pi = t.GetProperties();
        //    foreach (var item in pi)
        //    {
        //        result.Columns.Add(new DataColumn(item.Name, item.PropertyType));
        //    }
        //    foreach (var i in list)
        //    {
        //        var newRow = result.NewRow();
        //        for (var j = 0; j < result.Columns.Count; j++)
        //        {
        //            newRow[j] = t.InvokeMember(result.Columns[j].ColumnName, BindingFlags.GetProperty,
        //                null, i, new object[] { });
        //        }
        //        result.Rows.Add(newRow);
        //    }
        //    return result;
        //}
    }
}
