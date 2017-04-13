using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    /// <summary>
    /// Specialized <see cref="IQueryable{T}"/> for async executions using the Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IEFCoreAsyncQueryable<T> : IAsyncQueryable<T>
    {

    }
}
