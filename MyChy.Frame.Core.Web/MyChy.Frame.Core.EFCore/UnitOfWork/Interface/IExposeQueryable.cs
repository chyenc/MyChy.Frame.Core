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
    /// Offers the possibility of exposing an <see cref="IQueryable{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IExposeQueryable<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/>
        /// </summary>
        /// <param name="query">The Sql String</param>
        /// <param name="parameters">The Sql Parameters</param>
        /// <returns></returns>
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);

        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/>
        /// </summary>
        /// <returns>The <see cref="IQueryable{TEntity}"/> object</returns>
        IQueryable<TEntity> Query();

        /// <summary>
        ///  Gets an <see cref="IQueryable{TEntity}"/>
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> QueryNoTracking();

        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/> filtered by
        /// the entity id
        /// </summary>
        /// <param name="ids">The entity unique identifiers</param>
        /// <returns>The <see cref="IQueryable{TEntity}"/> object</returns>
        IQueryable<TEntity> QueryById(params object[] ids);

        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/> 
        /// that will also fetch, on execution, all the entity navigation properties
        /// </summary>
        /// <param name="propertiesToFetch">The navigation properties to also fetch on query execution</param>
        /// <returns>The <see cref="IQueryable{TEntity}"/> object</returns>
        IQueryable<TEntity> QueryFetching(params Expression<Func<TEntity, object>>[] propertiesToFetch);


        IPagedList<TEntity> QueryPage(
           Expression<Func<TEntity, bool>> predicate = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           List<Expression<Func<TEntity, object>>> includes = null,
           int page = 0,
           int pageSize = 15,
           bool IsNoTracking = false);

        /// <summary>
        ///  Gets an <see cref="IQueryable{TEntity}"/> 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<TEntity> QueryFilter(
             Expression<Func<TEntity, bool>> predicate = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             List<Expression<Func<TEntity, object>>> includes = null,
             int? page = null,
             int? pageSize = null,
             bool IsNoTracking = false);
    }

    /// <summary>
    /// Offers the possibility of exposing an <see cref="IQueryable{T}"/>
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public interface IExposeQueryable<TEntity, in TId>
        where TEntity : class
    {
        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/>
        /// </summary>
        /// <param name="query">The Sql String</param>
        /// <param name="parameters">The Sql Parameters</param>
        /// <returns></returns>
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);

        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/>
        /// </summary>
        /// <returns>The <see cref="IQueryable{TEntity}"/> object</returns>
        IQueryable<TEntity> Query();

        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/> filtered by
        /// the entity id
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <returns>The <see cref="IQueryable{TEntity}"/> object</returns>
        IQueryable<TEntity> QueryById(TId id);

        /// <summary>
        /// Gets an <see cref="IQueryable{TEntity}"/> 
        /// that will also fetch, on execution, all the entity navigation properties
        /// </summary>
        /// <param name="propertiesToFetch">The navigation properties to also fetch on query execution</param>
        /// <returns>The <see cref="IQueryable{TEntity}"/> object</returns>
        IQueryable<TEntity> QueryFetching(params Expression<Func<TEntity, object>>[] propertiesToFetch);

        /// <summary>
        ///  Gets an <see cref="IQueryable{TEntity}"/> 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<TEntity> QueryPage(
           Expression<Func<TEntity, bool>> predicate = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           List<Expression<Func<TEntity, object>>> includes = null,
           int page = 0,
           int pageSize = 15,
           bool IsNoTracking = false);

        /// <summary>
        ///  Gets an <see cref="IQueryable{TEntity}"/> 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IPagedList<TEntity>> QueryPageAsync(
         Expression<Func<TEntity, bool>> predicate = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          List<Expression<Func<TEntity, object>>> includes = null,
          int page = 0,
          int pageSize = 15,
          bool IsNoTracking = false);


        /// <summary>
        ///  Gets an <see cref="IQueryable{TEntity}"/> 
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<TEntity> QueryFilter(
             Expression<Func<TEntity, bool>> predicate = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             List<Expression<Func<TEntity, object>>> includes = null,
             int? page = null,
             int? pageSize = null,
             bool IsNoTracking = false);
    }
}
