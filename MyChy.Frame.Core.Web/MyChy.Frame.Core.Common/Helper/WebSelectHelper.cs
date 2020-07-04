using Microsoft.AspNetCore.Mvc.Rendering;
using MyChy.Frame.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Common.Helper
{
    ///// <summary>
    ///// 选择帮助
    ///// </summary>
    //public class WebSelectHelper
    //{
    //    /// <summary>
    //    /// 赋值默认选择
    //    /// </summary>
    //    /// <param name="listItems"></param>
    //    /// <param name="deflist"></param>
    //    /// <returns></returns>
    //    public ICollection<SelectListItem> DefaultSelect(ICollection<SelectListItem> listItems, IList<int> deflist)
    //    {
    //        if (listItems != null && listItems.Count > 0 && deflist != null && deflist.Count > 0)
    //        {
    //            foreach (var i in listItems)
    //            {
    //                int val = i.Value.To<int>();
    //                if (deflist.Contains(val))
    //                {
    //                    i.Selected = true;
    //                }
    //            }
    //        }
    //        return listItems;
    //    }
    //}
}
