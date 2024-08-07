using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sienar.Infrastructure;

/// <summary>
/// Accesses the current user's information
/// </summary>
public interface IUserAccessor
{
	/// <summary>
	/// Determines whether the current user is currently logged in
	/// </summary>
	/// <returns>the login status</returns>
	Task<bool> IsSignedIn();

	/// <summary>
	/// Gets the GUID of the currently logged in user, if any
	/// </summary>
	/// <returns>the GUID</returns>
	Task<Guid?> GetUserId();

	/// <summary>
	/// Gets the username of the currently logged in user, if any
	/// </summary>
	/// <returns>the username</returns>
	Task<string?> GetUsername();

	/// <summary>
	/// Gets the <see cref="ClaimsPrincipal"/> of the currently logged in user
	/// </summary>
	/// <returns>the <see cref="ClaimsPrincipal"/></returns>
	Task<ClaimsPrincipal> GetUserClaimsPrincipal();

	/// <summary>
	/// Determines whether the currently logged in user is in the given role
	/// </summary>
	/// <param name="roleName"></param>
	/// <returns></returns>
	Task<bool> UserInRole(string roleName);
}