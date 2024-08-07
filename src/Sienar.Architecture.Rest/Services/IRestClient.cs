using System.Net.Http;
using System.Threading.Tasks;
using Sienar.Data;

namespace Sienar.Services;

public interface IRestClient
{
	/// <summary>
	/// Sends an HTTP GET request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	Task<OperationResult<TResult>> Get<TResult>(
		string endpoint,
		object? input = null);

	/// <summary>
	/// Sends an HTTP POST request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	Task<OperationResult<TResult>> Post<TResult>(
		string endpoint,
		object input);

	/// <summary>
	/// Sends an HTTP PUT request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	Task<OperationResult<TResult>> Put<TResult>(
		string endpoint,
		object input);

	/// <summary>
	/// Sends an HTTP PATCH request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	Task<OperationResult<TResult>> Patch<TResult>(
		string endpoint,
		object input);

	/// <summary>
	/// Sends an HTTP DELETE request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	Task<OperationResult<TResult>> Delete<TResult>(
		string endpoint,
		object? input = null);

	/// <summary>
	/// Sends an HTTP HEAD request with the specified data
	/// </summary>
	/// <remarks>
	/// Because HEAD requests do not return a payload, this method simply returns the <see cref="HttpResponseMessage"/> so the developer can parse the response headers in any way s/he sees fit.
	/// </remarks>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <returns>the HTTP response message</returns>
	Task<HttpResponseMessage> Head(
		string endpoint,
		object? input = null);

	/// <summary>
	/// Sends an HTTP request without parsing the response
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <param name="method">the HTTP method</param>
	/// <returns>the response message</returns>
	Task<HttpResponseMessage> SendRaw(
		string endpoint,
		object? input = null,
		HttpMethod? method = null);
}