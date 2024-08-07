using System.Net.Http;
using System.Threading.Tasks;

namespace Sienar.Services;

public interface IRestAuthClient
{
	/// <summary>
	/// Obtains an access token from the REST API
	/// </summary>
	/// <remarks>
	/// This method is not intended to perform a primary login. Rather, this method is intended to obtain credentials for a REST API (e.g., obtaining a BEARER token). Those credentials will be stored in-memory and added to an <see cref="HttpRequestMessage"/> with the <see cref="AddAuthorization"/> method. Primary login should be performed using a different mechanism (for example, a Sienar <c>IStatusService&lt;LoginRequest&gt;</c>, and will likely vary from API to API.
	/// </remarks>
	/// <returns><c>true</c> if the user was granted an access token, else <c>false</c></returns>
	Task<bool> RefreshAuthorization();

	/// <summary>
	/// Adds the REST API's authorization mechanism to the <see cref="HttpRequestMessage"/>
	/// </summary>
	/// <remarks>
	/// This method applies a REST API's authorization to a given <see cref="HttpRequestMessage"/>. For example, a Sienar REST API uses a BEARER token for authorization, so the Sienar implementation of this interface will add an <c>Authorization</c> HTTP header to the provided request.
	/// </remarks>
	/// <param name="request">the request to which to add authorization details</param>
	Task AddAuthorization(HttpRequestMessage request);

	/// <summary>
	/// Updates the app's authentication using the supplied HTTP response
	/// </summary>
	/// <remarks>
	/// This method observes a given <see cref="HttpResponseMessage"/> for changes to the app's underlying authentication method. For example, a Sienar REST API will emit an ASP.NET Core identity cookie, which will be updated periodically as the cookie nears expiration. This method is not required to do anything; web apps will probably use the default implementation, which simply returns <c>false</c>, because web apps authenticate using HTTP-only cookies, which are inaccessible to WASM. However, a MAUI Blazor desktop app will need to manually listen for cookie updates because while MAUI Blazor apps are built using web technologies, they don't contain a cookie jar because desktop apps don't typically make use of cookies.
	/// </remarks>
	/// <param name="response">the response from the HTTP call that may include an updated authentication record</param>
	/// <returns><c>true</c> if the authentication was updated, else <c>false</c></returns>
	Task<bool> UpdateAuthentication(HttpResponseMessage response) => Task.FromResult(false);
}