using System.Collections.Generic;

namespace Sienar.Infrastructure;

/// <summary>
/// Contains all the data needed to create a menu link
/// </summary>
/// <remarks>
/// Developers should not render menu links provided directly from the <see cref="IMenuProvider"/>. Instead, they should process links with the <see cref="IMenuGenerator"/> first because <see cref="IMenuGenerator"/> excludes links for which the user does not meet the requirements to view.
/// </remarks>
public class MenuLink : DashboardLink
{
	/// <summary>
	/// Child links to display in a submenu, if any
	/// </summary>
	public List<MenuLink>? Sublinks;
}