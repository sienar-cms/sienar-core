using System.Threading.Tasks;
using Sienar.Data;

namespace Sienar.Hooks;

/// <summary>
/// Verifies that the request or entity does not violate the application state prior to executing a process
/// </summary>
/// <typeparam name="TRequest">the type of the request or entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IStateValidator<TRequest>
{
	/// <summary>
	/// Validates that an entity does not violate logical rules of the app state (for example, checking fields for uniqueness against the database)
	/// </summary>
	/// <param name="request">the request or entity</param>
	/// <param name="action">the action type</param>
	Task<OperationStatus> Validate(TRequest request, ActionType action);
}