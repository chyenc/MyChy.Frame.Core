using MyChy.Frame.Core.EFCore.Attributes;
using MyChy.Frame.Core.EFCore.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web5.Domains
{
   // [AuditIgnore] //表示不记录日志
   //[AuditInclude] //表示不记录日志
    public class CompUserRole : BaseEntity
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }

    }
}
