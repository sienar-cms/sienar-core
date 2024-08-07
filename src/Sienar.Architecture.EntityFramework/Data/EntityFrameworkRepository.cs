using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sienar.Hooks;
using Sienar.Processors;

namespace Sienar.Data;

/// <summary>
/// An <see cref="IRepository{TEntity}"/> that supports Entity Framework datastores
/// </summary>
/// <typeparam name="TEntity">the type of the entity</typeparam>
/// <typeparam name="TContext">the type of the database context</typeparam>
public class EntityFrameworkRepository<TEntity, TContext> : IRepository<TEntity>
	where TEntity : EntityBase
	where TContext : DbContext
{
	protected readonly IEntityFrameworkFilterProcessor<TEntity> FilterProcessor;

	/// <summary>
	/// The <see cref="DbContext"/> backing this repository
	/// </summary>
	protected readonly TContext Context;

	/// <summary>
	/// The <see cref="DbSet{TEntity}"/> backing this repository
	/// </summary>
	protected DbSet<TEntity> EntitySet => Context.Set<TEntity>();

	/// <exclude />
	public EntityFrameworkRepository(
		TContext context,
		IEntityFrameworkFilterProcessor<TEntity> filterProcessor)
	{
		Context = context;
		FilterProcessor = filterProcessor;
	}

	/// <inheritdoc />
	public async Task<TEntity?> Read(Guid id, Filter? filter = null)
	{
		filter = FilterProcessor.ModifyFilter(filter, ActionType.Read);
		return filter == null
			? await EntitySet.FindAsync(id)
			: await FilterProcessor
				.ProcessIncludes(EntitySet, filter)
				.FirstOrDefaultAsync(u => u.Id == id);
	}

	/// <inheritdoc />
	public async Task<PagedQuery<TEntity>> Read(Filter? filter = null)
	{
		filter = FilterProcessor.ModifyFilter(filter, ActionType.ReadAll);
		IQueryable<TEntity> entries;
		IQueryable<TEntity> countEntries;

		if (filter is not null)
		{
			entries = ProcessFilter(filter);
			countEntries = FilterProcessor.Search(EntitySet, filter);
		}
		else
		{
			entries = EntitySet;
			countEntries = EntitySet;
		}

		return new(
			await entries.ToListAsync(),
			await countEntries.CountAsync());
	}

	/// <inheritdoc />
	public async Task<Guid> Create(TEntity entity)
	{
		await EntitySet.AddAsync(entity);
		await Context.SaveChangesAsync();
		return entity.Id;
	}

	/// <inheritdoc />
	public async Task<bool> Update(TEntity entity)
	{
		EntitySet.Update(entity);
		await Context.SaveChangesAsync();
		return true;
	}

	/// <inheritdoc />
	public async Task<bool> Delete(Guid id)
	{
		var entity = await EntitySet.FindAsync(id);
		if (entity is null) return false;

		EntitySet.Remove(entity);
		await Context.SaveChangesAsync();
		return true;
	}

	/// <inheritdoc />
	public async Task<Guid?> ReadConcurrencyStamp(Guid id)
		=> await EntitySet
			.Where(e => e.Id == id)
			.Select(e => e.ConcurrencyStamp)
			.FirstOrDefaultAsync();

	/// <summary>
	/// Calls the appropriate methods of an <see cref="IEntityFrameworkFilterProcessor{TEntity}"/> against a <see cref="Filter"/>
	/// </summary>
	/// <param name="filter">the filter</param>
	/// <param name="predicate">an optional additional search predicate</param>
	/// <returns>the modified <see cref="IQueryable{T}"/></returns>
	protected IQueryable<TEntity> ProcessFilter(
		Filter filter,
		Expression<Func<TEntity, bool>>? predicate = null)
	{
		var result = (IQueryable<TEntity>)EntitySet;
		if (predicate is not null)
		{
			result = result.Where(predicate);
		}

		result = FilterProcessor.Search(result, filter);
		result = FilterProcessor.ProcessIncludes(result, filter);
		var sortPredicate = FilterProcessor.GetSortPredicate(filter.SortName);
		result = filter.SortDescending ?? false
			? result.OrderByDescending(sortPredicate)
			: result.OrderBy(sortPredicate);

		if (filter.Page > 1)
		{
			result = result.Skip((filter.Page - 1) * filter.PageSize);
		}

		// If filter.PageSize == 0, return all results
		if (filter.PageSize > 0)
		{
			result = result.Take(filter.PageSize);
		}

		return result;
	}
}