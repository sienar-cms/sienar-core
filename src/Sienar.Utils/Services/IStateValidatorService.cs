using System.Threading.Tasks;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Services;

/// <summary>
/// Runs state validation hooks for a hookable request
/// </summary>
/// <typeparam name="T">the type of the request or entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IStateValidatorService<T>
{
	/// <summary>
	/// Runs all state validation hooks for a hookable request
	/// </summary>
	/// <param name="input">the request or entity</param>
	/// <param name="action">the action type</param>
	/// <returns>an operation result representing whether the hooks indicate the state is valid</returns>
	Task<OperationResult<bool>> Validate(
		T input,
		ActionType action);
}
