using System.Threading.Tasks;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Processors;

// ReSharper disable once TypeParameterCanBeVariant
/// <summary>
/// A processor which accepts a <c>TRequest</c> as input and returns a <see cref="OperationResult{TResult}"/>
/// </summary>
/// <typeparam name="TRequest">the type of the processor input</typeparam>
/// <typeparam name="TResult">the type of the processor output</typeparam>
public interface IProcessor<TRequest, TResult>
{
	/// <summary>
	/// Processes the request and generates the result
	/// </summary>
	/// <param name="request">the request input</param>
	/// <returns>the result of the operation</returns>
	Task<OperationResult<TResult?>> Process(TRequest request);
}

// ReSharper disable once TypeParameterCanBeVariant
/// <summary>
/// A processor which accepts no input and returns a <see cref="OperationResult{TResult}"/>
/// </summary>
/// <typeparam name="TResult">the type of the processor output</typeparam>
public interface IProcessor<TResult>
{
	/// <summary>
	/// Processes the request and generates the result
	/// </summary>
	/// <returns>the result of the operation</returns>
	Task<OperationResult<TResult?>> Process();
}