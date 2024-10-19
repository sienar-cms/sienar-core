using System.Threading.Tasks;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Services;

/// <summary>
/// Runs before hooks for a hookable request
/// </summary>
/// <typeparam name="T">the type of the request or entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IBeforeProcessService<T>
{
	/// <summary>
	/// Runs all before hooks for a hookable request
	/// </summary>
	/// <param name="input">the request or entity</param>
	/// <param name="action">the action type</param>
	/// <returns>an operation result representing whether the hooks allow the process to continue</returns>
	Task<OperationResult<bool>> Run(
		T input,
		ActionType action);
}
