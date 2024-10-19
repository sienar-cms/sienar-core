#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Services;

/// <exclude />
public class EntityReader<TEntity> : IEntityReader<TEntity>
	where TEntity : EntityBase, new()
{
	private readonly IRepository<TEntity> _repository;
	private readonly ILogger<EntityReader<TEntity>> _logger;
	private readonly IAccessValidatorService<TEntity> _accessValidator;
	private readonly IAfterProcessService<TEntity> _afterHooks;

	public EntityReader(
		IRepository<TEntity> repository,
		ILogger<EntityReader<TEntity>> logger,
		IAccessValidatorService<TEntity> accessValidator,
		IAfterProcessService<TEntity> afterHooks)
	{
		_repository = repository;
		_logger = logger;
		_accessValidator = accessValidator;
		_afterHooks = afterHooks;
	}

	public async Task<OperationResult<TEntity>> Read(
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
			return new(
				OperationStatus.Unknown,
				default,
				StatusMessages.Crud<TEntity>.ReadSingleFailed());
		}

		if (entity is null)
		{
			return new(
				OperationStatus.NotFound,
				default,
				StatusMessages.Crud<TEntity>.NotFound(id));
		}

		// Run access validation
		var accessValidationResult = await _accessValidator.Validate(entity, ActionType.Read);
		if (!accessValidationResult.Result)
		{
			return new(
				OperationStatus.Unauthorized,
				default,
				StatusMessages.Crud<TEntity>.NoPermission());
		}

		await _afterHooks.Run(entity, ActionType.Read);
		return new(result: entity);
	}

	public async Task<OperationResult<PagedQuery<TEntity>>> Read(Filter? filter = null)
	{
		PagedQuery<TEntity> queryResult;

		try
		{
			queryResult = await _repository.Read(filter);
		}
		catch (Exception e)
		{
			_logger.LogError(e, StatusMessages.Database.QueryFailed);
			return new(
				OperationStatus.Unknown,
				new(),
				StatusMessages.Crud<TEntity>.ReadMultipleFailed());
		}

		foreach (var entity in queryResult.Items)
		{
			await _afterHooks.Run(entity, ActionType.ReadAll);
		}

		return new(result: queryResult);
	}
}