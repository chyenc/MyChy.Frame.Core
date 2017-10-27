using MyChy.Frame.Core.EFCore.Abstraction.Interceptors;
using MyChy.Frame.Core.EFCore.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore
{
    public class BaseUnitOfWork : EFCoreUnitOfWork, IBaseUnitOfWork
    {
        public BaseUnitOfWork(CoreDbContext context, IEnumerable<IInterceptor> interceptors) 
            : base(context, interceptors)
        {
            DbContext = context;
          //  BaseWorkArea = new BaseWorkArea(context);
        }

        public CoreDbContext DbContext { get; }

        // public IBaseWorkArea BaseWorkArea { get; }


    }

    public interface IBaseUnitOfWork : IEFCoreUnitOfWork
    {
        #region Work Areas

        //IBaseWorkArea BaseWorkArea { get; }

        #endregion
    }
}
