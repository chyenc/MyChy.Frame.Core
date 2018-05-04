using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.EFCore.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyChy.Frame.Core.EFCore.AutoHistorys.Internal
{
    /// <summary>
    /// Represents the entity change history.
    /// </summary>
    public class AutoHistory: BaseEntity
    {
        ///// <summary>
        ///// Gets or sets the primary key.
        ///// </summary>
        ///// <value>The id.</value>
        //public int Id { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        /// <value>The json after changed.</value>
        [StringLength(128)]
        [Description("操作人")]
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the source id.
        /// </summary>
        /// <value>The source id.</value>
        [StringLength(50)]
        [Description("原始ID")]
        public string SourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        [StringLength(128)]
        [Description("表名称")]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the json before changing.
        /// </summary>
        /// <value>The json before changing.</value>
        [Description("原始数据")]
        public string BeforeJson { get; set; }

        /// <summary>
        /// Gets or sets the json after changed.
        /// </summary>
        /// <value>The json after changed.</value>
        [Description("修改后数据")]
        public string AfterJson { get; set; }

        /// <summary>
        /// Gets or sets the change kind.
        /// </summary>
        /// <value>The change kind.</value>
        public EntityState Kind { get; set; }



        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
