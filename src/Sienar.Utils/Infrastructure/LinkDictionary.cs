using System.Collections.Generic;

namespace Sienar.Infrastructure;

/// <summary>
/// A dictionary that contains a list of links at different <see cref="MenuPriority"/> keys
/// </summary>
/// <typeparam name="TLink">the type of the link</typeparam>
public class LinkDictionary<TLink> : Dictionary<MenuPriority, List<TLink>>
{
	/// <summary>
	/// Adds a navigation link at the given priority
	/// </summary>
	/// <param name="link">The nav link to add</param>
	/// <param name="priority">The priority at which to add the nav link</param>
	public LinkDictionary<TLink> AddLink(
		TLink link,
		MenuPriority priority = MenuPriority.Normal)
	{
		if (!TryGetValue(priority, out var menuItems))
		{
			menuItems = [];
			this[priority] = menuItems;
		}

		menuItems.Add(link);

		return this;
	}
}