#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Services;

/// <exclude />
public class BeforeProcessService<T> : IBeforeProcessService<T>
{
	private readonly IEnumerable<IBeforeProcess<T>> _hooks;
	private readonly ILogger<IBeforeProcessService<T>> _logger;

	public BeforeProcessService(
		IEnumerable<IBeforeProcess<T>> hooks,
		ILogger<IBeforeProcessService<T>> logger)
	{
		_hooks = hooks;
		_logger = logger;
	}

	public async Task<OperationResult<bool>> Run(
		T input,
		ActionType action)
	{
		try
		{
			foreach (var hook in _hooks)
			{
				await hook.Handle(input, action);
			}
		}
		catch (Exception e)
		{
			_logger.LogError(
				e,
				"One or more before {action} hooks failed to run",
				action);
			return new(
				OperationStatus.Unknown,
				false,
				StatusMessages.Processes.BeforeHookFailure);
		}

		return new(OperationStatus.Success, true);
	}
}
