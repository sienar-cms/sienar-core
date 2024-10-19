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
	private readonly IAccessValidatorService<TEntity> _accessValidator;
	private readonly IStateValidatorService<TEntity> _stateValidator;
	private readonly IEnumerable<IBeforeProcess<TEntity>> _beforeHooks;
	private readonly IEnumerable<IAfterProcess<TEntity>> _afterHooks;

	public EntityDeleter(
		IRepository<TEntity> repository,
		INotificationService notifier,
		ILogger<EntityDeleter<TEntity>> logger,
		IAccessValidatorService<TEntity> accessValidator,
		IStateValidatorService<TEntity> stateValidator,
		IEnumerable<IBeforeProcess<TEntity>> beforeHooks,
		IEnumerable<IAfterProcess<TEntity>> afterHooks)
	{
		_repository = repository;
		_notifier = notifier;
		_logger = logger;
		_accessValidator = accessValidator;
		_stateValidator = stateValidator;
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

		// Run access validation
		var accessValidationResult = await _accessValidator.Validate(entity, ActionType.Delete);
		if (!accessValidationResult.Result)
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NoPermission());
			return false;
		}

		// Run state validation
		var stateValidationResult = await _stateValidator.Validate(entity, ActionType.Delete);
		if (!stateValidationResult.Result)
		{
			if (!string.IsNullOrEmpty(stateValidationResult.Message))
			{
				_notifier.Error(stateValidationResult.Message);
			}

			return false;
		}

		if (!await _beforeHooks.Run(entity, ActionType.Delete, _logger))
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