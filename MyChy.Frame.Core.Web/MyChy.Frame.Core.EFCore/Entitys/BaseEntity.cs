using MyChy.Frame.Core.EFCore.Entitys.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Entitys
{

    public abstract class BaseEntity : EntityWithTypedId
    {

    }

    public abstract class BaseEntity<TKey> : EntityWithTypedId<TKey>
    {


    }

    public abstract class BaseWithAllEntity : BaseWithAllEntity<int>
    {


    }

    public abstract class BaseWithAllEntity<TKey> : EntityWithAllMeta<TKey>
    {
        /// <summary>
        /// A Serial number for the value in the code table.  This can be used to sort (required).
        /// 排序码
        /// </summary>
        [Required]
        public int SortIndex { get; set; }

    }



}
