using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sienar.Data;

namespace Sienar.Infrastructure;

/// <summary>
/// A controller that wraps around Sienar services to return information from an API endpoint in a consistent way
/// </summary>
public class ServiceController : ControllerBase
{
	private readonly IOperationResultMapper _mapper;

	/// <inheritdoc />
	protected ServiceController(IOperationResultMapper mapper)
	{
		_mapper = mapper;
	}

	/// <summary>
	/// Executes an arbitrary function returning an operation result and maps the result to an ASP.NET <see cref="ObjectResult"/>
	/// </summary>
	/// <param name="action"></param>
	/// <typeparam name="TResult"></typeparam>
	/// <returns></returns>
	protected async Task<IActionResult> Execute<TResult>(
		Func<Task<OperationResult<TResult>>> action)
	{
		var result = await action();
		return _mapper.MapToObjectResult(result);
	}
}