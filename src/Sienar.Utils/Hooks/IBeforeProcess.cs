using System.Threading.Tasks;

namespace Sienar.Hooks;

/// <summary>
/// Performs arbitrary actions before a processor has run
/// </summary>
/// <typeparam name="TRequest">the type of the request or entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IBeforeProcess<TRequest>
{
	/// <summary>
	/// Performs arbitrary actions before a processor has run
	/// </summary>
	/// <param name="request">the request or entity</param>
	/// <param name="action">the action type</param>
	Task Handle(TRequest request, ActionType action);
}