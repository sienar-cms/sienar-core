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
	/// Runs all before-process hooks
	/// </summary>
	/// <param name="beforeHooks">the before-process hooks</param>
	/// <param name="entity">the entity or request model</param>
	/// <param name="action">the type of action for the current operation</param>
	/// <param name="logger">the current logger</param>
	/// <typeparam name="TEntity">the type of the entity or request</typeparam>
	/// <returns>whether all hooks ran successfully or not</returns>
	public static async Task<bool> Run<TEntity>(
		this IEnumerable<IBeforeProcess<TEntity>> beforeHooks,
		TEntity entity,
		ActionType action,
		ILogger logger)
	{
		try
		{
			foreach (var beforeHook in beforeHooks)
			{
				await beforeHook.Handle(entity, action);
			}
		}
		catch (Exception e)
		{
			logger.LogError(
				e,
				"One or more before {action} hooks failed to run",
				action);
			return false;
		}

		return true;
	}

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

	/// <summary>
	/// Runs all state validation hooks
	/// </summary>
	/// <param name="stateValidators">the state validation hooks</param>
	/// <param name="entity">the entity or request model</param>
	/// <param name="action">the type of action for the current operation</param>
	/// <param name="logger">the current logger</param>
	/// <typeparam name="TEntity">the type of the entity or request</typeparam>
	/// <returns>whether all hooks ran successfully or not</returns>
	public static async Task<bool> Validate<TEntity>(
		this IEnumerable<IStateValidator<TEntity>> stateValidators,
		TEntity entity,
		ActionType action,
		ILogger logger)
	{
		try
		{
			var wasSuccessful = true;
			foreach (var validator in stateValidators)
			{
				if (await validator.Validate(entity, action) != OperationStatus.Success) wasSuccessful = false;
			}

			return wasSuccessful;
		}
		catch (Exception e)
		{
			logger.LogError(e, "One or more state validators failed to run");
			return false;
		}
	}

	/// <summary>
	/// Runs all access validation hooks
	/// </summary>
	/// <param name="accessValidators">the access validation hooks</param>
	/// <param name="entity">the entity or request model</param>
	/// <param name="action">the type of action for the current operation</param>
	/// <param name="logger">the current logger</param>
	/// <typeparam name="TEntity">the type of the entity or request</typeparam>
	/// <returns>whether all hooks ran successfully or not</returns>
	public static async Task<bool> Validate<TEntity>(
		this IEnumerable<IAccessValidator<TEntity>> accessValidators,
		TEntity? entity,
		ActionType action,
		ILogger logger)
	{
		var context = new AccessValidationContext();
		var anyValidators = false;

		try
		{
			foreach (var validator in accessValidators)
			{
				anyValidators = true;
				await validator.Validate(context, action, entity);
			}
		}
		catch (Exception e)
		{
			logger.LogError(e, "One or more access validators failed to run");
			return false;
		}

		return !anyValidators || context.CanAccess;
	}
}