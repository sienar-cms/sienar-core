#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
	private readonly IStateValidatorService<TEntity> _stateValidator;
	private readonly IBeforeProcessService<TEntity> _beforeHooks;
	private readonly IAfterProcessService<TEntity> _afterHooks;

	public EntityWriter(
		IRepository<TEntity> repository,
		INotificationService notifier,
		ILogger<EntityWriter<TEntity>> logger,
		IAccessValidatorService<TEntity> accessValidator,
		IStateValidatorService<TEntity> stateValidator,
		IBeforeProcessService<TEntity> beforeHooks,
		IAfterProcessService<TEntity> afterHooks)
	{
		_repository = repository;
		_notifier = notifier;
		_logger = logger;
		_accessValidator = accessValidator;
		_stateValidator = stateValidator;
		_beforeHooks = beforeHooks;
		_afterHooks = afterHooks;
	}

	public async Task<Guid> Create(TEntity model)
	{
		// Run access validation
		var accessValidationResult = await _accessValidator.Validate(model, ActionType.Create);
		if (!accessValidationResult.Result)
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NoPermission());
			return Guid.Empty;
		}

		// Run state validation
		var stateValidationResult = await _stateValidator.Validate(model, ActionType.Create);
		if (!stateValidationResult.Result)
		{
			if (!string.IsNullOrEmpty(stateValidationResult.Message))
			{
				_notifier.Error(stateValidationResult.Message);
			}

			return Guid.Empty;
		}

		// Run before hooks
		var beforeHooksResult = await _beforeHooks.Run(model, ActionType.Create);
		if (!beforeHooksResult.Result)
		{
			if (!string.IsNullOrEmpty(beforeHooksResult.Message))
			{
				_notifier.Error(beforeHooksResult.Message);
			}

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

		// Run after hooks
		await _afterHooks.Run(model, ActionType.Create);

		_notifier.Success(StatusMessages.Crud<TEntity>.CreateSuccessful());
		return model.Id;
	}

	public async Task<bool> Update(TEntity model)
	{
		// Run access validation
		var accessValidationResult = await _accessValidator.Validate(model, ActionType.Update);
		if (!accessValidationResult.Result)
		{
			_notifier.Error(StatusMessages.Crud<TEntity>.NoPermission());
			return false;
		}

		// Run state validation
		var stateValidationResult = await _stateValidator.Validate(model, ActionType.Update);
		if (!stateValidationResult.Result)
		{
			if (!string.IsNullOrEmpty(stateValidationResult.Message))
			{
				_notifier.Error(stateValidationResult.Message);
			}

			return false;
		}

		// Run before hooks
		var beforeHooksResult = await _beforeHooks.Run(model, ActionType.Update);
		if (!beforeHooksResult.Result)
		{
			if (!string.IsNullOrEmpty(beforeHooksResult.Message))
			{
				_notifier.Error(beforeHooksResult.Message);
			}

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

		// Run after hooks
		await _afterHooks.Run(model, ActionType.Update);

		_notifier.Success(StatusMessages.Crud<TEntity>.UpdateSuccessful());
		return true;
	}
}