using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.Common.Extensions
{
    public static class SelectExtensions
    {
        public static ICollection<SelectListItem> DefaultSelect(this ICollection<SelectListItem> listItems, IList<int> deflist)
        {
            if (listItems != null && listItems.Count > 0 && deflist != null && deflist.Count > 0)
            {
                foreach (var i in listItems)
                {
                    int val = i.Value.To<int>();
                    if (deflist.Contains(val))
                    {
                        i.Selected = true;
                    }
                }
            }
            return listItems;
        }
    }
}
