﻿#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;
using Sienar.Hooks;
using Sienar.Infrastructure;
using Sienar.Processors;

namespace Sienar.Services;

/// <exclude />
public class StatusService<TRequest> : IStatusService<TRequest>
{
	private readonly ILogger<StatusService<TRequest>> _logger;
	private readonly IAccessValidatorService<TRequest> _accessValidator;
	private readonly IStateValidatorService<TRequest> _stateValidator;
	private readonly IBeforeProcessService<TRequest> _beforeHooks;
	private readonly IAfterProcessService<TRequest> _afterHooks;
	private readonly IProcessor<TRequest, bool> _processor;
	private readonly INotificationService _notifier;

	public StatusService(
		ILogger<StatusService<TRequest>> logger,
		IAccessValidatorService<TRequest> accessValidator,
		IStateValidatorService<TRequest> stateValidator,
		IBeforeProcessService<TRequest> beforeHooks,
		IAfterProcessService<TRequest> afterHooks,
		IProcessor<TRequest, bool> processor,
		INotificationService notifier)
	{
		_logger = logger;
		_accessValidator = accessValidator;
		_stateValidator = stateValidator;
		_beforeHooks = beforeHooks;
		_afterHooks = afterHooks;
		_processor = processor;
		_notifier = notifier;
	}

	/// <inheritdoc />
	public virtual async Task<OperationResult<bool>> Execute(TRequest request)
	{
		// Run access validation
		var result = await _accessValidator.Validate(request, ActionType.StatusAction);
		if (!result.Result)
		{
			return ProcessResult(result);
		}

		// Run state validation
		result = await _stateValidator.Validate(request, ActionType.StatusAction);
		if (!result.Result)
		{
			return ProcessResult(result);
		}

		// Run before hooks
		result = await _beforeHooks.Run(request, ActionType.StatusAction);
		if (!result.Result)
		{
			return ProcessResult(result);
		}

		try
		{
			result = await _processor.Process(request);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "{type} failed to process", typeof(IProcessor<TRequest>));
			return ProcessResult(new(OperationStatus.Unknown));
		}

		if (result.Status is OperationStatus.Success)
		{
			await _afterHooks.Run(request, ActionType.StatusAction);
		}

		return ProcessResult(result);
	}

	private OperationResult<bool> ProcessResult(OperationResult<bool> result)
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