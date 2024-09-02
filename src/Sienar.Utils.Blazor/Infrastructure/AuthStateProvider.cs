using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace Sienar.Infrastructure;

/// <summary>
/// A WASM-friendly implementation of <see cref="AuthenticationStateProvider"/>
/// </summary>
public class AuthStateProvider : AuthenticationStateProvider
{
	/// <ignore />
	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		return CreateAuthenticationState();
	}

	private static AuthenticationState CreateAuthenticationState()
	{
		return new(new ClaimsPrincipal());
	}
}