using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sienar.Data;

namespace Sienar.Services;

/// <summary>
/// A base class for services that interact with REST APIs
/// </summary>
public class RestClient : IRestClient
{
	protected readonly HttpClient Client;
	protected readonly IRestAuthClient AuthClient;
	protected readonly JsonSerializerOptions JsonOptions;

	/// <summary>
	/// The service's logger
	/// </summary>
	protected readonly ILogger<RestClient> Logger;

	/// <exclude />
	public RestClient(
		HttpClient client,
		IRestAuthClient authClient,
		ILogger<RestClient> logger)
	{
		Client = client;
		AuthClient = authClient;
		Logger = logger;
		JsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
	}

#region REST methods

	/// <summary>
	/// Sends an HTTP GET request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	public Task<OperationResult<TResult>> Get<TResult>(
		string endpoint,
		object? input = null)
		=> SendRequest<TResult>(endpoint, input, HttpMethod.Get);

	/// <summary>
	/// Sends an HTTP POST request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	public Task<OperationResult<TResult>> Post<TResult>(
		string endpoint,
		object input)
		=> SendRequest<TResult>(endpoint, input, HttpMethod.Post);

	/// <summary>
	/// Sends an HTTP PUT request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	public Task<OperationResult<TResult>> Put<TResult>(
		string endpoint,
		object input)
		=> SendRequest<TResult>(endpoint, input, HttpMethod.Put);

	/// <summary>
	/// Sends an HTTP PATCH request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	public Task<OperationResult<TResult>> Patch<TResult>(
		string endpoint,
		object input)
		=> SendRequest<TResult>(endpoint, input, HttpMethod.Patch);

	/// <summary>
	/// Sends an HTTP DELETE request with the specified data
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>the response wrapped with an operation result</returns>
	public Task<OperationResult<TResult>> Delete<TResult>(
		string endpoint,
		object? input = null)
		=> SendRequest<TResult>(endpoint, input, HttpMethod.Delete);

	/// <summary>
	/// Sends an HTTP HEAD request with the specified data
	/// </summary>
	/// <remarks>
	/// Because HEAD requests do not return a payload, this method simply returns the <see cref="HttpResponseMessage"/> so the developer can parse the response headers in any way s/he sees fit.
	/// </remarks>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <returns>the HTTP response message</returns>
	public Task<HttpResponseMessage> Head(
		string endpoint,
		object? input = null)
		=> SendRaw(endpoint, input, HttpMethod.Head);

#endregion

	/// <summary>
	/// Sends an HTTP request and parses the response
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <param name="method">the HTTP method</param>
	/// <typeparam name="TResult">the type of the response</typeparam>
	/// <returns>an operation result wrapping around the result</returns>
	protected async Task<OperationResult<TResult>> SendRequest<TResult>(
		string endpoint,
		object? input = null,
		HttpMethod? method = null)
	{
		try
		{
			var result = await SendRaw(endpoint, input, method);
			if (result.IsSuccessStatusCode)
			{
				var parsedResponse = await result.Content.ReadFromJsonAsync<TResult>(JsonOptions);
				if (parsedResponse is not null)
				{
					return new(OperationStatus.Success, parsedResponse);
				}

				return new(
					OperationStatus.Unknown,
					default,
					"The request was successful, but the server's response was not understood.");
			}

			if (result.StatusCode == HttpStatusCode.Unauthorized)
			{
				return new(
					OperationStatus.Unauthorized,
					message: StatusMessages.General.Unauthorized);
			}

			return HandleFailureResponse<TResult>(result);
		}
		catch (Exception e)
		{
			return HandleException<TResult>(e);
		}
	}

	/// <summary>
	/// Sends an HTTP request without parsing the response
	/// </summary>
	/// <param name="endpoint">the destination URL</param>
	/// <param name="input">the request payload, if any</param>
	/// <param name="method">the HTTP method</param>
	/// <returns>the response message</returns>
	public async Task<HttpResponseMessage> SendRaw(
		string endpoint,
		object? input = null,
		HttpMethod? method = null)
	{
		method ??= HttpMethod.Get;
		HttpResponseMessage result;
		var tries = 1;

		// Try to send the request and get a response
		// If the response indicates the user is unathorized,
		// refresh the authorization mechanism and repeat the request
		while (true)
		{
			var message = CreateRequestMessage(method, endpoint, input);
			await AuthClient.AddAuthorization(message);
			result = await Client.SendAsync(message);

			// If we've already been through once, or if the request was authorized, we're done
			if (tries > 1
				|| result.StatusCode != HttpStatusCode.Unauthorized)
			{
				break;
			}
	
			// Otherwise, we re-authenticate to the API if we got a 401
			if (!await AuthClient.RefreshAuthorization())
			{
				// If a token was not re-issued, they need to re-authenticate
				return result;
			}

			// Increment and try again
			tries++;
		}

		// Listen for updates to the authentication mechanism
		await AuthClient.UpdateAuthentication(result);

		return result;
	}

	private HttpRequestMessage CreateRequestMessage(
		HttpMethod method,
		string endpoint,
		object? input = null)
	{
		var message = new HttpRequestMessage(method, endpoint);
		if (input is null)
		{
			return message;
		}

		if (method == HttpMethod.Get)
		{
			message.RequestUri = CreateQueryPayload(message, input);
		}
		else
		{
			message.Content = CreateContentPayload(input);
		}

		return message;
	}

	private StringContent CreateContentPayload(object input)
		=> new(
			JsonSerializer.Serialize(input, JsonOptions),
			Encoding.UTF8,
			"application/json");

	private static Uri CreateQueryPayload(HttpRequestMessage m, object input)
	{
		var sb = new StringBuilder(m.RequestUri!.OriginalString);
		sb.Append('?');

		var inputType = input.GetType();
		var defaultInstance = Activator.CreateInstance(inputType);

		foreach (var prop in inputType.GetProperties())
		{
			var instanceValue = prop.GetValue(input)?.ToString();
			var defaultValue = prop.GetValue(defaultInstance)?.ToString();
			if (instanceValue != defaultValue)
			{
				sb.Append($"{prop.Name}={instanceValue}&");
			}
		}

		return new(sb.ToString(), UriKind.Relative);
	}

	private OperationResult<TResult> HandleException<TResult>(Exception e)
	{
		string logMessage;
		string errorMessage;
		switch (e)
		{
			case HttpRequestException:
				logMessage = "Network error";
				errorMessage = StatusMessages.Rest.NetworkFailed;
				break;
			case TaskCanceledException:
				logMessage =  "Network request timed out";
				errorMessage = StatusMessages.Rest.NetworkTimeout;
				break;
			default:
				logMessage = StatusMessages.General.Unknown;
				errorMessage = StatusMessages.General.Unknown;
				break;
		}

		Logger.LogError(e, "{}", logMessage);
		return new(OperationStatus.Unknown, default, errorMessage);
	}

	private OperationResult<TResult> HandleFailureResponse<TResult>(
		HttpResponseMessage message)
	{
		string logMessage;
		string errorMessage;
		var status = OperationStatus.Unknown;

		// Use the status code to generate an error message
		switch (message.StatusCode)
		{
			case HttpStatusCode.BadRequest:
				logMessage = "The request payload was not understood";
				errorMessage = StatusMessages.Rest.BadRequest;
				break;
			case HttpStatusCode.Unauthorized:
				logMessage = "Unauthorized user";
				errorMessage = StatusMessages.General.Unauthorized;
				break;
			case HttpStatusCode.UnprocessableEntity:
				logMessage = "There was a problem with the request data";
				errorMessage = StatusMessages.General.Unprocessable;
				break;
			default:
				logMessage = StatusMessages.General.Unknown;
				errorMessage = StatusMessages.General.Unknown;
				break;
		}

		Logger.LogError("{}", logMessage);

		return new(status, default, errorMessage);
	}
}