using System.Threading.Tasks;
using Sienar.Infrastructure;

namespace Sienar.Hooks;

/// <summary>
/// Performs access validation for a hookable request
/// </summary>
/// <typeparam name="T">the type of the request or entity</typeparam>
// ReSharper disable once TypeParameterCanBeVariant
public interface IAccessValidator<T>
{
	/// <summary>
	/// Performs access validation for a hookable request
	/// </summary>
	/// <param name="context">the access validation context</param>
	/// <param name="action">the action type</param>
	/// <param name="input">the request or entity</param>
	Task Validate(
		AccessValidationContext context,
		ActionType action,
		T? input);
}