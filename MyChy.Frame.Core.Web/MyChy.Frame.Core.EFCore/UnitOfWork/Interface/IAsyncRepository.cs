using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyChy.Frame.Core.EFCore.UnitOfWork
{
    /// <summary>
    /// Represents a repository that only exposes asynchronous operations 
    /// to manipulate persisted entities
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public interface IAsyncRepository<TEntity> : ISimplePersistenceRepository
        where TEntity : class
    {
        //#region GetById

        ///// <summary>
        ///// Gets an entity by its unique identifier from this repository asynchronously
        ///// </summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <returns>A <see cref="Task{TResult}"/> that will fetch the entity</returns>
        //Task<TEntity> GetByIdAsync(params object[] ids);

        ///// <summary>
        ///// Gets an entity by its unique identifier from this repository asynchronously
        ///// </summary>
        ///// <param name="ids">The entity unique identifier</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> that will fetch the entity</returns>
        //Task<TEntity> GetByIdAsync(CancellationToken ct, params object[] ids);

        //#endregion

        #region Add

        /// <summary>
        /// Adds the entity to the repository asynchronously
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entity</returns>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Adds a range of entities to the repository asynchronously
        /// </summary>
        /// <param name="entities">The entity to add</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> AddAsync(
            IEnumerable<TEntity> entities, CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Adds a range of entities to the repository asynchronously
        /// </summary>
        /// <param name="entities">The entity to add</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> AddAsync(params TEntity[] entities);

        /// <summary>
        /// Adds a range of entities to the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <param name="entities">The entity to add</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> AddAsync(CancellationToken ct, params TEntity[] entities);

        #endregion

        #region Update

        /// <summary>
        /// Updates the entity in the repository asynchronously
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entity</returns>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Updates a range of entities in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to update</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> UpdateAsync(
            IEnumerable<TEntity> entities, CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Updates a range of entities in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to update</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> UpdateAsync(params TEntity[] entities);

        /// <summary>
        /// Updates a range of entities in the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <param name="entities">The entities to update</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> UpdateAsync(CancellationToken ct, params TEntity[] entities);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the entity in the repository asynchronously
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entity</returns>
        Task<TEntity> DeleteAsync(TEntity entity, CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Deletes a range of entity in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to delete</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> DeleteAsync(
            IEnumerable<TEntity> entities, CancellationToken ct = default(CancellationToken));

        /// <summary>
        /// Deletes a range of entity in the repository asynchronously
        /// </summary>
        /// <param name="entities">The entities to delete</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> DeleteAsync(params TEntity[] entities);

        /// <summary>
        /// Deletes a range of entity in the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <param name="entities">The entities to delete</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        Task<IEnumerable<TEntity>> DeleteAsync(CancellationToken ct, params TEntity[] entities);

        #endregion

        #region Total

        /// <summary>
        /// Gets the total entities in the repository asynchronously
        /// </summary>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the total</returns>
        Task<long> TotalAsync(CancellationToken ct = default(CancellationToken));

        #endregion

        #region Exists

        ///// <summary>
        ///// Checks if an entity with the given key exists
        ///// </summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <returns>True if entity exists</returns>
        //Task<bool> ExistsAsync(params object[] ids);

        ///// <summary>
        ///// Checks if an entity with the given key exists
        ///// </summary>
        ///// <param name="ids">The entity unique identifiers</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>True if entity exists</returns>
        //Task<bool> ExistsAsync(CancellationToken ct, params object[] ids);

        #endregion

        #region Commit

        /// <summary>
        /// CommitAsync entity
        /// </summary>
        /// <returns>True if entity exists</returns>
        Task<int> CommitAsync();

        /// <summary>
        /// CommitAsync entity
        /// </summary>
        /// <returns>True if entity exists</returns>
        Task<int> CommitAutoHistoryAsync(string Operator = "SyStem");

        #endregion
    }

    /// <summary>
    /// Represents a repository that only exposes asynchronous operations 
    /// to manipulate persisted entities
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public interface IAsyncRepository<TEntity, in TId> : ISimplePersistenceRepository
        where TEntity : class
    {
        #region GetById


        /// <summary>
        /// Gets an entity by its unique identifier from this repository asynchronously
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <returns>A <see cref="Task"/> that will fetch the entity</returns>
        Task<TEntity> GetByIdAsync(TId id, bool IsNoTracking = false);

        /// <summary>
        /// Gets an entity by its unique identifier from this repository asynchronously
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>A <see cref="Task{TResult}"/> that will fetch the entity</returns>
        Task<TEntity> GetByIdAsync(TId id, CancellationToken ct, bool IsNoTracking = false);

        #endregion

        //#region Add

        ///// <summary>
        ///// Adds the entity to the repository asynchronously
        ///// </summary>
        ///// <param name="entity">The entity to add</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entity</returns>
        //Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default(CancellationToken));

        ///// <summary>
        ///// Adds a range of entities to the repository asynchronously
        ///// </summary>
        ///// <param name="entities">The entity to add</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> AddAsync(
        //    IEnumerable<TEntity> entities, CancellationToken ct = default(CancellationToken));

        ///// <summary>
        ///// Adds a range of entities to the repository asynchronously
        ///// </summary>
        ///// <param name="entities">The entity to add</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> AddAsync(params TEntity[] entities);

        ///// <summary>
        ///// Adds a range of entities to the repository asynchronously
        ///// </summary>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <param name="entities">The entity to add</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> AddAsync(CancellationToken ct, params TEntity[] entities);

        //#endregion

        //#region Update

        ///// <summary>
        ///// Updates the entity in the repository asynchronously
        ///// </summary>
        ///// <param name="entity">The entity to update</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entity</returns>
        //Task<TEntity> UpdateAsync(TEntity entity, CancellationToken ct = default(CancellationToken));

        ///// <summary>
        ///// Updates a range of entities in the repository asynchronously
        ///// </summary>
        ///// <param name="entities">The entities to update</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> UpdateAsync(
        //    IEnumerable<TEntity> entities, CancellationToken ct = default(CancellationToken));

        ///// <summary>
        ///// Updates a range of entities in the repository asynchronously
        ///// </summary>
        ///// <param name="entities">The entities to update</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> UpdateAsync(params TEntity[] entities);

        ///// <summary>
        ///// Updates a range of entities in the repository asynchronously
        ///// </summary>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <param name="entities">The entities to update</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> UpdateAsync(CancellationToken ct, params TEntity[] entities);

        //#endregion

        //#region Delete

        ///// <summary>
        ///// Deletes the entity in the repository asynchronously
        ///// </summary>
        ///// <param name="entity">The entity to delete</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entity</returns>
        //Task<TEntity> DeleteAsync(TEntity entity, CancellationToken ct = default(CancellationToken));

        ///// <summary>
        ///// Deletes a range of entity in the repository asynchronously
        ///// </summary>
        ///// <param name="entities">The entities to delete</param>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> DeleteAsync(
        //    IEnumerable<TEntity> entities, CancellationToken ct = default(CancellationToken));

        ///// <summary>
        ///// Deletes a range of entity in the repository asynchronously
        ///// </summary>
        ///// <param name="entities">The entities to delete</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> DeleteAsync(params TEntity[] entities);

        ///// <summary>
        ///// Deletes a range of entity in the repository asynchronously
        ///// </summary>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <param name="entities">The entities to delete</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the entities</returns>
        //Task<IEnumerable<TEntity>> DeleteAsync(CancellationToken ct, params TEntity[] entities);

        //#endregion

        //#region Total

        ///// <summary>
        ///// Gets the total entities in the repository asynchronously
        ///// </summary>
        ///// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        ///// <returns>A <see cref="Task{TResult}"/> containing the total</returns>
        //Task<long> TotalAsync(CancellationToken ct = default(CancellationToken));

        //#endregion

        #region Exists

        /// <summary>
        /// Checks if an entity with the given key exists
        /// </summary>
        /// <param name="id">The entity unique identifier</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for the returned task</param>
        /// <returns>True if entity exists</returns>
        Task<bool> ExistsAsync(TId id, CancellationToken ct = default(CancellationToken));

        #endregion

        //#region Commit

        ///// <summary>
        ///// CommitAsync entity
        ///// </summary>
        ///// <returns>True if entity exists</returns>
        //Task<int> CommitAsync();

        ///// <summary>
        ///// CommitAsync entity
        ///// </summary>
        ///// <returns>True if entity exists</returns>
        //Task<int> CommitAutoHistoryAsync(string Operator = "SyStem", bool IsThis = true);

        //#endregion
    }
}
