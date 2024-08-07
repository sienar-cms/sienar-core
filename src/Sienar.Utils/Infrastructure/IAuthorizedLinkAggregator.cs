using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sienar.Infrastructure;

/// <summary>
/// Processes available <c>TLink</c> instances from the appropriate link provider and determines which links the current user is authorized to view
/// </summary>
/// <typeparam name="TLink">the type of the link, which must inherit <see cref="DashboardLink"/></typeparam>
public interface IAuthorizedLinkAggregator<TLink>
	where TLink : DashboardLink
{
	/// <summary>
	/// Creates a list of authorized <c>TLink</c> instances to be rendered
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	Task<List<TLink>> Create(string name);
}