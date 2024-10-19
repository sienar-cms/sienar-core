using System;
using System.Threading.Tasks;
using Sienar.Data;

namespace Sienar.Services;

/// <summary>
/// A service to read instances of <c>TEntity</c> from the appropriate repository
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityReader<TEntity>
{
	/// <summary>
	/// Gets an entity by primary key
	/// </summary>
	/// <param name="id">The primary key of the entity to retrieve</param>
	/// <param name="filter">A <see cref="Filter"/> to specify included results</param>
	/// <returns>the requested entity</returns>
	Task<OperationResult<TEntity>> Read(
		Guid id,
		Filter? filter = null);

	/// <summary>
	/// Gets a list of all entities in the backend
	/// </summary>
	/// <param name="filter">A <see cref="Filter"/> to specify included results</param>
	/// <returns>a list of all entities in the database</returns>
	Task<OperationResult<PagedQuery<TEntity>>> Read(Filter? filter = null);
}