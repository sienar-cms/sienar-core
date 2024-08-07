#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Extensions;
using Sienar.Data;
using Sienar.Hooks;
using Sienar.Infrastructure;
using Sienar.Processors;

namespace Sienar.Services;

/// <exclude />
public class Service<TRequest, TResult> : IService<TRequest, TResult>
{
	private readonly ILogger<Service<TRequest, TResult>> _logger;
	private readonly IEnumerable<IAccessValidator<TRequest>> _accessValidators;
	private readonly IEnumerable<IStateValidator<TRequest>> _stateValidators;
	private readonly IEnumerable<IBeforeProcess<TRequest>> _beforeHooks;
	private readonly IEnumerable<IAfterProcess<TRequest>> _afterHooks;
	private readonly IProcessor<TRequest, TResult> _processor;
	private readonly INotificationService _notifier;

	public Service(
		ILogger<Service<TRequest, TResult>> logger,
		IEnumerable<IAccessValidator<TRequest>> accessValidators,
		IEnumerable<IStateValidator<TRequest>> stateValidators,
		IEnumerable<IBeforeProcess<TRequest>> beforeHooks,
		IEnumerable<IAfterProcess<TRequest>> afterHooks,
		IProcessor<TRequest, TResult> processor,
		INotificationService notifier)
	{
		_logger = logger;
		_accessValidators = accessValidators;
		_stateValidators = stateValidators;
		_beforeHooks = beforeHooks;
		_afterHooks = afterHooks;
		_processor = processor;
		_notifier = notifier;
	}

	public virtual async Task<OperationResult<TResult?>> Execute(TRequest request)
	{
		if (!await _accessValidators.Validate(request, ActionType.Action, _logger))
		{
			return ProcessResult(new(OperationStatus.Unauthorized));
		}

		if (!await _stateValidators.Validate(request, ActionType.Action, _logger))
		{
			return ProcessResult(
				new(
					OperationStatus.Unprocessable,
					default,
					StatusMessages.Processes.InvalidState));
		}

		if (!await _beforeHooks.Run(request, ActionType.Action, _logger))
		{
			return ProcessResult(
				new(
					OperationStatus.Unprocessable,
					default,
					StatusMessages.Processes.BeforeHookFailure));
		}

		OperationResult<TResult?> result;
		try
		{
			result = await _processor.Process(request);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "{type} failed to process", typeof(IProcessor<TRequest, TResult>));
			return ProcessResult(new(OperationStatus.Unknown));
		}

		if (result.Status is OperationStatus.Success)
		{
			await _afterHooks.Run(request, ActionType.Action, _logger);
		}

		return result;
	}

	private OperationResult<TResult?> ProcessResult(OperationResult<TResult?> result)
	{
		if (result.Status is OperationStatus.Success)
		{
			_notifier.Success(result.Message);
		}
		else
		{
			_notifier.Error(result.Message);
		}

		return result;
	}
}