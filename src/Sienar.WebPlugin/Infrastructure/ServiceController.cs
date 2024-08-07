using System.Threading.Tasks;
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
		return Ok(CreateResult(result));
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
		return Ok(CreateResult(result));
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
		return Ok(CreateResult(result));
	}

	/// <summary>
	/// Creates an <see cref="WebResult{TResult}"/> based on the provided result
	/// </summary>
	/// <typeparam name="TResult">the type of the result</typeparam>
	/// <returns>the new <see cref="WebResult{TResult}"/></returns>
	protected WebResult<TResult> CreateResult<TResult>(TResult result)
	{
		return new()
		{
			Result = result,
			Notifications = _notifier.Notifications.ToArray()
		};
	}
}