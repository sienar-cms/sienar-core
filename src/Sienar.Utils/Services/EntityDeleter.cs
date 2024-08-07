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
public class EntityDeleter<TEntity> : IEntityDeleter<TEntity>
	where TEntity : EntityBase
{
	private readonly IRepository<TEntity> _repository;
	private readonly INotificationService _notifier;
	private readonly ILogger<EntityDeleter<TEntity>> _logger;
	private readonly IEnumerable<IAccessValidator<TEntity>> _accessValidators;
	private readonly IEnumerable<IStateValidator<TEntity>> _stateValidators;
	private readonly IEnumerable<IBeforeProcess<TEntity>> _beforeHooks;
	private readonly IEnumerable<IAfterProcess<TEntity>> _afterHooks;

	public EntityDeleter(
		IRepository<TEntity> repository,
		INotificationService notifier,
		ILogger<EntityDeleter<TEntity>> logger,
		IEnumerable<IAccessValidator<TEntity>> accessValidators,
		IEnumerable<IStateValidator<TEntity>> stateValidators,
		IEnumerable<IBeforeProcess<TEntity>> beforeHooks,
		IEnumerable<IAfterProcess<TEntity>> afterHooks)
	{
		_repository = repository;
		_notifier = notifier;
		_logger = logger;
		_accessValidators = accessValidators;
		_stateValidators = stateValidators;
		_beforeHooks = beforeHooks;
		_afterHooks = afterHooks;
	}

	public async Task<bool> Delete(Guid id)
	{
		TEntity? entity;
		try
		{
			entity = await _repository.Read(id);
			if (entity is null)
			{
				_notifier.Error(StatusMessages.Crud<TEntity>.NotFound(id));
				return false;
			}
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			_notifier.Error(StatusMessages.Crud<TEntity>.DeleteFailed());
			return false;
		}

		if (!await _accessValidators.Validate(entity, ActionType.Delete, _logger))
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NoPermission());
			return false;
		}

		if (!await _stateValidators.Validate(entity, ActionType.Delete, _logger)
			|| !await _beforeHooks.Run(entity, ActionType.Delete, _logger))
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.DeleteFailed());
			return false;
		}

		try
		{
			await _repository.Delete(entity.Id);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			_notifier.Error(StatusMessages.Crud<TEntity>.DeleteFailed());
			return false;
		}

		await _afterHooks.Run(entity, ActionType.Delete, _logger);
		_notifier.Success(StatusMessages.Crud<TEntity>.DeleteSuccessful());
		return true;
	}
}