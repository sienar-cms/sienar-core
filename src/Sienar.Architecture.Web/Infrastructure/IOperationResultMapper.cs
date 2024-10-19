using Microsoft.AspNetCore.Mvc;
using Sienar.Data;

namespace Sienar.Infrastructure;

/// <summary>
/// Maps <see cref="OperationResult{T}"/> objects to <see cref="ObjectResult"/> objects
/// </summary>
public interface IOperationResultMapper
{
	/// <summary>
	/// Maps an operation result to an ASP.NET <see cref="ObjectResult"/>
	/// </summary>
	/// <param name="result">the result to map</param>
	/// <typeparam name="T">the generic type of the operation result</typeparam>
	/// <returns>the object result</returns>
	ObjectResult MapToObjectResult<T>(OperationResult<T> result);
}
