using MyChy.Frame.Core.EFCore;
using MyChy.Frame.Core.Web.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web.Work
{
   public  interface ICompetencesWorkArea
    {

        IBaseRepository<CompUser> CompUserR { get; }

        IBaseRepository<CompUserRole> CompUserRoleR { get; }

    }
}
