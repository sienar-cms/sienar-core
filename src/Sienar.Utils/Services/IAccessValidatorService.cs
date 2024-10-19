using System.Threading.Tasks;
using Sienar.Data;
using Sienar.Hooks;

namespace Sienar.Services;

/// <summary>
/// Runs access validation hooks for a hookable request
/// </summary>
/// <typeparam name="T">the type of the request or entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IAccessValidatorService<T>
{
	/// <summary>
	/// Runs all access validation hooks for a hookable request
	/// </summary>
	/// <param name="input">the request or entity</param>
	/// <param name="action">the action type</param>
	/// <returns>an operation result representing whether the hooks indicate validation passed</returns>
	Task<OperationResult<bool>> Validate(
		T? input,
		ActionType action);
}
