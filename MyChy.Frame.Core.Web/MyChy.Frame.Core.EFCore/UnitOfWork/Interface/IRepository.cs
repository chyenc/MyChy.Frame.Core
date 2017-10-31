using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    /// <summary>
    /// Repository interface that will be used to query and manipulate
    /// persisted entities
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IRepository<TEntity> : IAsyncRepository<TEntity>, ISyncRepository<TEntity>
        where TEntity : class
    {

    }

    /// <summary>
    /// Repository interface that will be used to query and manipulate
    /// persisted entities
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public interface IRepository<TEntity, in TId> : IRepository<TEntity>,
        IAsyncRepository<TEntity, TId>, ISyncRepository<TEntity, TId>
        where TEntity : class
    {

    }
}
