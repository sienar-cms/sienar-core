using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Infrastructure;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Extensions;

/// <summary>
/// Contains utilities for executing hooks
/// </summary>
public static class HooksExtensions
{
	/// <summary>
	/// Runs all after-process hooks
	/// </summary>
	/// <param name="afterHooks">the after-process hooks</param>
	/// <param name="entity">the entity or request model</param>
	/// <param name="action">the type of action for the current operation</param>
	/// <param name="logger">the current logger</param>
	/// <typeparam name="TEntity">the type of the entity or request</typeparam>
	public static async Task Run<TEntity>(
		this IEnumerable<IAfterProcess<TEntity>> afterHooks,
		TEntity entity,
		ActionType action,
		ILogger logger)
	{
		foreach (var afterHook in afterHooks)
		{
			try
			{
				await afterHook.Handle(entity, action);
			}
			catch (Exception e)
			{
				logger.LogError(
					e,
					"One or more after {action} hooks failed to run",
					action);
			}
		}
	}
}