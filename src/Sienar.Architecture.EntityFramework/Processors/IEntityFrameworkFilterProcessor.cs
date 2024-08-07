using System;
using System.Linq;
using System.Linq.Expressions;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Processors;

/// <summary>
/// A processor which converts a <see cref="Filter"/> into EF queries for the given <c>TEntity</c>
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IEntityFrameworkFilterProcessor<TEntity>
{
	/// <summary>
	/// Applies included entities to the <see cref="IQueryable"/>
	/// </summary>
	/// <param name="dataset">The <see cref="IQueryable{TEntity}"/> containing the current query</param>
	/// <param name="filter">The <see cref="Filter"/></param>
	/// <returns>the updated <see cref="IQueryable{TEntity}"/></returns>
	IQueryable<TEntity> ProcessIncludes(IQueryable<TEntity> dataset, Filter filter) => dataset;

	/// <summary>
	/// Performs a search of the <see cref="IQueryable{TEntity}"/>
	/// </summary>
	/// <param name="dataset">The <see cref="IQueryable{TEntity}"/> containing the current query</param>
	/// <param name="filter">The <see cref="Filter"/></param>
	/// <returns>the updated <see cref="IQueryable{TEntity}"/></returns>
	IQueryable<TEntity> Search(IQueryable<TEntity> dataset, Filter filter);

	/// <summary>
	/// Creates a predicate for use in sorting by columns
	/// </summary>
	/// <param name="sortName">The name of the column to sort by</param>
	/// <returns>the sort predicate</returns>
	Expression<Func<TEntity, object>> GetSortPredicate(string? sortName);

	/// <summary>
	/// Performs modifications to a filter prior to accessing the database
	/// </summary>
	/// <param name="filter">The existing filter, if any</param>
	/// <param name="action">The <see cref="ActionType">action type</see></param>
	/// <returns></returns>
	Filter? ModifyFilter(Filter? filter, ActionType action) => filter;
}