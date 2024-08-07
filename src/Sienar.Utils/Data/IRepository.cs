using System;
using System.Threading.Tasks;

namespace Sienar.Data;

/// <summary>
/// Exposes methods for storing, updating, and retrieving instances of <c>TEntity</c> from a datastore
/// </summary>
/// <typeparam name="TEntity">the type of the entity</typeparam>
public interface IRepository<TEntity>
{
	/// <summary>
	/// Reads an entity from the datastore
	/// </summary>
	/// <param name="id">the ID of the entity</param>
	/// <param name="filter">a <see cref="Filter"/> to to specify included results</param>
	/// <returns>the entity if it exists, else <c>null</c></returns>
	Task<TEntity?> Read(Guid id, Filter? filter = null);

	/// <summary>
	/// Reads all entities from the datastore that satisfy the provided filter
	/// </summary>
	/// <param name="filter">a <see cref="Filter"/> to specify included results</param>
	/// <returns>a <see cref="PagedQuery{TModel}"/> of entities matching the provided filter</returns>
	Task<PagedQuery<TEntity>> Read(Filter? filter = null);

	/// <summary>
	/// Creates a new entity in the datastore
	/// </summary>
	/// <param name="entity">the entity to create</param>
	/// <returns>the entity's primary key</returns>
	Task<Guid> Create(TEntity entity);

	/// <summary>
	/// Updates an existing entity in the datastore
	/// </summary>
	/// <param name="entity">the entity to update</param>
	/// <returns>whether the edit operation was successful</returns>
	Task<bool> Update(TEntity entity);

	/// <summary>
	/// Deletes an entity from the datastore
	/// </summary>
	/// <param name="id">the primary key of the entity to delete</param>
	/// <returns>whether the delete operation was successful</returns>
	Task<bool> Delete(Guid id);

	/// <summary>
	/// Reads the concurrency stamp for the entity with the given ID
	/// </summary>
	/// <param name="id">the ID of the entity</param>
	/// <returns>the concurrency stamp of the entity if it exists, else <c>null</c></returns>
	Task<Guid?> ReadConcurrencyStamp(Guid id);
}