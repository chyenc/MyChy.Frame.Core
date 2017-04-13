using System;
using System.Collections.Generic;
using System.Text;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    /// <summary>
    /// Repository interface that will be used to query and manipulate
    /// persisted entities and can be used as an <see cref="IQueryable{T}"/> source
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IQueryableRepository<TEntity>
        : IRepository<TEntity>, IExposeQueryable<TEntity>
        where TEntity : class
    {

    }

    /// <summary>
    /// Repository interface that will be used to query and manipulate
    /// persisted entities and can be used as an <see cref="IQueryable{T}"/> source
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public interface IQueryableRepository<TEntity, in TId>
        : IRepository<TEntity, TId>, IExposeQueryable<TEntity, TId>
        where TEntity : class
    {

    }
}
