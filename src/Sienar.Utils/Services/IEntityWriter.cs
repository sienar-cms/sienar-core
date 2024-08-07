using System;
using System.Threading.Tasks;

namespace Sienar.Services;

// ReSharper disable once TypeParameterCanBeVariant
/// <summary>
/// A service to write or update instances of <c>TEntity</c> in the appropriate repository
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityWriter<TEntity>
{
	/// <summary>
	/// Creates a new entry in the database
	/// </summary>
	/// <param name="model">The entity to create</param>
	/// <returns>the <see cref="Guid"/> representing the entity's primary key</returns>
	Task<Guid> Create(TEntity model);

	/// <summary>
	/// Updates an existing entity in the database
	/// </summary>
	/// <param name="model">The entity to update</param>
	/// <returns>whether the edit operation was successful</returns>
	Task<bool> Update(TEntity model);
}