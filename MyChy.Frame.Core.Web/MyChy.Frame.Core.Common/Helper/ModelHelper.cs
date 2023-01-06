using MyChy.Frame.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        /// <summary>
        /// Model 根据Keys转换成Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name=""></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> DictionaryByModel<T>
            (T Model, IList<string> keys, string mainkey)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var t = typeof(T);
            // var m = Activator.CreateInstance(t);
            if (keys == null || keys.Count == 0)
            {
                var m = Activator.CreateInstance(t);
                var col = TypeDescriptor.GetProperties(m);
                foreach (PropertyDescriptor item in col)
                {
                    if (!keys.Contains(item.Name))
                    {
                        keys.Add(item.Name);
                    }
                }
            }
            var model = new Dictionary<string, string>();
            var keyvalues = string.Empty;
            foreach (var i in keys)
            {
                keyvalues = string.Empty;
                try
                {
                    var obj = t.InvokeMember(i, BindingFlags.GetProperty,
                         null, Model, new object[] { });
                    if (obj != null)
                    {
                        model.Add(i, obj.ToString());
                        if (mainkey == i) keyvalues = obj.ToString();
                        //worksheet.Cells[Cells].Value = obj;
                    }
                }
                catch (Exception e)
                {
                    //_logger.LogError(e.Message);
                }
            }
            if (string.IsNullOrEmpty(keyvalues))
            {
                result.Add(keyvalues, model);
            }

            return result;
        }


        /// <summary>
        /// Model List 根据Keys转换成Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name=""></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> DictionaryListByModel<T>
            (IList<T> Models, IList<string> keys, string mainkey)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var t = typeof(T);
            // var m = Activator.CreateInstance(t);

            if (keys == null || keys.Count == 0)
            {
                var m = Activator.CreateInstance(t);
                var col = TypeDescriptor.GetProperties(m);
                foreach (PropertyDescriptor item in col)
                {
                    if (!keys.Contains(item.Name))
                    {
                        keys.Add(item.Name);
                    }
                }
            }
            var keyvalues = string.Empty;
            foreach (var m in Models)
            {
                keyvalues = string.Empty;
                var model = new Dictionary<string, string>();
                foreach (var i in keys)
                {
                    try
                    {
                        var obj = t.InvokeMember(i, BindingFlags.GetProperty,
                             null, m, new object[] { });
                        if (obj != null)
                        {
                            model.Add(i, obj.ToString());
                            if (mainkey == i) keyvalues = obj.ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        //_logger.LogError(e.Message);
                    }
                }
                result.Add(keyvalues, model);

            }

            return result;
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

        /// <summary>
        /// 类 自动转换成 table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable GetTableByListModel<T>(IList<T> list)
        {
            var result = new DataTable();
            if (list == null)
            {
                return result;
            }
            var t = typeof(T);
            var pi = t.GetProperties();
            foreach (var item in pi)
            {
                result.Columns.Add(new DataColumn(item.Name, item.PropertyType));
            }
            foreach (var i in list)
            {
                var newRow = result.NewRow();
                for (var j = 0; j < result.Columns.Count; j++)
                {
                    newRow[j] = t.InvokeMember(result.Columns[j].ColumnName, BindingFlags.GetProperty,
                        null, i, new object[] { });
                }
                result.Rows.Add(newRow);
            }
            return result;
        }



        /// <summary>
        /// 类 自动转换成 table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="OutName"></param>
        /// <param name="IsAddNull">是否添加可以空的类</param>
        /// <returns></returns>
        public static DataTable GetTableByListModel<T>(IEnumerable<T> list, IList<string> OutName, bool IsAddNull = false)
        {
            var result = new DataTable();
            if (list == null)
            {
                return result;
            }
            var t = typeof(T);
            var pi = t.GetProperties();
            var DateTimeOffsetList = new List<string>();
            foreach (var item in pi)
            {
                if (!OutName.Contains(item.Name))
                {
                    if (item.PropertyType.Name == "Nullable`1")
                    {
                        if (IsAddNull)
                        {
                            result.Columns.Add(new DataColumn(item.Name));
                        }
                    }
                    else
                    {
                        //if (item.PropertyType.Name == "DateTimeOffset")
                        //{
                        //    result.Columns.Add(new DataColumn(item.Name, typeof(DateTime)));
                        //    DateTimeOffsetList.Add(item.Name);
                        //}
                        //else
                        //{
                            result.Columns.Add(new DataColumn(item.Name, item.PropertyType));
                      //  }


                    }

                }
            }
            foreach (var i in list)
            {
                var newRow = result.NewRow();

                foreach (DataColumn col in result.Columns)
                {
                    //foreach (var prop in pi)
                    //{
                    var obj = t.InvokeMember(col.ColumnName, BindingFlags.GetProperty, null, i, new object[] { });
                    //if (DateTimeOffsetList.Contains(col.ColumnName))
                    //{
                    //    obj = ((DateTimeOffset)obj).DateTime;
                    //}
                    newRow[col.ColumnName] = obj;

                    //}
                }
                result.Rows.Add(newRow);
            }
            return result;
        }
    }
}
