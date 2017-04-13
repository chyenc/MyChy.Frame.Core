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
          //  BaseWorkArea = new BaseWorkArea(context);
        }

       // public IBaseWorkArea BaseWorkArea { get; }


    }

    public interface IBaseUnitOfWork : IEFCoreUnitOfWork
    {
        #region Work Areas

        //IBaseWorkArea BaseWorkArea { get; }

        #endregion
    }
}
