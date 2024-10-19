#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Services;

/// <exclude />
public class EntityDeleter<TEntity> : IEntityDeleter<TEntity>
	where TEntity : EntityBase
{
	private readonly IRepository<TEntity> _repository;
	private readonly ILogger<EntityDeleter<TEntity>> _logger;
	private readonly IAccessValidatorService<TEntity> _accessValidator;
	private readonly IStateValidatorService<TEntity> _stateValidator;
	private readonly IBeforeProcessService<TEntity> _beforeHooks;
	private readonly IAfterProcessService<TEntity> _afterHooks;

	public EntityDeleter(
		IRepository<TEntity> repository,
		ILogger<EntityDeleter<TEntity>> logger,
		IAccessValidatorService<TEntity> accessValidator,
		IStateValidatorService<TEntity> stateValidator,
		IBeforeProcessService<TEntity> beforeHooks,
		IAfterProcessService<TEntity> afterHooks)
	{
		_repository = repository;
		_logger = logger;
		_accessValidator = accessValidator;
		_stateValidator = stateValidator;
		_beforeHooks = beforeHooks;
		_afterHooks = afterHooks;
	}

	public async Task<OperationResult<bool>> Delete(Guid id)
	{
		TEntity? entity;
		try
		{
			entity = await _repository.Read(id);
			if (entity is null)
			{
				return new(
					OperationStatus.NotFound,
					false,
					StatusMessages.Crud<TEntity>.NotFound(id));
			}
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			return new(
				OperationStatus.Unknown,
				false,
				StatusMessages.Crud<TEntity>.DeleteFailed());
		}

		// Run access validation
		var accessValidationResult = await _accessValidator.Validate(entity, ActionType.Delete);
		if (!accessValidationResult.Result)
		{
			return new(
				OperationStatus.Unauthorized,
				false,
				StatusMessages.Crud<TEntity>.NoPermission());
		}

		// Run state validation
		var stateValidationResult = await _stateValidator.Validate(entity, ActionType.Delete);
		if (!stateValidationResult.Result)
		{
			return new(
				OperationStatus.Unprocessable,
				false,
				stateValidationResult.Message ?? StatusMessages.Crud<TEntity>.DeleteFailed());
		}

		// Run before hooks
		var beforeHooksResult = await _beforeHooks.Run(entity, ActionType.Delete);
		if (!beforeHooksResult.Result)
		{
			return new(
				OperationStatus.Unknown,
				false,
				beforeHooksResult.Message ?? StatusMessages.Crud<TEntity>.DeleteFailed());
		}

		try
		{
			await _repository.Delete(entity.Id);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			return new(
				OperationStatus.Unknown,
				false,
				StatusMessages.Crud<TEntity>.DeleteFailed());
		}

		// Run after hooks
		await _afterHooks.Run(entity, ActionType.Delete);

		return new(
			OperationStatus.Success,
			true,
			StatusMessages.Crud<TEntity>.DeleteSuccessful());
	}
}