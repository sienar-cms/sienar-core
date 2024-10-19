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
public class EntityWriter<TEntity> : IEntityWriter<TEntity>
	where TEntity : EntityBase
{
	private readonly IRepository<TEntity> _repository;
	private readonly INotificationService _notifier;
	private readonly ILogger<EntityWriter<TEntity>> _logger;
	private readonly IAccessValidatorService<TEntity> _accessValidator;
	private readonly IEnumerable<IStateValidator<TEntity>> _stateValidators;
	private readonly IEnumerable<IBeforeProcess<TEntity>> _beforeHooks;
	private readonly IEnumerable<IAfterProcess<TEntity>> _afterHooks;

	public EntityWriter(
		IRepository<TEntity> repository,
		INotificationService notifier,
		ILogger<EntityWriter<TEntity>> logger,
		IAccessValidatorService<TEntity> accessValidator,
		IEnumerable<IStateValidator<TEntity>> stateValidators,
		IEnumerable<IBeforeProcess<TEntity>> beforeHooks,
		IEnumerable<IAfterProcess<TEntity>> afterHooks)
	{
		_repository = repository;
		_notifier = notifier;
		_logger = logger;
		_accessValidator = accessValidator;
		_stateValidators = stateValidators;
		_beforeHooks = beforeHooks;
		_afterHooks = afterHooks;
	}

	public async Task<Guid> Create(TEntity model)
	{
		// Run access validation
		var accessResult = await _accessValidator.Validate(model, ActionType.Create);
		if (!accessResult.Result)
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NoPermission());
			return Guid.Empty;
		}

		if (!await _stateValidators.Validate(model, ActionType.Create, _logger)
		|| !await _beforeHooks.Run(model, ActionType.Create, _logger))
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.CreateFailed());
			return Guid.Empty;
		}

		try
		{
			await _repository.Create(model);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			_notifier.Error(StatusMessages.Crud<TEntity>.CreateFailed());
			return Guid.Empty;
		}

		await _afterHooks.Run(model, ActionType.Create, _logger);
		_notifier.Success(StatusMessages.Crud<TEntity>.CreateSuccessful());
		return model.Id;
	}

	public async Task<bool> Update(TEntity model)
	{
		var accessResult = await _accessValidator.Validate(model, ActionType.Update);

		if (!accessResult.Result)
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NoPermission());
			return false;
		}

		if (!await _stateValidators.Validate(model, ActionType.Update, _logger)
		|| !await _beforeHooks.Run(model, ActionType.Update, _logger))
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.UpdateFailed());
			return false;
		}

		try
		{
			await _repository.Update(model);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			_notifier.Error(StatusMessages.Crud<TEntity>.UpdateFailed());
			return false;
		}

		await _afterHooks.Run(model, ActionType.Update, _logger);
		_notifier.Success(StatusMessages.Crud<TEntity>.UpdateSuccessful());
		return true;
	}
}