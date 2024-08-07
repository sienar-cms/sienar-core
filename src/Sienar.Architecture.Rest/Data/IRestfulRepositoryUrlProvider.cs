using System;

namespace Sienar.Data;

/// <summary>
/// Provides CRUD endpoints for RESTful repositories
/// </summary>
/// <typeparam name="TEntity">the type of the entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IRestfulRepositoryUrlProvider<TEntity>
{
	/// <summary>
	/// Generates a URL to perform a read operation on a single entity
	/// </summary>
	/// <param name="id">the ID of the entity to read</param>
	/// <returns>the URL</returns>
	string GenerateReadUrl(Guid id);

	/// <summary>
	/// Generates a URL to perform a read operation on multiple entities
	/// </summary>
	/// <returns>the URL</returns>
	string GenerateReadUrl();

	/// <summary>
	/// Generates a URL to perform a create operation on an entity
	/// </summary>
	/// <param name="entity">the entity to create</param>
	/// <returns>the URL</returns>
	string GenerateCreateUrl(TEntity entity);

	/// <summary>
	/// Generates a URL to perform an update operation on an entity
	/// </summary>
	/// <param name="entity">the entity to update</param>
	/// <returns>the URL</returns>
	string GenerateUpdateUrl(TEntity entity);

	/// <summary>
	/// Generates a URL to perform a delete operation on an entity
	/// </summary>
	/// <param name="id">the ID of the entity to delete</param>
	/// <returns>the URL</returns>
	string GenerateDeleteUrl(Guid id);
}