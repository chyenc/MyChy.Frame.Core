using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _service;

        public UnitOfWorkFactory(IServiceProvider service)
        {
            _service = service;
        }

        public TUoW Get<TUoW>() where TUoW : IUnitOfWork
        {
            return (TUoW)_service.GetService(typeof(TUoW));
        }

        public IUnitOfWork Get(Type uowType)
        {
            return (IUnitOfWork)_service.GetService(uowType);
        }

        public void Release(IUnitOfWork unitOfWork)
        {
            var asDisposable = unitOfWork as IDisposable;
            asDisposable?.Dispose();
        }
    }
}
