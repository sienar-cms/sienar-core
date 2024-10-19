using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Services;

/// <inheritdoc />
public class StateValidatorService<T> : IStateValidatorService<T>
{
	private readonly IEnumerable<IStateValidator<T>> _validators;
	private readonly ILogger<IStateValidatorService<T>> _logger;

	/// <exclude />
	public StateValidatorService(
		IEnumerable<IStateValidator<T>> validators,
		ILogger<IStateValidatorService<T>> logger)
	{
		_validators = validators;
		_logger = logger;
	}

	/// <inheritdoc />
	public async Task<OperationResult<bool>> Validate(
		T input,
		ActionType action)
	{
		var wasSuccessful = true;
		string? validationMessage = null;

		try
		{
			foreach (var validator in _validators)
			{
				if (await validator.Validate(input, action) != OperationStatus.Success) wasSuccessful = false;
			}
		}
		catch (Exception e)
		{
			_logger.LogError(e, "One or more state validators failed to run");
			wasSuccessful = false;
			validationMessage = StatusMessages.Processes.InvalidState;
		}

		return new(
			wasSuccessful ? OperationStatus.Success : OperationStatus.Unprocessable,
			wasSuccessful,
			validationMessage);
	}
}
