using MyChy.Frame.Core.EFCore;
using MyChy.Frame.Core.Web5.Domains;

namespace MyChy.Frame.Core.Web5.Work
{
    public  interface ICompetencesWorkArea
    {

        IBaseRepository<CompUser> CompUserR { get; }

        IBaseRepository<CompUserRole> CompUserRoleR { get; }

    }
}
