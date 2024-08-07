#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading.Tasks;
using Sienar.Infrastructure;

namespace Sienar.Infrastructure;

/// <exclude />
public class MenuGenerator : AuthorizedLinkAggregator<MenuLink>, IMenuGenerator
{
	public MenuGenerator(
		IUserAccessor userAccessor,
		IMenuProvider menuProvider)
		: base(userAccessor, menuProvider) {}

	/// <inheritdoc />
	protected override async Task PerformAdditionalProcessing(MenuLink link)
	{
		if (link.Sublinks is not null)
		{
			link.Sublinks = await ProcessNavLinks(link.Sublinks);
		}
	}
}