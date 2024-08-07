#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Sienar.Data;

namespace Sienar.Hooks;

/// <exclude />
public class ConcurrencyStampUpdateHook<TEntity> : IBeforeProcess<TEntity>
	where TEntity : EntityBase
{
	public Task Handle(TEntity entity, ActionType action)
	{
		if (action is ActionType.Create or ActionType.Update)
		{
			entity.ConcurrencyStamp = Guid.NewGuid();
		}

		return Task.CompletedTask;
	}
}