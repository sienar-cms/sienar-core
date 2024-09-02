using System;

namespace Sienar.Infrastructure;

/// <summary>
/// A provider to contain references to various components to render in the Sienar UI
/// </summary>
public interface IComponentProvider
{
	/// <summary>
	/// The default layout component to use when no layout is specified
	/// </summary>
	public Type? DefaultLayout { get; set; }

	/// <summary>
	/// A component to render in place of the default dashboard UI
	/// </summary>
	public Type? Dashboard { get; set; }

	/// <summary>
	/// A component to render on the left side of the appbar on large web screens
	/// </summary>
	public Type? AppbarLeft { get; set; }

	/// <summary>
	/// A component to render on the right side of the appbar on large web screens
	/// </summary>
	public Type? AppbarRight { get; set; }

	/// <summary>
	/// A component to render at the top of the sidebar on dashboard screens
	/// </summary>
	public Type? SidebarHeader { get; set; }

	/// <summary>
	/// A component to render at the bottom of the sidebar on dashboard screens
	/// </summary>
	public Type? SidebarFooter { get; set; }
}