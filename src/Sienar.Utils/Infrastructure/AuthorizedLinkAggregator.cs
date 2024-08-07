#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sienar.Infrastructure;

/// <exclude />
public class AuthorizedLinkAggregator<TLink> : IAuthorizedLinkAggregator<TLink>
	where TLink : DashboardLink
{
	private readonly IUserAccessor _userAccessor;
	private readonly IDictionaryProvider<LinkDictionary<TLink>> _provider;

	public AuthorizedLinkAggregator(
		IUserAccessor userAccessor,
		IDictionaryProvider<LinkDictionary<TLink>> provider)
	{
		_userAccessor = userAccessor;
		_provider = provider;
	}

	/// <inheritdoc />
	public Task<List<TLink>> Create(string name)
	{
		var orderedLinks = new List<TLink>();
		var linkDictionary = _provider.Access(name);

		foreach (var i in linkDictionary.Keys.OrderDescending())
		{
			orderedLinks.AddRange(linkDictionary[i]);
		}

		return ProcessNavLinks(orderedLinks);
	}

	protected async Task<List<TLink>> ProcessNavLinks(List<TLink> navLinks)
	{
		var includedLinks = new List<TLink>();

		foreach (var link in navLinks)
		{
			if (!await UserIsAuthorized(link))
			{
				continue;
			}

			await PerformAdditionalProcessing(link);

			includedLinks.Add(link);
		}

		return includedLinks;
	}

	protected virtual Task PerformAdditionalProcessing(TLink link) => Task.CompletedTask;

	private async Task<bool> UserIsAuthorized(TLink menuLink)
	{
		if (menuLink.RequireLoggedIn && !await _userAccessor.IsSignedIn()) return false;
		if (menuLink.RequireLoggedOut && await _userAccessor.IsSignedIn()) return false;
		if (menuLink.Roles is null) return true;

		foreach (var role in menuLink.Roles)
		{
			if (await _userAccessor.UserInRole(role))
			{
				if (menuLink.AllRolesRequired)
				{
					continue;
				}

				return true;
			}

			if (menuLink.AllRolesRequired)
			{
				return false;
			}
		}

		return menuLink.AllRolesRequired;
	}
}