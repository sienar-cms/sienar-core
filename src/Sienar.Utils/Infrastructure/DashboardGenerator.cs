using Sienar.Infrastructure;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Sienar.Infrastructure;

/// <exclude />
public class DashboardGenerator : AuthorizedLinkAggregator<DashboardLink>, IDashboardGenerator
{
	public DashboardGenerator(
		IUserAccessor userAccessor,
		IDashboardProvider dashboardProvider)
		: base(userAccessor, dashboardProvider) {}
}