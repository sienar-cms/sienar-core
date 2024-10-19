#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;
using Sienar.Hooks;
using Sienar.Infrastructure;

namespace Sienar.Services;

/// <exclude />
public class AccessValidatorService<T> : IAccessValidatorService<T>
{
	private readonly IEnumerable<IAccessValidator<T>> _validators;
	private readonly ILogger<IAccessValidatorService<T>> _logger;

	public AccessValidatorService(
		IEnumerable<IAccessValidator<T>> validators,
		ILogger<IAccessValidatorService<T>> logger)
	{
		_validators = validators;
		_logger = logger;
	}

	public async Task<OperationResult<bool>> Validate(T? input, ActionType action)
	{
		var context = new AccessValidationContext();
		var anyValidators = false;

		try
		{
			foreach (var validator in _validators)
			{
				anyValidators = true;
				await validator.Validate(context, action, input);
			}
		}
		catch (Exception e)
		{
			_logger.LogError(e, "One or more access validators failed to run");
			return new(
				OperationStatus.Unknown,
				false,
				StatusMessages.Processes.InvalidState);
		}

		var success = !anyValidators || context.CanAccess;
		return new(
			success ? OperationStatus.Success : OperationStatus.Forbidden,
			success);
	}
}
