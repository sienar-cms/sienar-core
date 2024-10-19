#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Hooks;

namespace Sienar.Services;

/// <exclude />
public class AfterProcessService<T> : IAfterProcessService<T>
{
	private readonly IEnumerable<IAfterProcess<T>> _hooks;
	private readonly ILogger<IAfterProcessService<T>> _logger;

	public AfterProcessService(
		IEnumerable<IAfterProcess<T>> hooks,
		ILogger<IAfterProcessService<T>> logger)
	{
		_hooks = hooks;
		_logger = logger;
	}

	public async Task Run(T input, ActionType action)
	{
		foreach (var hook in _hooks)
		{
			try
			{
				await hook.Handle(input, action);
			}
			catch (Exception e)
			{
				_logger.LogError(
					e,
					"One or more after {action} hooks failed to run",
					action);
			}
		}
	}
}
