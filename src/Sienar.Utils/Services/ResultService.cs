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
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class ResultService<TResult> : IResultService<TResult>
{
	private readonly ILogger<ResultService<TResult>> _logger;
	private readonly IEnumerable<IAccessValidator<TResult>> _accessValidators;
	private readonly IEnumerable<IAfterProcess<TResult>> _afterHooks;
	private readonly IProcessor<TResult> _processor;
	private readonly INotificationService _notifier;

	public ResultService(
		ILogger<ResultService<TResult>> logger,
		IEnumerable<IAccessValidator<TResult>> accessValidators,
		IEnumerable<IAfterProcess<TResult>> afterHooks,
		IProcessor<TResult> processor,
		INotificationService notifier)
	{
		_logger = logger;
		_accessValidators = accessValidators;
		_afterHooks = afterHooks;
		_processor = processor;
		_notifier = notifier;
	}

	public virtual async Task<OperationResult<TResult?>> Execute()
	{
		if (!await _accessValidators.Validate(default, ActionType.ResultAction, _logger))
		{
			return ProcessResult(new(OperationStatus.Unauthorized));
		}

		OperationResult<TResult?> result;
		try
		{
			result = await _processor.Process();
		}
		catch (Exception e)
		{
			_logger.LogError(e, "{type} failed to process", typeof(IProcessor<TResult>));
			return ProcessResult(new(OperationStatus.Unknown));
		}

		if (result.Status is OperationStatus.Success && result.Result is not null)
		{
			await _afterHooks.Run(result.Result, ActionType.ResultAction, _logger);
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