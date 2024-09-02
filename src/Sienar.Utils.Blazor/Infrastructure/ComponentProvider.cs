#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;

namespace Sienar.Infrastructure;

/// <exclude />
public class ComponentProvider : IComponentProvider
{
	public Type? DefaultLayout { get; set; }
	public Type? Dashboard { get; set; }
	public Type? AppbarLeft { get; set; }
	public Type? AppbarRight { get; set; }
	public Type? SidebarHeader { get; set; }
	public Type? SidebarFooter { get; set; }
}