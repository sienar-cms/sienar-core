using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Sienar.Infrastructure;

/// <summary>
/// A Blazor-based implementation of <see cref="IUserAccessor"/>
/// </summary>
/// <remarks>
/// This version of the <see cref="IUserAccessor"/> internally uses the <see cref="AuthenticationStateProvider"/> to access user information. 
/// </remarks>
public class BlazorUserAccessor : IUserAccessor
{
	private AuthenticationStateProvider _authStateProvider;

	/// <summary>
	/// Creates a new instance of <c>BlazorUserAccessor</c>
	/// </summary>
	/// <param name="authStateProvider">the <see cref="AuthenticationStateProvider"/></param>
	public BlazorUserAccessor(AuthenticationStateProvider authStateProvider)
	{
		_authStateProvider = authStateProvider;
	}

	/// <exclude />
	public async Task<bool> IsSignedIn()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		return state.User.Identity?.IsAuthenticated ?? false;
	}

	/// <exclude />
	public async Task<Guid?> GetUserId()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		var claim = state.User.Claims.FirstOrDefault(
			c => c.Type == ClaimTypes.NameIdentifier);
		return claim is null
			? null
			: Guid.Parse(claim.Value);
	}

	/// <exclude />
	public async Task<string?> GetUsername()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		return state.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
	}

	/// <exclude />
	public async Task<ClaimsPrincipal> GetUserClaimsPrincipal()
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		return state.User;
	}

	/// <exclude />
	public async Task<bool> UserInRole(string roleName)
	{
		var state = await _authStateProvider.GetAuthenticationStateAsync();
		return state.User.IsInRole(roleName);
	}
}