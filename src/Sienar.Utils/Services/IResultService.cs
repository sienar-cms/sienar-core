using System.Threading.Tasks;
using Sienar.Data;

namespace Sienar.Services;

/// <summary>
/// A service that accepts no input and returns output
/// </summary>
/// <typeparam name="TResult">the type of the output</typeparam>
public interface IResultService<TResult>
{
	/// <summary>
	/// Executes the request
	/// </summary>
	/// <returns>the output of the operation, or <c>null</c></returns>
	Task<OperationResult<TResult?>> Execute();
}