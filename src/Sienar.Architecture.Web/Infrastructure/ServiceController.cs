using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sienar.Data;
using Sienar.Services;

namespace Sienar.Infrastructure;

/// <summary>
/// A controller that wraps around Sienar services to return information from an API endpoint in a consistent way
/// </summary>
public class ServiceController : ControllerBase
{
	private readonly IReadableNotificationService _notifier;

	/// <inheritdoc />
	protected ServiceController(IReadableNotificationService notifier)
	{
		_notifier = notifier;
	}

	/// <summary>
	/// Executes a service
	/// </summary>
	/// <param name="service">the service to execute</param>
	/// <param name="request">the request data</param>
	/// <typeparam name="TRequest">the type of the request</typeparam>
	/// <typeparam name="TResult">the type of the result</typeparam>
	/// <returns>the action result</returns>
	protected async Task<IActionResult> ExecuteService<TRequest, TResult>(
		IService<TRequest, TResult> service,
		TRequest request)
	{
		var result = await service.Execute(request);
		return CreateResult(result);
	}

	/// <summary>
	/// Executes a status service
	/// </summary>
	/// <param name="service">the service to execute</param>
	/// <param name="request">the request data</param>
	/// <typeparam name="TRequest">the type of the request</typeparam>
	/// <returns>the action result</returns>
	protected async Task<IActionResult> ExecuteService<TRequest>(
		IStatusService<TRequest> service,
		TRequest request)
	{
		var result = await service.Execute(request);
		return CreateResult(result);
	}

	/// <summary>
	/// Executes a result service
	/// </summary>
	/// <param name="service">the service to execute</param>
	/// <typeparam name="TResult">the type of the result</typeparam>
	/// <returns>the action result</returns>
	protected async Task<IActionResult> ExecuteService<TResult>(IResultService<TResult> service)
	{
		var result = await service.Execute();
		return CreateResult(result);
	}

	/// <summary>
	/// Creates an <see cref="WebResult{TResult}"/> based on the provided result
	/// </summary>
	/// <typeparam name="TResult">the type of the result</typeparam>
	/// <returns>the new <see cref="WebResult{TResult}"/></returns>
	protected IActionResult CreateResult<TResult>(OperationResult<TResult> result)
	{
		var webResult =  new WebResult<TResult>
		{
			Result = result.Result,
			Notifications = _notifier.Notifications.ToArray()
		};

		return new ObjectResult(webResult)
		{
			StatusCode = MapStatusCodeFromOperationStatus(result.Status)
		};
	}

	/// <summary>
	/// Maps a Sienar <see cref="OperationStatus"/> to an HTTP status code
	/// </summary>
	/// <param name="status">The status of the operation performed</param>
	/// <returns>the HTTP status code represented by the operation status</returns>
	protected static int MapStatusCodeFromOperationStatus(OperationStatus status)
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