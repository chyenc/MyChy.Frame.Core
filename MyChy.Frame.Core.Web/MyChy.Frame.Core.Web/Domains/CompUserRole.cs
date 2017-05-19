using MyChy.Frame.Core.EFCore.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web.Domains
{
    public class CompUserRole : BaseEntity
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }

    }
}
