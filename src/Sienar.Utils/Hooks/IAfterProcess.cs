using System.Threading.Tasks;

namespace Sienar.Hooks;

/// <summary>
/// Performs arbitrary actions after a processor has already run
/// </summary>
/// <typeparam name="TRequest">the type of the request or entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IAfterProcess<TRequest>
{
	/// <summary>
	/// Performs arbitrary actions after a processor has already run
	/// </summary>
	/// <param name="request">the request or entity</param>
	/// <param name="action">the action type</param>
	Task Handle(TRequest request, ActionType action);
}