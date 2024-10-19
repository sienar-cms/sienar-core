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

	public async Task<OperationResult<Guid>> Create(TEntity model)
	{
		// Run access validation
		var accessValidationResult = await _accessValidator.Validate(model, ActionType.Create);
		if (!accessValidationResult.Result)
		{
			return new(
				OperationStatus.Unauthorized,
				default,
				StatusMessages.Crud<TEntity>.NoPermission());
		}

		// Run state validation
		var stateValidationResult = await _stateValidator.Validate(model, ActionType.Create);
		if (!stateValidationResult.Result)
		{
			if (!string.IsNullOrEmpty(stateValidationResult.Message))
			{
				// Notify of this message because if it exists,
				// the user needs to know in this case
				_notifier.Error(stateValidationResult.Message);
			}

			return new(
				OperationStatus.Unprocessable,
				default,
				StatusMessages.Crud<TEntity>.CreateFailed());
		}

		// Run before hooks
		var beforeHooksResult = await _beforeHooks.Run(model, ActionType.Create);
		if (!beforeHooksResult.Result)
		{
			if (!string.IsNullOrEmpty(beforeHooksResult.Message))
			{
				// Notify of this message because if it exists,
				// the user needs to know in this case
				_notifier.Error(beforeHooksResult.Message);
			}

			return new(
				OperationStatus.Unknown,
				default,
				StatusMessages.Crud<TEntity>.CreateFailed());
		}

		try
		{
			await _repository.Create(model);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			return new(
				OperationStatus.Unknown,
				default,
				StatusMessages.Crud<TEntity>.CreateFailed());
		}

		// Run after hooks
		await _afterHooks.Run(model, ActionType.Create);

		return new(
			OperationStatus.Success,
			model.Id,
			StatusMessages.Crud<TEntity>.CreateSuccessful());
	}

	public async Task<OperationResult<bool>> Update(TEntity model)
	{
		// Run access validation
		var accessValidationResult = await _accessValidator.Validate(model, ActionType.Update);
		if (!accessValidationResult.Result)
		{
			return new(
				OperationStatus.Unauthorized,
				false,
				StatusMessages.Crud<TEntity>.NoPermission());
		}

		// Run state validation
		var stateValidationResult = await _stateValidator.Validate(model, ActionType.Update);
		if (!stateValidationResult.Result)
		{
			if (!string.IsNullOrEmpty(stateValidationResult.Message))
			{
				// Notify of this message because if it exists,
				// the user needs to know in this case
				_notifier.Error(stateValidationResult.Message);
			}

			return new(
				OperationStatus.Unprocessable,
				false,
				StatusMessages.Crud<TEntity>.UpdateFailed());
		}

		// Run before hooks
		var beforeHooksResult = await _beforeHooks.Run(model, ActionType.Update);
		if (!beforeHooksResult.Result)
		{
			if (!string.IsNullOrEmpty(beforeHooksResult.Message))
			{
				// Notify of this message because if it exists,
				// the user needs to know in this case
				_notifier.Error(beforeHooksResult.Message);
			}

			return new(
				OperationStatus.Unknown,
				false,
				StatusMessages.Crud<TEntity>.UpdateFailed());
		}

		try
		{
			await _repository.Update(model);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			return new(
				OperationStatus.Unknown,
				false,
				StatusMessages.Crud<TEntity>.UpdateFailed());
		}

		// Run after hooks
		await _afterHooks.Run(model, ActionType.Update);

		return new(
			OperationStatus.Success,
			true,
			StatusMessages.Crud<TEntity>.UpdateSuccessful());
	}
}