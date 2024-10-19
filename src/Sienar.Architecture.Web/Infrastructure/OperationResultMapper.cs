#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sienar.Data;

namespace Sienar.Infrastructure;

/// <exclude />
public class OperationResultMapper : IOperationResultMapper
{
	private readonly IReadableNotificationService _notifications;

	public OperationResultMapper(IReadableNotificationService notifications)
	{
		_notifications = notifications;
	}

	public ObjectResult MapToObjectResult<T>(OperationResult<T> result)
	{
		var webResult = new WebResult<T>
		{
			Result = result.Result,
			Notifications = _notifications.Notifications.ToArray()
		};

		return new ObjectResult(webResult)
		{
			StatusCode = MapStatusCodeFromOperationStatus(result.Status)
		};
	}

	private static int MapStatusCodeFromOperationStatus(OperationStatus status)
		=> status switch
		{
			OperationStatus.Success => StatusCodes.Status200OK,
			OperationStatus.Unauthorized => StatusCodes.Status401Unauthorized,
			OperationStatus.Forbidden => StatusCodes.Status403Forbidden,
			OperationStatus.NotFound => StatusCodes.Status404NotFound,
			OperationStatus.Concurrency => StatusCodes.Status409Conflict,
			OperationStatus.Conflict => StatusCodes.Status409Conflict,
			OperationStatus.Unprocessable => StatusCodes.Status422UnprocessableEntity,
			_ => StatusCodes.Status500InternalServerError
		};
}
