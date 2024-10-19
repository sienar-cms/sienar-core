#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sienar.Data;

namespace Sienar.Infrastructure;

/// <exclude />
public class OperationResultMapper : IOperationResultMapper
{
	private readonly IReadableNotificationService _notifier;
	private readonly IHttpContextAccessor _contextAccessor;

	public OperationResultMapper(
		IReadableNotificationService notifier,
		IHttpContextAccessor contextAccessor)
	{
		_notifier = notifier;
		_contextAccessor = contextAccessor;
	}

	public ObjectResult MapToObjectResult<T>(OperationResult<T> result)
	{
		if (!string.IsNullOrEmpty(result.Message))
		{
			if (result.Status is OperationStatus.Success)
			{
				_notifier.Success(result.Message);
			}
			else
			{
				_notifier.Error(result.Message);
			}
		}

		var webResult = new WebResult<T>
		{
			Result = result.Result,
			Notifications = _notifier.Notifications.ToArray()
		};

		return new ObjectResult(webResult)
		{
			StatusCode = MapStatusCodeFromOperationStatus(result.Status)
		};
	}

	private int MapStatusCodeFromOperationStatus(OperationStatus status)
		=> status switch
		{
			OperationStatus.Success => StatusCodes.Status200OK,
			OperationStatus.Unauthorized => _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false
				? StatusCodes.Status403Forbidden
				: StatusCodes.Status401Unauthorized,
			OperationStatus.NotFound => StatusCodes.Status404NotFound,
			OperationStatus.Concurrency => StatusCodes.Status409Conflict,
			OperationStatus.Conflict => StatusCodes.Status409Conflict,
			OperationStatus.Unprocessable => StatusCodes.Status422UnprocessableEntity,
			_ => StatusCodes.Status500InternalServerError
		};
}
