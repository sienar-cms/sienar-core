#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Sienar.Data;
using Sienar.Infrastructure;

namespace Sienar.Hooks;

/// <exclude />
public class ConcurrencyStampValidator<TEntity> : IStateValidator<TEntity>
	where TEntity : EntityBase
{
	private readonly IRepository<TEntity> _repository;
	private readonly INotificationService _notifier;

	public ConcurrencyStampValidator(
		IRepository<TEntity> repository,
		INotificationService notifier)
	{
		_repository = repository;
		_notifier = notifier;
	}

	public async Task<OperationStatus> Validate(TEntity request, ActionType action)
	{
		// Only run on update
		if (action is not ActionType.Update) return OperationStatus.Success;

		var concurrencyStamp = await _repository.ReadConcurrencyStamp(request.Id);

		if (concurrencyStamp == Guid.Empty
			|| concurrencyStamp != request.ConcurrencyStamp)
		{
			_notifier.Error(
				$"Unable to update {typeof(TEntity).Name}: the entity has been updated by another user.");
			return OperationStatus.Conflict;
		}

		return OperationStatus.Success;
	}
}