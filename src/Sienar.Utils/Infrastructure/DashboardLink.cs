using System.Collections.Generic;

namespace Sienar.Infrastructure;

/// <summary>
/// Contains all the data needed to create a dashboard link
/// </summary>
/// <remarks>
/// Developers should not render dashboard links provided directly from the <see cref="IDashboardProvider"/>. Instead, they should process links with the <see cref="IDashboardGenerator"/> first because <see cref="IDashboardGenerator"/> excludes links for which the user does not meet the requirements to view.
/// </remarks>
public class DashboardLink
{
	/// <summary>
	/// The display text of the link
	/// </summary>
	public required string Text { get; set; }

	/// <summary>
	/// The URL the link points to
	/// </summary>
	public string? Url { get; set; }

	/// <summary>
	/// The icon to show along with the link, if any
	/// </summary>
	/// <remarks>
	/// If using the default Sienar UI, this property should be an SVG string. Sienar uses <see href="https://mudblazor.com/features/icons#icons">MudBlazor icons</see> internally, but this is not required as long as a valid SVG string is used. If providing your own UI, you can use this property however you see fit. For example, you might use FontAwesome icon identifiers, such as <c>fas fa-times</c>.
	/// </remarks>
	public string? Icon { get; set; }

	/// <summary>
	/// Whether the authorization requirements stored in <see cref="Roles"/> should be satisfied by all roles in the list being present, or only by a single role being present. Defaults to <c>true</c>, which requires all roles to be present.
	/// </summary>
	public bool AllRolesRequired { get; set; } = true;

	/// <summary>
	/// Whether the link should only be displayed if the user is logged in
	/// </summary>
	public bool RequireLoggedIn { get; set; }

	/// <summary>
	/// Whether the link should only be displayed if the user is logged out
	/// </summary>
	public bool RequireLoggedOut { get; set; }

	/// <summary>
	/// The role(s) required to see the link in the menu
	/// </summary>
	public IEnumerable<string>? Roles { get; set; }
}