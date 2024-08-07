#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Extensions;
using Sienar.Data;
using Sienar.Hooks;
using Sienar.Infrastructure;

namespace Sienar.Services;

/// <exclude />
public class EntityReader<TEntity> : IEntityReader<TEntity>
	where TEntity : EntityBase, new()
{
	private readonly IRepository<TEntity> _repository;
	private readonly INotificationService _notifier;
	private readonly ILogger<EntityReader<TEntity>> _logger;
	private readonly IEnumerable<IAccessValidator<TEntity>> _accessValidators;
	private readonly IEnumerable<IAfterProcess<TEntity>> _afterHooks;

	public EntityReader(
		IRepository<TEntity> repository,
		INotificationService notifier,
		ILogger<EntityReader<TEntity>> logger,
		IEnumerable<IAccessValidator<TEntity>> accessValidators,
		IEnumerable<IAfterProcess<TEntity>> afterHooks)
	{
		_repository = repository;
		_notifier = notifier;
		_logger = logger;
		_accessValidators = accessValidators;
		_afterHooks = afterHooks;
	}

	public async Task<TEntity?> Read(
		Guid id,
		Filter? filter = null)
	{
		TEntity? entity;
		try
		{
			entity = await _repository.Read(id, filter);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			_notifier.Error(StatusMessages.Crud<TEntity>.ReadSingleFailed());
			return null;
		}

		if (entity is null)
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NotFound(id));
			return null;
		}

		if (!await _accessValidators.Validate(entity, ActionType.Read, _logger))
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NoPermission());
			return null;
		}

		await _afterHooks.Run(entity, ActionType.Read, _logger);
		return entity;
	}

	public async Task<PagedQuery<TEntity>> Read(Filter? filter = null)
	{
		PagedQuery<TEntity> queryResult;

		try
		{
			queryResult = await _repository.Read(filter);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			_notifier.Error(StatusMessages.Crud<TEntity>.ReadMultipleFailed());
			return new();
		}

		foreach (var entity in queryResult.Items)
		{
			await _afterHooks.Run(entity, ActionType.ReadAll, _logger);
		}

		return queryResult;
	}
}