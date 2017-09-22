using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.EFCore.Entitys.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    /// <summary>
    /// Specialized interface of an <see cref="IQueryableRepository{TEntity}"/> 
    /// for the Entity Framework Core.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IEFCoreQueryableRepository<TEntity> : IQueryableRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The Entity Framework database context
        /// </summary>
        DbContext Context { get; }

        /// <summary>
        /// The <see cref="DbSet{TEntity}"/> of this repository entity
        /// </summary>
        DbSet<TEntity> Set { get; }

        ///// <summary>
        ///// 
        ///// </summary>
        //CoreDbContext CoreContext { get; }
    }

    /// <summary>
    /// Specialized interface of an <see cref="IQueryableRepository{TEntity,TId}"/> 
    /// for the Entity Framework Core.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TKey">The entity id type</typeparam>
    public interface IEFCoreQueryableRepository<TEntity, in TKey>
        : IQueryableRepository<TEntity, TKey>
        where TEntity : class
    {
        /// <summary>
        /// The Entity Framework database context
        /// </summary>
        DbContext Context { get; }

        /// <summary>
        /// The <see cref="DbSet{TEntity}"/> of this repository entity
        /// </summary>
        DbSet<TEntity> Set { get; }

        ///// <summary>
        ///// 
        ///// </summary>
        //CoreDbContext CoreContext { get; }
    }

}
