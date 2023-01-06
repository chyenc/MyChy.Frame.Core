﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyChy.Frame.Core.Common.Helper;
using MyChy.Frame.Core.Common.Model;
using MyChy.Frame.Core.EFCore.AutoHistorys;
using MyChy.Frame.Core.EFCore.Config;
using MyChy.Frame.Core.EFCore.Entitys.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    /// <summary>
    /// Implementation of an <see cref="IQueryableRepository{TEntity}"/> for the Entity Framework Core
    /// exposing both sync and async operations. It also exposes an <see cref="IQueryable{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TKey">The entity id type</typeparam>
    public abstract class EFCoreQueryableRepository<TEntity, TKey> :
            EFCoreQueryableRepository<TEntity>, IEFCoreQueryableRepository<TEntity, TKey>
        where TEntity : class
    {

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The database context</param>
        protected EFCoreQueryableRepository(DbContext context) : base(context)
        {

        }

        #region Implementation of IAsyncRepository<TEntity,in TKey>

        /// <summary>
        /// Gets an entity by its unique identifier from this repository asynchronously
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that will fetch the entity</returns>
        public async Task<TEntity> GetByIdAsync(TKey id, bool IsNoTracking = false)
        {
            if (IsNoTracking) return await QueryByIdNoTracking(id).SingleOrDefaultAsync();
            return await QueryById(id).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets an entity by its unique identifier from this repository asynchronously
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that will fetch the entity</returns>
        public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken ct, bool IsNoTracking = false)
        {
            if (IsNoTracking) return await QueryByIdNoTracking(id).SingleOrDefaultAsync(ct);
            return await QueryById(id).SingleOrDefaultAsync(ct);
        }

        /// <summary>Checks if an entity with the given key exists</summary>
        /// <param name="id">The entity unique identifier</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>True if entity exists</returns>
        public async Task<bool> ExistsAsync(TKey id, CancellationToken ct = new CancellationToken())
        {
            return await QueryByIdNoTracking(id).AnyAsync(ct);
        }

        #endregion

        #region Implementation of ISyncRepository<TEntity,in TKey>

        /// <summary>
        /// Gets an entity by its unique identifier from this repository
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <returns>The entity or null if not found</returns>
        public TEntity GetById(TKey id, bool IsNoTracking = false)
        {
            if (IsNoTracking) return QueryByIdNoTracking(id).SingleOrDefault();
            return QueryById(id).SingleOrDefault();
        }

        /// <summary>Checks if an entity with the given key exists</summary>
        /// <param name="id">The entity unique identifier</param>
        /// <returns>True if entity exists</returns>
        public bool Exists(TKey id)
        {
            return QueryByIdNoTracking(id).Any();
        }

        #endregion

        #region Implementation of IExposeQueryable<TEntity,in TKey>

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" /> filtered by
        /// the entity id
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <returns>The <see cref="T:System.Linq.IQueryable`1" /> object</returns>
        public abstract IQueryable<TEntity> QueryById(TKey id);


        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" /> filtered by
        /// the entity id
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <returns>The <see cref="T:System.Linq.IQueryable`1" /> object</returns>
        public abstract IQueryable<TEntity> QueryByIdNoTracking(TKey id);

        #endregion

        ///#region Overrides of EFCoreQueryableRepository<TEntity>

        ///// <summary>
        ///// Gets an <see cref="T:System.Linq.IQueryable`1" /> filtered by
        ///// the entity id
        ///// </summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <returns>The <see cref="T:System.Linq.IQueryable`1" /> object</returns>
        //public override IQueryable<TEntity> QueryById(params object[] ids)
        //{
        //    if (ids == null)
        //        throw new ArgumentNullException(nameof(ids));
        //    if (ids.Length != 1)
        //        throw new ArgumentException("Collection must contain one element.", nameof(ids));
        //    return QueryById((TKey)ids[0]);
        //}

        ///#endregion
    }


    /// <summary>
    /// Implementation of an <see cref="IQueryableRepository{TEntity}"/> for the Entity Framework Core
    /// exposing both sync and async operations. It also exposes an <see cref="IQueryable{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public abstract class EFCoreQueryableRepository<TEntity> : IEFCoreQueryableRepository<TEntity>
        where TEntity : class
    {
        private EntityFrameworkConfig efconfig = null;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">The database context</param>
        protected EFCoreQueryableRepository(DbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            Context = context;
            Set = context.Set<TEntity>();
        }

        #region Implementation of IAsyncRepository<TEntity>

        ///// <summary>
        ///// Gets an entity by its unique identifier from this repository asynchronously
        ///// </summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that will fetch the entity</returns>
        //public async Task<TEntity> GetByIdAsync(params object[] ids)
        //{
        //    return await GetByIdAsync(CancellationToken.None, ids);
        //}

        ///// <summary>
        ///// Gets an entity by its unique identifier from this repository asynchronously
        ///// </summary>
        ///// <param name="ids">The entity unique identifier</param>
        ///// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        ///// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that will fetch the entity</returns>
        //public async Task<TEntity> GetByIdAsync(CancellationToken ct, params object[] ids)
        //{
        //    if (ids == null) throw new ArgumentNullException(nameof(ids));
        //    return await QueryById(ids).SingleOrDefaultAsync(ct);
        //}

        /// <summary>Adds the entity to the repository asynchronously</summary>
        /// <param name="entity">The entity to add</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entity</returns>
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = new CancellationToken())
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await Task.FromResult(Add(entity));
        }

        /// <summary>
        /// Adds a range of entities to the repository asynchronously
        /// </summary>
        /// <param name="entities">The entity to add</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities, CancellationToken ct = new CancellationToken())
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return await Task.FromResult(Add(entities));
        }

        /// <summary>
        /// Adds a range of entities to the repository asynchronously
        /// </summary>
        /// <param name="entities">The entity to add</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> AddAsync(params TEntity[] entities)
        {
            return await AddAsync(CancellationToken.None, entities);
        }

        /// <summary>
        /// Adds a range of entities to the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <param name="entities">The entity to add</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> AddAsync(CancellationToken ct, params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return await Task.FromResult(Add(entities));
        }

        /// <summary>
        /// 硬拷贝数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> BulkCopyAsync(IEnumerable<TEntity> entities)
        {

            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count() <= 0) return true;
            var result = false;
            if (efconfig == null)
            {
                efconfig = EntityFrameworkHelper.ReadConfiguration("config/EntityFramework.json");
            }
            if (efconfig.SqlType == EntityFrameworkType.MsSql)
            {
                Type type = typeof(TEntity);
                var tableName = type.Name;
                DataTable dt = ModelHelper.GetTableByListModel<TEntity>(entities, new List<string> { "Id" }, true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    using (var tran = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            result = await BulkCopyAsync(dt, tableName, efconfig.Connect);
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                        }
                        tran.Commit();
                    }

                }
            }
            else
            {
                var res = Add(entities as TEntity[] ?? entities.ToArray());
                result = true;
            }
            return result;

        }
        private async Task<bool> BulkCopyAsync(DataTable dt, string name, string Connect)
        {
            //string constr = System.Configuration.ConfigurationManager.ConnectionStrings["BingStampAzureContext"].ToString();
            //string constr = Context.Database.GetDbConnection().ConnectionString;
            SqlBulkCopy sqlBulkCopy = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(Connect))
                {
                    sqlBulkCopy = new SqlBulkCopy(Connect)
                    {
                        BulkCopyTimeout = 600000,
                        DestinationTableName = string.Format("dbo.{0}", name),
                        BatchSize = dt.Rows.Count
                    };
                    foreach (var col in dt.Columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(col.ToString(), col.ToString());
                    }
                    await sqlBulkCopy.WriteToServerAsync(dt);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlBulkCopy != null) sqlBulkCopy.Close();
            }

        }


        /// <summary>Updates the entity in the repository asynchronously</summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entity</returns>
        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = new CancellationToken())
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await Task.FromResult(Update(entity));
        }

        /// <summary>
        /// Updates a range of entities in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to update</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken ct = new CancellationToken())
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return await Task.FromResult(Update(entities));
        }

        /// <summary>
        /// Updates a range of entities in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to update</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> UpdateAsync(params TEntity[] entities)
        {
            return await UpdateAsync(CancellationToken.None, entities);
        }

        /// <summary>
        /// Updates a range of entities in the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <param name="entities">The entities to update</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> UpdateAsync(CancellationToken ct, params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return await Task.FromResult(Update(entities));
        }

        /// <summary>Deletes the entity in the repository asynchronously</summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entity</returns>
        public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken ct = new CancellationToken())
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return await Task.FromResult(Delete(entity));
        }

        /// <summary>
        /// Deletes a range of entity in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to delete</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken ct = new CancellationToken())
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return await Task.FromResult(Delete(entities));
        }

        /// <summary>
        /// Deletes a range of entity in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to delete</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> DeleteAsync(params TEntity[] entities)
        {
            return await DeleteAsync(CancellationToken.None, entities);
        }

        /// <summary>
        /// Deletes a range of entity in the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <param name="entities">The entities to delete</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the entities</returns>
        public async Task<IEnumerable<TEntity>> DeleteAsync(CancellationToken ct, params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return await Task.FromResult(Delete(entities));
        }

        /// <summary>
        /// Gets the total entities in the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> containing the total</returns>
        public async Task<long> TotalAsync(CancellationToken ct = new CancellationToken())
        {
            return await QueryNoTracking().LongCountAsync(ct);
        }

        ///// <summary>Checks if an entity with the given key exists</summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <returns>True if entity exists</returns>
        //public async Task<bool> ExistsAsync(params object[] ids)
        //{
        //    if (ids == null) throw new ArgumentNullException(nameof(ids));
        //    return await QueryById(ids).AnyAsync();
        //}

        ///// <summary>Checks if an entity with the given key exists</summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        ///// <returns>True if entity exists</returns>
        //public async Task<bool> ExistsAsync(CancellationToken ct, params object[] ids)
        //{
        //    if (ids == null) throw new ArgumentNullException(nameof(ids));
        //    return await QueryById(ids).AnyAsync(ct);
        //}

        #endregion

        #region Implementation of ISyncRepository<TEntity>

        ///// <summary>
        ///// Gets an entity by its unique identifier from this repository
        ///// </summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <returns>The entity or null if not found</returns>
        //public TEntity GetById(params object[] ids)
        //{
        //    if (ids == null) throw new ArgumentNullException(nameof(ids));
        //    return QueryById(ids).SingleOrDefault();
        //}

        /// <summary>Adds the entity to the repository</summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The entity</returns>
        public TEntity Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Set.Add(entity).Entity;
        }

        /// <summary>Adds a range of entities to the repository</summary>
        /// <param name="entities">The entities to add</param>
        /// <returns>The range of entities added</returns>
        public IEnumerable<TEntity> Add(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return Add(entities as TEntity[] ?? entities.ToArray());
        }


        /// <summary>
        /// 硬拷贝数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool BulkCopy(IEnumerable<TEntity> entities)
        {

            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count() <= 0) return true;
            var result = false;
            if (efconfig == null)
            {
                efconfig = EntityFrameworkHelper.ReadConfiguration("config/EntityFramework.json");
            }
            if (efconfig.SqlType == EntityFrameworkType.MsSql)
            {
                Type type = typeof(TEntity);
                var tableName = type.Name;
                DataTable dt = ModelHelper.GetTableByListModel<TEntity>(entities, new List<string> { "Id" },true);

                if (dt != null && dt.Rows.Count > 0)
                {
                    using (var tran = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            result = BulkCopy(dt, tableName, efconfig.Connect);
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();

                        }
                        tran.Commit();
                    }

                }
            }
            else
            {
                var res = Add(entities as TEntity[] ?? entities.ToArray());
                result = true;
            }
            return result;

        }
        private bool BulkCopy(DataTable dt, string name,string Connect)
        {
            //string constr = System.Configuration.ConfigurationManager.ConnectionStrings["BingStampAzureContext"].ToString();
            //string constr = Context.Database.GetDbConnection().ConnectionString;
            SqlBulkCopy sqlBulkCopy = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(Connect))
                {
                    sqlBulkCopy = new SqlBulkCopy(Connect)
                    {
                        BulkCopyTimeout = 600000,
                        DestinationTableName = string.Format("dbo.{0}", name),
                        BatchSize = dt.Rows.Count
                    };
                    foreach (var col in dt.Columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(col.ToString(), col.ToString());
                    }
                    sqlBulkCopy.WriteToServer(dt);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlBulkCopy != null) sqlBulkCopy.Close();
            }

        }

        /// <summary>Adds a range of entities to the repository</summary>
        /// <param name="entities">The entities to add</param>
        /// <returns>The range of entities added</returns>
        public IEnumerable<TEntity> Add(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            Set.AddRange(entities);
            return entities;
        }

        /// <summary>Updates the entity in the repository</summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>The entity</returns>
        public TEntity Update(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Set.Update(entity).Entity;
        }

        /// <summary>Updates a range of entities in the repository</summary>
        /// <param name="entities">The entities to update</param>
        /// <returns>The entities</returns>
        public IEnumerable<TEntity> Update(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return Update(entities as TEntity[] ?? entities.ToArray());
        }

        /// <summary>Updates a range of entities in the repository</summary>
        /// <param name="entities">The entities to update</param>
        /// <returns>The entities</returns>
        public IEnumerable<TEntity> Update(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            Set.UpdateRange(entities);
            return entities;
        }

        /// <summary>Deletes the entity in the repository</summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>The entity</returns>
        public TEntity Delete(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Set.Remove(entity).Entity;
        }

        /// <summary>Deletes a range of entity in the repository</summary>
        /// <param name="entities">The entities to delete</param>
        /// <returns>The entities</returns>
        public IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            return Delete(entities as TEntity[] ?? entities.ToArray());
        }

        /// <summary>Deletes a range of entity in the repository</summary>
        /// <param name="entities">The entities to delete</param>
        /// <returns>The entities</returns>
        public IEnumerable<TEntity> Delete(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            Set.RemoveRange(entities);
            return entities;
        }

        /// <summary>Gets the total entities in the repository</summary>
        /// <returns>The total</returns>
        public long Total()
        {
            return QueryNoTracking().LongCount();
        }

        ///// <summary>Checks if an entity with the given key exists</summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <returns>True if entity exists</returns>
        //public bool Exists(params object[] ids)
        //{
        //    return QueryById(ids).Any();
        //}

        #endregion

        #region Implementation of IExposeQueryable<TEntity>

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" />
        /// </summary>
        /// <param name="query">The Sql String</param>
        /// <param name="parameters">The Sql Parameters</param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
            => Set.FromSqlRaw(query, parameters).AsQueryable();

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" />
        /// </summary>
        /// <returns>The <see cref="T:System.Linq.IQueryable`1" /> object</returns>
        public IQueryable<TEntity> Query()
        {
            return Set;
        }

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" />
        /// </summary>
        /// <returns>The <see cref="T:System.Linq.IQueryable`1" /> object</returns>
        public IQueryable<TEntity> QueryNoTracking()
        {
            return Set.AsNoTracking();
        }

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" /> filtered by
        /// the entity id
        /// </summary>
        /// <param name="ids">The entity unique identifiers</param>
        /// <returns>The <see cref="T:System.Linq.IQueryable`1" /> object</returns>
        //public abstract IQueryable<TEntity> QueryById(params object[] ids);

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" />
        /// that will also fetch, on execution, all the entity navigation properties
        /// </summary>
        /// <param name="propertiesToFetch">The navigation properties to also fetch on query execution</param>
        /// <returns>The <see cref="T:System.Linq.IQueryable`1" /> object</returns>
        public IQueryable<TEntity> QueryFetching(params Expression<Func<TEntity, object>>[] propertiesToFetch)
        {
            if (propertiesToFetch == null) throw new ArgumentNullException(nameof(propertiesToFetch));

            return propertiesToFetch.Aggregate(Query(), (current, expression) => current.Include(expression));
        }

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" />
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IPagedList<TEntity> QueryPage(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int page = 0,
            int pageSize = 15,
            bool IsNoTracking = true)
        {
            var count = 0;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 1;
            var query = QueryFilter(out count, predicate, orderBy, includes, page, pageSize, IsNoTracking);
            return new PagedList<TEntity>(query, page, pageSize, count);
        }

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" />
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public async Task<IPagedList<TEntity>> QueryPageAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int page = 0,
            int pageSize = 15,
            bool IsNoTracking = true)
        {
            var count = 0;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 1;
            var query = QueryFilter(out count, predicate, orderBy, includes, page, pageSize, IsNoTracking);
            var list = await query.ToListAsync();

            return new PagedList<TEntity>(list, page, pageSize, count);
        }

        /// <summary>
        /// Gets an <see cref="T:System.Linq.IQueryable`1" />
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> QueryFilter(out int Count,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null,
            bool IsNoTracking = false)
        {
            IQueryable<TEntity> query = Set;

            if (IsNoTracking)
            {
                query = Set.AsNoTracking();
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            //else
            //{
            //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> IdItemFunc = source => source.OrderByDescending(x => x.PrimaryKey());
            //    query = orderBy(query);
            //}
            if (predicate != null && predicate.Body.ToString() != "False")
            {
                query = query.Where(predicate);
            }
            Count = query.Count();
            if (Count > 0)
            {
                if (page != null && pageSize != null)
                {
                    if (page.Value <= 0) page = 1;
                    if (pageSize.Value <= 0) pageSize = 1;
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }
            }
            return query;
        }


        //public int QueryCount(
        //Expression<Func<TEntity, bool>> predicate = null,
        //List<Expression<Func<TEntity, object>>> includes = null,
        //bool IsNoTracking = true)
        //{
        //    IQueryable<TEntity> query = Set;

        //    if (IsNoTracking)
        //    {
        //        query = Set.AsNoTracking();
        //    }

        //    if (includes != null)
        //    {
        //        query = includes.Aggregate(query, (current, include) => current.Include(include));
        //    }
        //    if (predicate != null && predicate.Body.ToString() != "False")
        //    {
        //        query = query.Where(predicate);
        //    }
        //    var count = query.Count();
        //    return count;
        //}



        #endregion

        #region Implementation of IEFCoreQueryableRepository<TEntity>

        /// <summary>
        /// The Entity Framework database context
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        /// The <see cref="DbSet{TEntity}"/> of this repository entity
        /// </summary>
        public DbSet<TEntity> Set { get; }

        #endregion


        #region Commit of IAsyncRepository<TEntity>


        /// <summary>Checks if an entity with the given key exists</summary>
        /// <param name="ids">The entity unique identifiers</param>
        /// <returns>True if entity exists</returns>
        public async Task<int> CommitAsync()
        {
            return await Context.SaveChangesAsync();
        }

        /// <summary>Checks if an entity with the given key exists</summary>
        /// <param name="ids">The entity unique identifiers</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>True if entity exists</returns>
        public async Task<int> CommitAutoHistoryAsync(string Operator = "SyStem")
        {
            var FullName = typeof(TEntity).FullName;
            Context.EnsureAutoHistory(Operator, FullName);
            return await Context.SaveChangesAsync();

        }


        /// <summary>Checks if an entity with the given key exists</summary>
        /// <param name="ids">The entity unique identifiers</param>
        /// <returns>True if entity exists</returns>
        public int Commit()
        {
            return Context.SaveChanges();
        }



        /// <summary>Checks if an entity with the given key exists</summary>
        /// <param name="ids">The entity unique identifiers</param>
        /// <param name="ct">The <see cref="T:System.Threading.CancellationToken" /> for the returned task</param>
        /// <returns>True if entity exists</returns>
        public int CommitAutoHistory(string Operator = "SyStem")
        {
            var FullName = typeof(TEntity).FullName;
            Context.EnsureAutoHistory(Operator, FullName);
            return Commit();
        }

        #endregion
    }
}
