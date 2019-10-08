using MyChy.Frame.Core.EFCore;
using MyChy.Frame.Core.Web.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.Web.Work
{
    public class CompetencesWorkArea : EFCoreWorkArea<CoreDbContext>, ICompetencesWorkArea
    {
        public CompetencesWorkArea(CoreDbContext context) : base(context)
        {
            CompUserR = new BaseRepository<CompUser>(context);

            CompUserRoleR = new BaseRepository<CompUserRole>(context);
        }

        public IBaseRepository<CompUser> CompUserR { get; }

        public IBaseRepository<CompUserRole> CompUserRoleR { get; }


    }

}
